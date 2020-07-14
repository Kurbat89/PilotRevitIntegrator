using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PilotRevitAddin.SynchronizeProject
{
    class SynchronizeSettingsWatcher
    {
        public delegate void SettingsChanged(SynchronizeModel settings);
        public event SettingsChanged SettingsChangedEvent;

        private SynchronizeModel _settings;
        public SynchronizeModel Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                OnSettingsChangedEvent(_settings);
            }
        }

        public SynchronizeSettingsWatcher()
        {
            Task.Factory.StartNew(StartListener);
            Settings = GetSettings();
        }

        private static SynchronizeModel GetSettings()
        {
            var jsonValue = Load();
            return JsonConvert.DeserializeObject<SynchronizeModel>(jsonValue);
        }

        private static string Load()
        {
            var fileInfo = new FileInfo(SettingsPath.SynchronizeSettingsPath);

            int counter = 0;
            while (IsFileLocked(fileInfo) || counter < 10)
            {
                counter++;
                Thread.Sleep(10);
            }

            using (var sr = new StreamReader(SettingsPath.SynchronizeSettingsPath))
            {
                return sr.ReadToEnd();
            }
        }

        private static bool IsFileLocked(FileInfo file) // this is complete shit
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        private void StartListener()
        {
            var path = Path.GetDirectoryName(SettingsPath.SynchronizeSettingsPath);

            FileSystemWatcher watcher = new FileSystemWatcher(path);
            watcher.Filter = "*.json";

            watcher.Changed += OnChanged;

            watcher.NotifyFilter = NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName
                                   | NotifyFilters.DirectoryName;

            watcher.EnableRaisingEvents = true;
            watcher.WaitForChanged(WatcherChangeTypes.All);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Settings = GetSettings();
        }

        private void OnSettingsChangedEvent(SynchronizeModel settings)
        {
            SettingsChangedEvent?.Invoke(settings);
        }
    }
}
