using System;

namespace WmcSoft.Monitoring.Sentries
{
    class NullObserver : IObserver<SentryStatus>
    {
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(SentryStatus value)
        {
        }
    }
}
