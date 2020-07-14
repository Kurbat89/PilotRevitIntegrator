using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace PilotRevitAddin.CommandsPrism
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool? _closeResult;

        [JsonIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool? CloseResult
        {
            get { return _closeResult; }
            private set
            {
                _closeResult = value;
                NotifyPropertyChanged();
            }
        }

        protected virtual void NotifyAllPropertiesChanged()
        {
            NotifyPropertyChanged(null);
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CloseView(bool dialogResult)
        {
            CloseResult = dialogResult;
        }
        public bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {

            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

    }
}
