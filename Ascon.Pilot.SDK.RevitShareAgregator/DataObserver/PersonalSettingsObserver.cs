using System;
using System.Collections.Generic;

namespace Ascon.Pilot.SDK.RevitShareAgregator.DataObserver
{

    public class PersonalSettingsObserver : IObserver<KeyValuePair<string, string>>
    {
        private readonly Action<KeyValuePair<string, string>> _action;

        public PersonalSettingsObserver(Action<KeyValuePair<string, string>> action)
        {
            _action = action;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, string> value)
        {
            _action(value);
        }
    }
}
