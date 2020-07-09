using System;
using Ascon.Pilot.SDK;

namespace Ascon.Pilot.RevitShareAgregator.DataObserver
{
    public class SearchResultsObserver : IObserver<ISearchResult>
    {
        private readonly Action<ISearchResult> _action;

        public SearchResultsObserver(Action<ISearchResult> action)
        {
            _action = action;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(ISearchResult value)
        {
            _action(value);
        }
    }
}
