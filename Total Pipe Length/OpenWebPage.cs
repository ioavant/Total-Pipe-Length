using System.Diagnostics;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace TotalPipeLength
{
    [Transaction(TransactionMode.Manual)]
    public class OpenWebPage : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Process.Start(new ProcessStartInfo(Brand.HelpUrl) { UseShellExecute = true });
            return Result.Succeeded;
        }
    }
}
