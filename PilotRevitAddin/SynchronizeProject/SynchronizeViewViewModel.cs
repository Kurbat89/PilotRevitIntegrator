using Autodesk.Revit.DB;

namespace PilotRevitAddin.SynchronizeProject
{
    internal class SynchronizeViewViewModel
    {
        private readonly Document _document;

        public SynchronizeViewViewModel(Document document)
        {
            _document = document;
        }
    }
}