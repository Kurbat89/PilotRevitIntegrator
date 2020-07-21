using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using PilotRevitAddin.SynchronizeProject;
using PilotRevitAddin.Utils;
using System;
using System.Reflection;

namespace PilotRevitAddin
{
    //40A992C2-CD30-4F75-8632-FEE0A4B8B44C
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class ExternalApplication : IExternalApplication
    {
        private static readonly string AddInPath = typeof(ExternalApplication).Assembly.Location;
        private const string PilotIceTabName = "Pilot-ICE";

        private SynchronizeControl _synchronizeControl;

        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                CreatePilotTab(application);
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ribbon Sample", ex.ToString());
                return Result.Failed;
            }
        }

        private void CreatePilotTab(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentOpened += ControlledApplication_DocumentOpened;
            _synchronizeControl = new SynchronizeControl(application);
            
            application.CreateRibbonTab(PilotIceTabName);
            var pilotRibbon = application.CreateRibbonPanel(PilotIceTabName, "Панель команд Pilot-ICE");
            var prepareButton = new PushButtonData("PrepareButton", "Подготовить проект", AddInPath,
                "PilotRevitAddin.PrepareProjectCommand")
            {
                ToolTip = "Подготовить проект для совместной работы",
                LargeImage = ImageUtils.NewBitmapImage(Assembly.GetExecutingAssembly(), "prepareIcon.png"),
                AvailabilityClassName = "PilotRevitAddin.PrepareProjectCommandAvailability"
            };

            var startButton = new PushButtonData("StartButton", "Начать проектирование", AddInPath,
                "PilotRevitAddin.StartDesigningCommand")
            {
                ToolTip = "Начать проектирование в режиме совместной работы",
                LargeImage = ImageUtils.NewBitmapImage(Assembly.GetExecutingAssembly(), "startDesigningIcon.png"),
                AvailabilityClassName = "PilotRevitAddin.StartDesigningCommandAvailability"
            };

            var updateProjectSettingsButton = new PushButtonData("UpdateProjectButton", "Обновить настройки проекта", AddInPath,
              "PilotRevitAddin.UpdateProjectSettingsCommand")
            {
                ToolTip = "Обновить настройки проекта",
                LargeImage = ImageUtils.NewBitmapImage(Assembly.GetExecutingAssembly(), "updateSettingsIcon.png"),
                AvailabilityClassName = "PilotRevitAddin.UpdateSettingsCommandAvailability"
            };

            var synchronizeProjectButton = new PushButtonData("SynchronizeProjectButton", "Настройки синхронизации", AddInPath,
                "PilotRevitAddin.SynchronizeProject.SynchronizeCommand")
            {
                ToolTip = "Обновить настройки автоматической синхронизации",
                LargeImage = ImageUtils.NewBitmapImage(Assembly.GetExecutingAssembly(), "synchronizeIcon.png"),
                AvailabilityClassName = "PilotRevitAddin.SynchronizeProject.SynchronizeAvailability"
            };

            pilotRibbon.AddItem(prepareButton);
            pilotRibbon.AddItem(startButton);
            pilotRibbon.AddItem(updateProjectSettingsButton);
            pilotRibbon.AddItem(synchronizeProjectButton);
        }

        private void ControlledApplication_DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            var document = e.Document;
            if (!StartDesigning.IsProjectReady(document))
                return;

            document.SaveAs(StartDesigning.GetSafeFilePath(document));
            document.ReloadLatest(new ReloadLatestOptions());
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
