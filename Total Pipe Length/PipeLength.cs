using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TotalPipeLength
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class PipeLength : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication app = commandData.Application;
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;

            // Two selection modes:
            // 1) elements are already selected -> measure them
            // 2) nothing is selected -> let the user pick linear elements
            List<Element> elementsToMeasure = new List<Element>();
            ICollection<ElementId> preselected = sel.GetElementIds();

            if (preselected != null && preselected.Count > 0)
            {
                foreach (ElementId elemId in preselected)
                {
                    Element elem = doc.GetElement(elemId);
                    if (elem != null)
                        elementsToMeasure.Add(elem);
                }
            }
            else
            {
                try
                {
                    IList<Reference> picked = sel.PickObjects(
                        ObjectType.Element,
                        new LinearElementSelectionFilter(),
                        "Select linear elements to measure, then click Finish.");

                    foreach (Reference reference in picked)
                    {
                        Element elem = doc.GetElement(reference);
                        if (elem != null)
                            elementsToMeasure.Add(elem);
                    }
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    // User pressed Esc / cancelled the selection
                    return Result.Cancelled;
                }
            }

            // Accumulate length per category, and split the total into horizontal / vertical runs
            Dictionary<string, double> lengthByCategory = new Dictionary<string, double>();
            double totalLength = 0;
            double horizontalLength = 0;
            double verticalLength = 0;

            foreach (Element elem in elementsToMeasure)
            {
                if (elem.Category == null)
                    continue;

                double length = GetLength(elem);
                if (length <= 0)
                    continue;

                string categoryName = elem.Category.Name;
                if (lengthByCategory.ContainsKey(categoryName))
                    lengthByCategory[categoryName] += length;
                else
                    lengthByCategory[categoryName] = length;

                totalLength += length;

                if (IsVertical(elem))
                    verticalLength += length;
                else
                    horizontalLength += length;
            }

            if (lengthByCategory.Count == 0)
            {
                TaskDialog.Show("Total Length",
                    "No linear elements selected. Select pipes, ducts, conduits, cable trays or lines.");
                return Result.Succeeded;
            }

            Units units = doc.GetUnits();
            StringBuilder sb = new StringBuilder();

            // When several categories are selected, show a per-category breakdown
            if (lengthByCategory.Count > 1)
            {
                foreach (KeyValuePair<string, double> pair in lengthByCategory.OrderByDescending(p => p.Value))
                    sb.AppendLine($"{pair.Key}: {Format(units, pair.Value)}");

                sb.AppendLine();
            }

            sb.AppendLine($"Total: {Format(units, totalLength)}");
            sb.AppendLine($"Horizontal runs: {Format(units, horizontalLength)}");
            sb.Append($"Vertical runs: {Format(units, verticalLength)}");

            TaskDialog.Show("Total Length", sb.ToString());
            return Result.Succeeded;
        }

        // Length of an element in Revit internal units (feet); 0 if the element is not linear
        internal static double GetLength(Element elem)
        {
            // MEP curves (pipes, ducts, conduits, cable trays) and lines expose a built-in length parameter
            Parameter lengthParam = elem.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
            if (lengthParam != null && lengthParam.HasValue)
                return lengthParam.AsDouble();

            // Fallback: elements placed along a curve
            if (elem.Location is LocationCurve locationCurve && locationCurve.Curve != null)
                return locationCurve.Curve.Length;

            // Fallback: model and detail lines
            if (elem is CurveElement curveElem && curveElem.GeometryCurve != null)
                return curveElem.GeometryCurve.Length;

            return 0;
        }

        // The element's driving curve (used to judge orientation)
        private static Curve GetCurve(Element elem)
        {
            if (elem.Location is LocationCurve locationCurve && locationCurve.Curve != null)
                return locationCurve.Curve;

            if (elem is CurveElement curveElem && curveElem.GeometryCurve != null)
                return curveElem.GeometryCurve;

            return null;
        }

        // A run counts as vertical when its rise (Z) exceeds its horizontal reach
        // (i.e. it is steeper than 45°). Everything else — including sloped and flat
        // runs — counts as horizontal. Elements without a curve default to horizontal.
        private static bool IsVertical(Element elem)
        {
            Curve curve = GetCurve(elem);
            if (curve == null)
                return false;

            XYZ delta = curve.GetEndPoint(1) - curve.GetEndPoint(0);
            double rise = Math.Abs(delta.Z);
            double reach = Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
            return rise > reach;
        }

        // Format a length in the current project's units, always appending the unit symbol.
        // The project's own length format may have the unit symbol turned off, so we force one on
        // while keeping the project's unit and accuracy.
        private static string Format(Units units, double internalLength)
        {
            FormatOptions projectOptions = units.GetFormatOptions(SpecTypeId.Length);
            ForgeTypeId unitTypeId = projectOptions.GetUnitTypeId();

            FormatOptions options = new FormatOptions(unitTypeId);
            options.Accuracy = projectOptions.Accuracy;

            foreach (ForgeTypeId symbol in FormatOptions.GetValidSymbols(unitTypeId))
            {
                if (!symbol.Empty())
                {
                    options.SetSymbolTypeId(symbol);
                    break;
                }
            }

            FormatValueOptions valueOptions = new FormatValueOptions();
            valueOptions.SetFormatOptions(options);

            return UnitFormatUtils.Format(units, SpecTypeId.Length, internalLength, false, valueOptions);
        }
    }

    // Restricts interactive picking to linear (measurable) elements
    public class LinearElementSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem != null && elem.Category != null && PipeLength.GetLength(elem) > 0;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
