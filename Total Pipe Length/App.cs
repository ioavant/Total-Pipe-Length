using Autodesk.Revit.UI;
using System;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace TotalPipeLength
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "Vixeldorf";
            CreateTabIfNotExists(application, tabName);

            RibbonPanel panel = GetOrCreatePanel(application, tabName, "Total Pipe Length");
            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            // Shared F1 (contextual) help target for every button.
            ContextualHelp help = new ContextualHelp(ContextualHelpType.Url, "https://www.vixeldorf.com");

            PushButtonData buttonData = new PushButtonData(
                "TotalPipeLengthBtn",
                "Total Pipe\nLength",
                assemblyPath,
                "TotalPipeLength.PipeLength");
            buttonData.SetContextualHelp(help);

            PushButton button = panel.AddItem(buttonData) as PushButton;
            if (button != null)
            {
                button.ToolTip = "Calculate total length of selected linear elements (pipes, ducts, conduits, cable trays, lines), grouped by category.";
                button.LargeImage = LoadImage("icon.png");
            }

            const string bimGuideKey = "Vixeldorf.BimGuide.Added";
            RibbonPanel aboutPanel = GetOrCreatePanel(application, tabName, "About");
            if (AppDomain.CurrentDomain.GetData(bimGuideKey) == null)
            {
                AppDomain.CurrentDomain.SetData(bimGuideKey, true);
                PushButtonData webButtonData = new PushButtonData(
                    "Vixeldorf_HelpCenter", "Help\nCenter",
                    assemblyPath, "TotalPipeLength.OpenWebPage");
                webButtonData.SetContextualHelp(help);
                PushButton webButton = aboutPanel.AddItem(webButtonData) as PushButton;
                if (webButton != null)
                {
                    webButton.ToolTip = "Open the Vixeldorf Help Center web page.";
                    webButton.LargeImage = LoadImage("web_icon.png");
                }
            }

            // Keep the "About" panel at the very end of the tab even when other Vixeldorf
            // add-ins add their panels later. Ribbon panels can't be reordered through a
            // supported API, and ApplicationInitialized can fire before the ribbon is fully
            // built (and never fires when the add-in loads after Revit has started), so we
            // reorder on the first Idling tick — raised once, when the ribbon is guaranteed
            // to exist and the reorder reliably sticks.
            _uiApp = application;
            application.Idling += OnFirstIdling;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private UIControlledApplication _uiApp;

        private void OnFirstIdling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {
            _uiApp.Idling -= OnFirstIdling;   // one-shot: only the first idle
            MoveAboutPanelToEnd();
        }

        // Uses the (unofficial but long-stable) Autodesk.Windows ribbon model to move the
        // shared "About" panel to the end of the "Vixeldorf" tab. Wrapped so it can never break startup.
        private static void MoveAboutPanelToEnd()
        {
            try
            {
                Autodesk.Windows.RibbonControl ribbon = Autodesk.Windows.ComponentManager.Ribbon;
                if (ribbon == null)
                    return;

                foreach (Autodesk.Windows.RibbonTab tab in ribbon.Tabs)
                {
                    if (tab.Title != "Vixeldorf" && tab.Id != "Vixeldorf")
                        continue;

                    Autodesk.Windows.RibbonPanel about = null;
                    foreach (Autodesk.Windows.RibbonPanel p in tab.Panels)
                    {
                        if (p.Source != null && p.Source.Title != null &&
                            p.Source.Title.Trim().Equals("About", StringComparison.OrdinalIgnoreCase))
                        {
                            about = p;
                            break;
                        }
                    }

                    if (about != null && tab.Panels[tab.Panels.Count - 1] != about)
                    {
                        tab.Panels.Remove(about);
                        tab.Panels.Add(about);
                    }
                    break;
                }
            }
            catch
            {
                // Never let a ribbon-cosmetics tweak interfere with loading.
            }
        }

        private void CreateTabIfNotExists(UIControlledApplication app, string tabName)
        {
            try { app.CreateRibbonTab(tabName); }
            catch (Autodesk.Revit.Exceptions.ArgumentException) { }
        }

        private RibbonPanel GetOrCreatePanel(UIControlledApplication app, string tabName, string panelName)
        {
            foreach (RibbonPanel panel in app.GetRibbonPanels(tabName))
                if (panel.Name == panelName) return panel;
            return app.CreateRibbonPanel(tabName, panelName);
        }

        private BitmapImage LoadImage(string resourceName)
        {
            string fullName = $"TotalPipeLength.Resources.{resourceName}";
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullName);
            if (stream == null) return null;
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            return image;
        }
    }
}

