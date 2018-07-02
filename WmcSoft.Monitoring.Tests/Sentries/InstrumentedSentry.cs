namespace WmcSoft.Monitoring.Sentries
{
    class InstrumentedSentry : SentryBase
    {
        public InstrumentedSentry(string name) : base(name)
        {
        }

        public bool Subscribed { get; private set; }
        public bool Unsubscribed { get; private set; }

        protected override void OnObserving()
        {
            base.OnObserving();
            Subscribed = true;
        }
        protected override void OnObserved()
        {
            base.OnObserved();
            Unsubscribed = true;
        }
    }

}
