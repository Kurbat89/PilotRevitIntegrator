using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

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