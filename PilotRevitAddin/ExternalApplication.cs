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

        private readonly SynchronizeTimer _synchronizeTimer = new SynchronizeTimer();

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
            application.Idling += Application_Idling;

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

        private void Application_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {
            var uiApp = (UIApplication)sender;

            if (uiApp.ActiveUIDocument == null)
                return;

            var document = uiApp.ActiveUIDocument.Document;

            if (!document.IsWorkshared)
                return;

            if (!_synchronizeTimer.SynchronizeFlag) return;

            _synchronizeTimer.SynchronizeFlag = false;
            var settings = _synchronizeTimer.GetSettings();


            var centralOption = GetCentralOptions(settings);

            document.SynchronizeWithCentral(new TransactWithCentralOptions(), centralOption);
        }
        private SynchronizeWithCentralOptions GetCentralOptions(SynchronizeModel settings)
        {
            var relinquishOptions = new RelinquishOptions(false)
            {
                UserWorksets = settings.RelinquishModel.UserWorksets,
                ViewWorksets = settings.RelinquishModel.ViewWorksets,
                StandardWorksets = settings.RelinquishModel.StandardWorksets,
                CheckedOutElements = settings.RelinquishModel.CheckedOutElements,
                FamilyWorksets = settings.RelinquishModel.FamilyWorkset
            };

            var options = new SynchronizeWithCentralOptions()
            {
                SaveLocalBefore = settings.WithCentralModel.SaveLocalBefore,
                Compact = settings.WithCentralModel.Compact,
                Comment = settings.WithCentralModel.Comment,
                SaveLocalAfter = settings.WithCentralModel.SaveLocalAfter
            };

            options.SetRelinquishOptions(relinquishOptions);

            return options;
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
