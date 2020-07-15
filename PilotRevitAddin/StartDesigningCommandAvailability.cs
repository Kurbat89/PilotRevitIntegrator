using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace PilotRevitAddin
{
    public class StartDesigningCommandAvailability : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication a, CategorySet b)
        {
            return StartDesigning.IsProjectReady(a);
        }
    }
}