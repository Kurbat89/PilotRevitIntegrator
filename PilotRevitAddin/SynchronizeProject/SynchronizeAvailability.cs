using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace PilotRevitAddin.SynchronizeProject
{
    class SynchronizeAvailability : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            return StartDesigning.IsProjectReady(applicationData.ActiveUIDocument.Document);
        }
    }
}
