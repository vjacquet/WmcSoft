using System;
using System.Collections.Generic;

namespace WmcSoft.Monitoring.Sentries
{
    class GarbageCollectorObserver : IObserver<SentryStatus>
    {
        private readonly List<IDisposable> disposables;

        public GarbageCollectorObserver(params IDisposable[] disposables)
        {
            this.disposables = new List<IDisposable>(disposables);
        }

        public void OnNext(SentryStatus value)
        {
        }

        public void OnCompleted()
        {
            Cleanup();
        }

        public void OnError(Exception error)
        {
            Cleanup();
        }

        private void Cleanup()
        {
            for (int i = disposables.Count - 1; i >= 0; i--) {
                disposables[i].Dispose();
            }
            disposables.Clear();
        }
    }
}
