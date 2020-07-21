using Ascon.Pilot.Theme.Controls;
using Autodesk.Revit.DB;
using Newtonsoft.Json;
using PilotRevitAddin.CommandsPrism;
using PilotRevitAddin.SynchronizeProject.Journal;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace PilotRevitAddin.SynchronizeProject
{
    internal class SynchronizeViewModel : PropertyChangedBase
    {
        private readonly Document _document;
        public SynchronizeModel Synchronize { get; set; }

        public SynchronizeViewModel(Document document)
        {
            _document = document;
            Synchronize = InitializationModel();
            Synchronize.CentralModelPath = ModelPathUtils.ConvertModelPathToUserVisiblePath(_document.GetWorksharingCentralModelPath());
        }

        private SynchronizeModel InitializationModel()
        {
            var jsonValue = Load();
            return JsonConvert.DeserializeObject<SynchronizeModel>(jsonValue);
        }

        private string Load()
        {
            using (var sr = new StreamReader(SettingsPath.SynchronizeSettingsPath))
            {
                return sr.ReadToEnd();
            }
        }

        private void Save()
        {
            var jsonValue = JsonConvert.SerializeObject(Synchronize, Formatting.Indented);

            using (var sr = new StreamWriter(SettingsPath.SynchronizeSettingsPath))
            {
                sr.Write(jsonValue);
            }

        }

        private ICommand _okCommand;
        public ICommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new CommandLight(obj => { Save(); CloseView(true); }));
            }
        }

        private ICommand _cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new CommandLight(obj => { base.CloseView(true); }));
            }
        }

        private ICommand _showJournalCommand;

        public ICommand ShowJournalCommand
        {
            get
            {
                return _showJournalCommand ?? (_showJournalCommand = new CommandLight(obj =>
                {
                    ShowJournal();
                }));
            }
        }

        private void ShowJournal()
        {
            var viewModel = new JournalViewModel(_document);
            var control = new JournalView();
            var window = new PureWindow
            {
                Title = "Журнал синхронизаций",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = control,
                DataContext = viewModel,
                Height = 450,
                Width = 600,
                CanClose = true,
                ResizeMode = ResizeMode.NoResize
            };
            window.Show();
        }
    }
}