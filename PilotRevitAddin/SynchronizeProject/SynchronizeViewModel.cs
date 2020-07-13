using System;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using Ascon.Pilot.Theme.Tools;
using Autodesk.Revit.DB;
using Newtonsoft.Json;
using PilotRevitAddin.CommandsPrism;

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
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources\SynchronizeSettings.json");
            using (var sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }

        private void Save()
        {
            var jsonValue = JsonConvert.SerializeObject(Synchronize, Formatting.Indented);

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources\SynchronizeSettings.json");
            using (var sr = new StreamWriter(path))
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
                return _cancelCommand ?? (_cancelCommand = new CommandLight(obj => { base.CloseView(true);}));
            }
        }
    }
}