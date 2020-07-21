using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace PilotRevitAddin.SynchronizeProject
{
    class SynchronizeControl
    {
        private readonly UIControlledApplication _application;
        private readonly SynchronizeTimer _synchronizeTimer = new SynchronizeTimer();
        private List<Document> _documentsPool;

        public SynchronizeControl(UIControlledApplication application)
        {
            _application = application;
            _application.ControlledApplication.DocumentOpened += ControlledApplication_DocumentOpened;
            _application.ControlledApplication.DocumentClosed += ControlledApplication_DocumentClosed;
            _application.Idling += Application_Idling;
            _documentsPool = new List<Document>();
        }

        private void Application_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {
            if (!_documentsPool.Any())
                return;
            
            e.SetRaiseWithoutDelay();

            if (!_synchronizeTimer.SynchronizeFlag) return;

            _synchronizeTimer.SynchronizeFlag = false;

            var settings = _synchronizeTimer.GetSettings();
            var centralOption = GetCentralOptions(settings);

            foreach (var document in _documentsPool)
            {
                if (!document.IsValidObject || !document.IsWorkshared)
                    continue;
                document.SynchronizeWithCentral(new TransactWithCentralOptions(), centralOption);
            }
        }

        private void ControlledApplication_DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            var doc = e.Document;
            var fileName =  Path.GetFileName(doc.PathName);

            if (!doc.IsWorkshared) return;

            if (_documentsPool.Where(z=>z.IsValidObject).All(z => Path.GetFileName(z.PathName) != fileName))
                _documentsPool.Add(doc);
        }

        private void ControlledApplication_DocumentClosed(object sender, Autodesk.Revit.DB.Events.DocumentClosedEventArgs e)
        {
            var application = (Application)sender;
            var newPool = new List<Document>();

            foreach (Document doc in application.Documents)
            {
                foreach (var documentPool in _documentsPool)
                {
                    if (!documentPool.IsValidObject)
                        continue;
                    
                    var fileName = Path.GetFileName(doc.PathName);
                    var name = Path.GetFileName(documentPool.PathName);
                    if (fileName == name)
                        newPool.Add(doc);
                }
            }
            _documentsPool = newPool;
        }

        private SynchronizeWithCentralOptions GetCentralOptions(SynchronizeModel settings)
        {
            var relinquishOptions = new RelinquishOptions(false)
            {
                UserWorksets = settings.RelinquishModel.UserWorksets,
                ViewWorksets = settings.RelinquishModel.ViewWorksets,
                StandardWorksets = settings.RelinquishModel.StandardWorksets,
                CheckedOutElements = settings.RelinquishModel.CheckedOutElements,
                FamilyWorksets = settings.RelinquishModel.FamilyWorkset
            };

            var options = new SynchronizeWithCentralOptions()
            {
                SaveLocalBefore = settings.WithCentralModel.SaveLocalBefore,
                Compact = settings.WithCentralModel.Compact,
                Comment = settings.WithCentralModel.Comment,
                SaveLocalAfter = settings.WithCentralModel.SaveLocalAfter
            };

            options.SetRelinquishOptions(relinquishOptions);
            return options;
        }
    }
}
