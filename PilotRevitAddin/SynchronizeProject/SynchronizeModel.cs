using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PilotRevitAddin.CommandsPrism;

namespace PilotRevitAddin.SynchronizeProject
{
    class SynchronizeModel : PropertyChangedBase
    {
        private bool _syncOn;
        public bool SyncOn
        {
            get => _syncOn;
            set => SetField(ref _syncOn, value);
        }

        public int SelectTimeIntervals { get; set; } = 5;

        public string CentralModelPath { get; set; }
        
        [JsonIgnore]
        public List<int> TimeIntervals { get; set; } = new List<int>() { 5,10,15,20,30,60,120,180,240,300 };

        public SynchronizeModel()
        {
            WithCentralModel = new WithCentralOptionsModel();
            RelinquishModel = new RelinquishOptionsModel();
        }

        public WithCentralOptionsModel WithCentralModel { get; set; }

        public RelinquishOptionsModel RelinquishModel { get; set; }

    }
}
