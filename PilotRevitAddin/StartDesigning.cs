using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotRevitAddin
{
    public static class StartDesigning
    {
        public static bool IsProjectReady(UIApplication a)
        {
            if (a.ActiveUIDocument == null)
                return false;
            var doc = a.ActiveUIDocument.Document;
            if (!doc.IsWorkshared)
                return false;

            return IsProjectReady(doc);
        }

        public static bool IsProjectReady(Document document)
        {
            if (!document.IsWorkshared)
                return false;
            var centralModelPath = document.GetWorksharingCentralModelPath();
            var centralFilePath = ModelPathUtils.ConvertModelPathToUserVisiblePath(centralModelPath);
            var myDocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            return !string.IsNullOrEmpty(centralFilePath) && !document.PathName.Contains(myDocsPath);
        }

        public static string GetSafeFilePath(Document document)
        {
            var myDocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filePath = Path.Combine(myDocsPath, document.Title);

            if (!File.Exists(filePath))
                return filePath;
            var validFileName = Path.GetFileNameWithoutExtension(filePath);
            var directory = Path.GetDirectoryName(filePath);
            var extension = Path.GetExtension(filePath);
            for (var i = 1; ; i++)
            {
                var nextFileName = $"{validFileName} ({i})";
                if (directory == null)
                    continue;
                var nextFilePath = Path.Combine(directory, nextFileName + extension);
                if (!File.Exists(nextFilePath))
                    return nextFilePath;
            }
        }

    }
}
