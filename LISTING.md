# Total Pipe Length — product listing (source of truth for website + Autodesk App Store)

Plain-text / HTML-lite copy. The website project can restyle these facts to its
own design system; the same fields map 1:1 onto the Autodesk App Store form.

## App Name
Total Pipe Length

## App Short Description
Instantly totals the length of selected pipes, ducts, conduits, cable trays and lines in Revit — grouped by category, shown in your project's own units. Read-only: nothing is written to the model.

## App Description
Total Pipe Length answers a question every MEP modeler asks constantly: "how much of this is there?" Select any run of linear elements in Revit and get their combined length in one click — no schedule to build, no formula to write, no temporary view to set up.

HOW IT WORKS
Select the elements you want to measure and click Total Pipe Length on the Vixeldorf ribbon tab. A dialog shows the combined length immediately. If nothing is selected when you click, the add-in drops you straight into a pick session (it only lets you pick measurable, linear elements) — draw a selection, click Finish, and the total appears. Press Esc to cancel at any time.

When your selection spans more than one category — say pipes and cable trays together — the result is broken down per category (longest first) with a grand total underneath, so you can read each discipline's quantity at a glance.

KEY FEATURES
- Measures every curve-based element type: pipes, ducts, conduits, cable trays, flexible pipe/duct, and model or detail lines.
- Two ways to work: measure what's already selected, or click with an empty selection to pick elements on the fly with a filter that only allows linear elements.
- Per-category breakdown when several categories are selected, plus a combined total — no mixing of disciplines into one meaningless number.
- Splits the total into horizontal runs and vertical runs (risers), so distribution and riser quantities are separated automatically.
- Results in your project's own length unit, with the unit symbol always shown (mm, m, or feet-and-inches — whatever the project uses), rounded to the project's own accuracy setting.
- Completely read-only: it never starts a transaction, never adds parameters, and never changes or stores anything in the model. Safe to run on live, workshared central models.

WHO IT'S FOR
MEP modelers, coordinators, and estimators who need a fast, throwaway quantity — checking a run before ordering, sanity-checking a schedule, or answering "how many meters of conduit is on this level?" without touching the model.

Supported on Revit 2022 through 2027 (single installer, pick your versions during setup).

## App Version
**Version Number:** 1.0.2
**Version Description:** First public release. Totals the length of selected linear elements in Revit — pipes, ducts, conduits, cable trays, and model/detail lines. Measure an existing selection or pick elements on the fly; results are grouped by category with a combined total, split into horizontal and vertical runs, and shown in the project's own units. Entirely read-only — the tool never modifies the model.

## General Usage Instructions
1. (Optional) Select the linear elements you want to measure — pipes, ducts, conduits, cable trays, or lines. You can mix categories.
2. Click Total Pipe Length on the Vixeldorf ribbon tab.
3. If you had a selection, the result dialog appears immediately. If nothing was selected, Revit enters a pick session that only allows linear elements — select them in the view and click Finish (or press Esc to cancel).
4. Read the total in the dialog. When more than one category is selected, each category's length is listed separately (longest first) with a combined total underneath.
5. Lengths are shown in your project's current length unit and accuracy; change the project units (Manage > Project Units) to see the total in a different unit.

## Installation/Uninstallation
Total Pipe Length installs via a standard Windows Installer (MSI) package, TotalPipeLengthSetup.msi. The add-in DLL is copied to %ProgramData%\Vixeldorf\TotalPipeLength\, and a .addin manifest is registered for each Revit version you select during setup: 2022–2026 under %ProgramData%\Autodesk\Revit\Addins\{year}, and 2027 under %ProgramFiles%\Autodesk\Revit\Addins\2027 (Revit 2027 no longer scans ProgramData for machine-wide add-ins). Restart Revit after installing to load the "Vixeldorf" ribbon tab.

To uninstall, open Windows Settings > Apps > Installed apps (or Control Panel > Programs and Features), select "Vixeldorf Total Pipe Length", and click Uninstall. This removes the DLL and all per-version .addin manifests. Restart Revit afterward to confirm the ribbon button is gone.

## Support Information
For support, contact us at yoav@vixeldorf.com or via <a href="https://www.vixeldorf.com">vixeldorf.com</a>. Please include your Revit version, the Total Pipe Length version, and a short description or screenshot of the issue (and, if relevant, the categories of the elements you were measuring). We aim to respond within 5 business days.

## Additional Information
<b>Supported Revit versions:</b> 2022, 2023, 2024, 2025, 2026, 2027 (single 64-bit build).<br>
<b>Category:</b> MEP / quantities &amp; takeoff.<br>
<b>Requirements:</b> No third-party dependencies; safe in workshared (central) models.<br>
<b>Data storage:</b> None. Total Pipe Length is a read-only tool — it never starts a transaction, adds parameters, or writes to the model or any external store. It only reads element lengths and displays a total.<br>
<b>Publisher:</b> <a href="https://www.vixeldorf.com">Vixeldorf</a>.

## Known Issues
- Only curve-based elements are measured — straight and flexible pipe/duct/conduit/cable-tray runs and model/detail lines. Fittings, accessories, and equipment carry no length and are excluded, so a run's total does not include the length taken up by elbows, tees, and similar fittings.
- The length reported for each element is its stored centerline length — the same value Revit shows in a schedule — rounded to the project's Length unit accuracy.
- In pick mode (empty selection), the tool's filter intentionally allows only linear elements; non-linear elements cannot be picked for measurement.

## Learn More Url
https://www.vixeldorf.com

## Version History
- **1.0.2** — First public release. Total length of selected linear elements (pipes, ducts, conduits, cable trays, model/detail lines) with per-category breakdown, horizontal/vertical run split, project-unit formatting, and two selection modes (measure existing selection or pick on the fly). Read-only. Revit 2022–2027.
