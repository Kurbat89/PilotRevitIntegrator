using System.Threading;

namespace PilotRevitAddin.SynchronizeProject
{
    class SynchronizeTimer
    {
        private static readonly SynchronizeSettingsWatcher _synchronizeWatcher = new SynchronizeSettingsWatcher();
        private Timer _timer;

        public bool SynchronizeFlag;

        public SynchronizeTimer()
        {
            _synchronizeWatcher.SettingsChangedEvent += SynchronizeWatcher_SettingsChangedEvent;
            StartTimer();
        }

        public SynchronizeModel GetSettings()
        {
            return _synchronizeWatcher.Settings;
        }

        private void StartTimer()
        {
            var settings = _synchronizeWatcher.Settings;

            if (settings.SyncOn)
            {
                if (settings.SelectTimeIntervals == 0 || settings.SelectTimeIntervals < 0) return;

                var timeCB = new TimerCallback(TikTakTimeToGetUp);

                _timer?.Dispose();

                _timer = new Timer(timeCB, null, settings.SelectTimeIntervals * 60000, settings.SelectTimeIntervals * 60000);
            }
            else
            {
                _timer?.Dispose();
            }
        }

        private void TikTakTimeToGetUp(object obj)
        {
            SynchronizeFlag = true;
        }

        private void SynchronizeWatcher_SettingsChangedEvent(SynchronizeModel settings)
        {
            StartTimer();
        }
    }
}
