namespace WmcSoft.Monitoring.Sentries
{
    class StubSentry : SentryBase
    {
        public StubSentry(string name, SentryStatus status) : base(name)
        {
            OnNext(status);
        }

        public void Reset(SentryStatus value)
        {
            OnNext(value);
        }
    }
}
