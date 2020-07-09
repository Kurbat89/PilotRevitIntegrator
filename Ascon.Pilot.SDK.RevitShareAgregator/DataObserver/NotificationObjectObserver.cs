using System;
using Ascon.Pilot.SDK;

namespace Ascon.Pilot.RevitShareAgregator.DataObserver
{
    public class NotificationObjectObserver : IObserver<INotification>
    {
        private readonly Action<INotification> _action;

        public NotificationObjectObserver(Action<INotification> action)
        {
            _action = action;
        }

        public void OnNext(INotification value)
        {
            _action(value);
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

    }
}
