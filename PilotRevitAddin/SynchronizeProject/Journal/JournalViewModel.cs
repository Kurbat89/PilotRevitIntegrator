using Autodesk.Revit.DB;
using PilotRevitAddin.CommandsPrism;
using System.Collections.ObjectModel;

namespace PilotRevitAddin.SynchronizeProject.Journal
{
    class JournalViewModel : PropertyChangedBase
    {
        private readonly Document _document;

        public ObservableCollection<JournalModel> JournalModels { get; set; }


        public JournalViewModel(Document document)
        {
            _document = document;
            JournalModels = GetSynchronizeInfo();
        }

        private ObservableCollection<JournalModel> GetSynchronizeInfo()
        {

            return null;
        }
    }
}
