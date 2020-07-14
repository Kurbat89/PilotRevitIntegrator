using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ascon.Pilot.Theme.Controls;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace PilotRevitAddin.SynchronizeProject
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class SynchronizeCommand : IExternalCommand
    {
        private readonly SynchronizeTimer _synchronizeTimer = new SynchronizeTimer();

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            commandData.Application.Idling += Application_Idling;

            ShowDialogSynchronize(doc);

            return Result.Succeeded;
        }

        private void Application_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {
            if (_synchronizeTimer.SynchronizeFlag)
            {
                _synchronizeTimer.SynchronizeFlag = false;
                var uiApp = (UIApplication)sender;
                var settings = _synchronizeTimer.GetSettings();

                var document = uiApp.ActiveUIDocument.Document;

                var centralOption = GetCentralOptions(settings);
                
                document.SynchronizeWithCentral(new TransactWithCentralOptions(), centralOption);
            }
        }

        private SynchronizeWithCentralOptions GetCentralOptions(SynchronizeModel settings)
        {
            var relinquishOptions = new RelinquishOptions(false);
            relinquishOptions.UserWorksets = settings.RelinquishModel.UserWorksets;
            relinquishOptions.ViewWorksets = settings.RelinquishModel.ViewWorksets;
            relinquishOptions.StandardWorksets = settings.RelinquishModel.StandardWorksets;
            relinquishOptions.CheckedOutElements = settings.RelinquishModel.CheckedOutElements;
            relinquishOptions.FamilyWorksets = settings.RelinquishModel.FamilyWorkset;

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

        private void ShowDialogSynchronize(Document document)
        {
            var viewModel = new SynchronizeViewModel(document);
            var control = new SynchronizeView();
            var window = new PureWindow
            {
                Title = "Синхронизация с центральным файлом",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = control,
                DataContext = viewModel,
                Height = 450,
                Width = 600,
                CanClose = true,
                ResizeMode = ResizeMode.NoResize
            };
            window.ShowDialog();
        }
    }
}
