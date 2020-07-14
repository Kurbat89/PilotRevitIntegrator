using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PilotRevitAddin.SynchronizeProject
{
    class SynchronizeSettingsWatcher
    {
        public static SynchronizeModel Settings { get; set; }

        public SynchronizeSettingsWatcher()
        {
            Task.Factory.StartNew(StartListener);
            Settings = GetSettings();
        }

        public static SynchronizeModel GetSettings()
        {
            var jsonValue = Load();
            return JsonConvert.DeserializeObject<SynchronizeModel>(jsonValue);
        }

        private static string Load()
        {
            using (var sr = new StreamReader(SettingsPath.SynchronizeSettingsPath))
            {
                return sr.ReadToEnd();
            }
        }

        private static void StartListener()
        {
            var path = Path.GetDirectoryName(SettingsPath.SynchronizeSettingsPath);

            using (FileSystemWatcher watcher = new FileSystemWatcher(path))
            {
                watcher.Filter = "*.json";

                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;

                watcher.NotifyFilter = NotifyFilters.LastAccess
                                       | NotifyFilters.LastWrite
                                       | NotifyFilters.FileName
                                       | NotifyFilters.DirectoryName;

                watcher.EnableRaisingEvents = true;
                watcher.WaitForChanged(WatcherChangeTypes.All);

            }
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            Settings = GetSettings();
        }
    }
}
