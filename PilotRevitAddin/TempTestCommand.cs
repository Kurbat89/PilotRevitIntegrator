using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace PilotRevitAddin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class TempTestCommand : IExternalCommand
    {
        public void GetElementWorksharingInfo(Document doc, Element elem)
        {
            String message = String.Empty;
            message += "Element Id: " + elem.Id;

            // The workset the element belongs to
            WorksetId worksetId = elem.WorksetId;
            message += ("\nWorkset Id : " + worksetId.ToString());

            // Model Updates Status of the element
            ModelUpdatesStatus updateStatus = WorksharingUtils.GetModelUpdatesStatus(doc, elem.Id);
            message += ("\nUpdate status : " + updateStatus.ToString());

            // Checkout Status of the element
            CheckoutStatus checkoutStatus = WorksharingUtils.GetCheckoutStatus(doc, elem.Id);
            message += ("\nCheckout status : " + checkoutStatus.ToString());

            // Getting WorksharingTooltipInfo of a given element Id
            WorksharingTooltipInfo tooltipInfo = WorksharingUtils.GetWorksharingTooltipInfo(doc, elem.Id);
            message += ("\nCreator : " + tooltipInfo.Creator);
            message += ("\nCurrent Owner : " + tooltipInfo.Owner);
            message += ("\nLast Changed by : " + tooltipInfo.LastChangedBy);

            Autodesk.Revit.UI.TaskDialog.Show("GetElementWorksharingInfo", message);
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var application = commandData.Application;
            var document = application.ActiveUIDocument.Document;
            var elem = document.GetElement(new ElementId(947108));
             GetElementWorksharingInfo(document, elem);

            //947057

            //var worksetTable = document.GetWorksetTable();
            //var path = document.GetWorksharingCentralModelPath();
            //var worksetInfo = WorksharingUtils.GetUserWorksetInfo(path);

            //FilteredElementCollector elementCollector = new FilteredElementCollector(document);

            //foreach (var workset in worksetInfo)
            //{
            //    ElementWorksetFilter elementWorksetFilter = new ElementWorksetFilter(workset.Id);
            //    ICollection<Element> worksetElemsfounds = elementCollector.WherePasses(elementWorksetFilter).ToElements();

            //    if (!worksetElemsfounds.Any())
            //        continue;


            //    // var tt = WorksharingUtils.GetWorksharingTooltipInfo(document, worksetElemsfounds.Select(z=>z.Id));
            //    int counter = 0;
            //    string statusInfo = "";
            //    foreach (var worksetElemsfound in worksetElemsfounds)
            //    {
            //       var toolTip = WorksharingUtils.GetWorksharingTooltipInfo(document, worksetElemsfound.Id);
            //       var status = WorksharingUtils.GetCheckoutStatus(document, worksetElemsfound.Id);

            //       if (toolTip.LastChangedBy == application.Application.Username)
            //       {
            //           statusInfo += status + Environment.NewLine;
            //           counter++;
            //       }

            //    }
            //    TaskDialog.Show("Еуые", $"{counter.ToString()}, {statusInfo}");
            // }

            var relinquishOptions = new RelinquishOptions(true);
            relinquishOptions.CheckedOutElements = false;

            var options = new SynchronizeWithCentralOptions()
            {
                Comment = "Test",
                Compact = false,
                SaveLocalAfter = true,
                SaveLocalBefore = true,
            };

            options.SetRelinquishOptions(relinquishOptions);
            
            var transaction = new TransactWithCentralOptions();
            
            document.SynchronizeWithCentral(transaction, options);

            //document.SynchronizeWithCentral(new TransactWithCentralOptions(), new SynchronizeWithCentralOptions()
            //{
            //    Comment = "Test",
            //    Compact = false,
            //    SaveLocalAfter = true,
            //    SaveLocalBefore = true,
            //});


            return Result.Succeeded;
        }
    }
}
