using System;

namespace WmcSoft.Monitoring.Sentries
{
    class MockObserver : IObserver<SentryStatus>
    {
        public void OnCompleted()
        {
            IsCompleted = true;
        }

        public void OnError(Exception error)
        {
            Error = error;
        }

        public void OnNext(SentryStatus value)
        {
            LatestValue = value;
            ObservedValues++;
        }

        public bool IsCompleted { get; set; }
        public Exception Error { get; set; }
        public SentryStatus LatestValue { get; set; }
        public int ObservedValues { get; set; }
    }
}
