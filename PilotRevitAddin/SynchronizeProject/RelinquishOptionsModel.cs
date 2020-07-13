using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotRevitAddin.SynchronizeProject
{
    class RelinquishOptionsModel
    {
        public bool CheckedOutElements { get; set; }
        public bool FamilyWorkset { get; set; }
        public bool StandardWorksets { get; set; }
        public bool UserWorksets { get; set; }
        public bool ViewWorksets { get; set; }
    }
}
