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
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ShowDialogSynchronize(commandData.Application.ActiveUIDocument.Document);

            return Result.Succeeded;
        }

        private void ShowDialogSynchronize(Document document)
        {
            var viewModel = new SynchronizeViewViewModel(document);
            var control = new SynchronizeView();
            var window = new PureWindow
            {
                Title = "Синхронизация с центральным файлом",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = control,
                DataContext = viewModel,
                Height = 700,
                Width = 800,
                CanClose = true,
                ResizeMode = ResizeMode.NoResize
            };
            window.ShowDialog();
        }
    }
}
