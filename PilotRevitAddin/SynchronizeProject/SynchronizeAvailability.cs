using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace PilotRevitAddin.SynchronizeProject
{
    class SynchronizeAvailability : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            return applicationData.ActiveUIDocument.Document.IsWorkshared;
        }
    }
}
