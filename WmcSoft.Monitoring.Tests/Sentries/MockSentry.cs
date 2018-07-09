using System;

namespace WmcSoft.Monitoring.Sentries
{
    class MockSentry : SentryBase
    {
        public MockSentry(string name, SentryStatus status = SentryStatus.None)
            : base(name)
        {
            OnNext(status);
        }

        public void SetStatus(SentryStatus status)
        {
            OnNext(status);
        }

        protected override void OnObserving()
        {
            base.OnObserving();
            OnObservingCalls++;
        }

        protected override void OnObserved()
        {
            base.OnObserved();
            OnObservedCalls++;
        }

        protected override void OnSubscribe(IObserver<SentryStatus> observer)
        {
            base.OnSubscribe(observer);
            OnSubscribeCalls++;
        }

        protected override void OnUnsubscribe(IObserver<SentryStatus> observer)
        {
            base.OnUnsubscribe(observer);
            OnUnsubscribeCalls++;
        }

        public int OnObservingCalls { get; set; }
        public int OnObservedCalls { get; set; }
        public int OnSubscribeCalls { get; set; }
        public int OnUnsubscribeCalls { get; set; }

    }
}
