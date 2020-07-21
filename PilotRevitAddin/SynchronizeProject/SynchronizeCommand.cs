using Ascon.Pilot.Theme.Controls;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows;

namespace PilotRevitAddin.SynchronizeProject
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class SynchronizeCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            ShowDialogSynchronize(doc);
            return Result.Succeeded;
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
