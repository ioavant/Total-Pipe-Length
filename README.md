<div align="center">
  <img src="Total%20Pipe%20Length/Resources/sigma_pipe_200.png" width="120" alt="Total Pipe Length">
  <h1>Total Pipe Length</h1>
  <p><strong>A Revit add-in that instantly totals the length of selected pipes, ducts, conduits, cable trays and lines — grouped by category, in your project's own units.</strong></p>
  <p>
    <img src="https://img.shields.io/badge/Revit-2022%20%E2%80%93%202027-0696D7" alt="Revit 2022-2027">
    <img src="https://img.shields.io/badge/.NET%20Framework-4.8-512BD4" alt=".NET Framework 4.8">
    <img src="https://img.shields.io/badge/read--only-no%20model%20changes-2E7D32" alt="Read-only">
  </p>
  <p><a href="https://github.com/ioavant/Total-Pipe-Length/releases/latest"><strong>Download the installer &rarr;</strong></a></p>
</div>

---

Total Pipe Length answers a question every MEP modeler asks constantly: *"how much of this is there?"* Select any run of linear elements and get their combined length in one click — no schedule to build, no formula to write, no temporary view to set up.

<div align="center">
  <img src="Total%20Pipe%20Length/Resources/PipeLength3.png" width="700" alt="Result dialog with a per-category breakdown">
</div>

## Features

- **Measures every curve-based element type** — pipes, ducts, conduits, cable trays, flexible pipe/duct, and model or detail lines.
- **Two ways to work** — measure what is already selected, or click with an empty selection to pick elements on the fly through a filter that only allows measurable, linear elements.
- **Per-category breakdown** when several categories are selected (longest first) with a combined total underneath, so disciplines are never mixed into one meaningless number.
- **Horizontal / vertical split** — every total is also broken into horizontal runs and vertical runs (risers), so distribution and riser quantities separate automatically. A run counts as vertical when its rise exceeds its horizontal reach, i.e. it is steeper than 45°.
- **Your project's units** — results use the project's own length unit and accuracy (mm, m, or feet-and-inches), with the unit symbol always shown even when the project format has symbols turned off.
- **Completely read-only** — it never starts a transaction, never adds parameters, and never changes or stores anything in the model. Safe to run on live, workshared central models.

## Usage

1. *(Optional)* Select the linear elements you want to measure. Mixing categories is fine.
2. Click **Total Pipe Length** on the **Vixeldorf** ribbon tab.
3. With a selection, the result dialog appears immediately. With nothing selected, Revit enters a pick session that only allows linear elements — select them in the view and click **Finish**, or press <kbd>Esc</kbd> to cancel.
4. Read the total. When more than one category is selected, each category is listed separately with a combined total underneath.

Lengths follow the project's current length unit and accuracy — change **Manage → Project Units** to see the total in a different unit.

## Installation

Download `TotalPipeLengthSetup.msi` from the [latest release](https://github.com/ioavant/Total-Pipe-Length/releases/latest) and run it. The installer:

- copies the add-in DLL to `%ProgramData%\Vixeldorf\TotalPipeLength\`;
- registers a `.addin` manifest for each Revit version you select during setup — 2022–2026 under `%ProgramData%\Autodesk\Revit\Addins\{year}`, and 2027 under `%ProgramFiles%\Autodesk\Revit\Addins\2027` (Revit 2027 no longer scans ProgramData for machine-wide add-ins).

Restart Revit after installing to load the **Vixeldorf** ribbon tab.

**To uninstall**, open *Windows Settings → Apps → Installed apps* (or *Control Panel → Programs and Features*), select **Vixeldorf Total Pipe Length** and click **Uninstall**. This removes the DLL and every per-version `.addin` manifest. Restart Revit afterward to confirm the button is gone.

## Building from source

| Requirement | Notes |
|---|---|
| Visual Studio 2022 | with the .NET desktop workload |
| .NET Framework 4.8 | target framework of `TotalPipeLength.csproj` |
| Revit 2023 SDK assemblies | `RevitAPI.dll`, `RevitAPIUI.dll`, `AdWindows.dll` are referenced from `C:\Program Files\Autodesk\Revit 2023\`. A single build targeting the 2023 API runs on 2022–2027. |
| WiX Toolset v3 | only needed to build `Installer\Installer.wixproj` |

```
Total Pipe Length/         add-in sources
  App.cs                   IExternalApplication - ribbon tab, panel and buttons
  PipeLength.cs            IExternalCommand - selection, measurement, formatting
  OpenWebPage.cs           IExternalCommand - Help Center button
  Resources/               icons and screenshots (icons are embedded resources)
Installer/                 WiX v3 MSI project
  Product.wxs              product definition and install layout
  TotalPipeLength_2x.addin per-Revit-version manifests (2022-2027)
LISTING.md                 product listing copy (website + Autodesk App Store)
```

Open `Total Pipe Length\TotalPipeLength.sln`, build **Release**, then build the installer project to produce `Installer\bin\Release\TotalPipeLengthSetup.msi`.

## Notes and known limitations

- Only curve-based elements carry a length. Fittings, accessories and equipment are excluded, so a run's total does not include the length taken up by elbows, tees and similar fittings.
- The length reported for each element is its stored centerline length — the same value Revit shows in a schedule — rounded to the project's Length accuracy.
- In pick mode the selection filter intentionally allows only linear elements; non-linear elements cannot be picked for measurement.

## Support

Questions and bug reports: [yoav@vixeldorf.com](mailto:yoav@vixeldorf.com) or [vixeldorf.com](https://www.vixeldorf.com). Please include your Revit version, the add-in version, and a short description or screenshot of the issue — and, if relevant, the categories of the elements you were measuring.

## License

Copyright © Vixeldorf. Distributed under the end-user licence agreement included with the installer ([`Installer/License.rtf`](Installer/License.rtf)).
