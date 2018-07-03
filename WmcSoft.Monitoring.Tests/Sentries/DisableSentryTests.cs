using Xunit;

namespace WmcSoft.Monitoring.Sentries
{
    public class DisableSentryTests
    {
        [Fact]
        public void DisabledSentriesReturnsNoneStatus()
        {
            var subject = new SubjectSentry("subject");
            var sentry = new DisableSentry(subject, enabled: false);

            subject.Status = SentryStatus.Success;
            Assert.Equal(SentryStatus.None, sentry.Status);

            sentry.Enabled = true;
            Assert.Equal(subject.Status, sentry.Status);

            subject.Status = SentryStatus.Warning;
            Assert.Equal(subject.Status, sentry.Status);

            subject.Status = SentryStatus.Error;
            Assert.Equal(subject.Status, sentry.Status);

            sentry.Enabled = false;
            Assert.Equal(SentryStatus.None, sentry.Status);
        }

        [Fact]
        public void DisablingSentriesStopsObservation()
        {
            var subject = new SubjectSentry("subject", SentryStatus.Success);
            var sentry = new DisableSentry(subject, enabled: true);
            var mock = new MockObserver();
            using (sentry.Subscribe(mock)) {
                Assert.Equal(subject.Status, sentry.Status);
                Assert.Equal(1, mock.ObservedValues);
                Assert.Equal(mock.LatestValue, sentry.Status);

                subject.Status = SentryStatus.Warning;
                Assert.Equal(2, mock.ObservedValues);
                Assert.Equal(subject.Status, sentry.Status);

                sentry.Enabled = false;
                Assert.Equal(SentryStatus.None, sentry.Status);
                Assert.Equal(3, mock.ObservedValues);
                Assert.Equal(mock.LatestValue, sentry.Status);

                subject.Status = SentryStatus.Warning;
                Assert.Equal(SentryStatus.None, sentry.Status);
                Assert.Equal(3, mock.ObservedValues);
                Assert.Equal(mock.LatestValue, sentry.Status);

                sentry.Enabled = true;
                Assert.Equal(subject.Status, sentry.Status);
                Assert.Equal(4, mock.ObservedValues);
                Assert.Equal(mock.LatestValue, sentry.Status);
            }
        }

        [Fact]
        public void ObserveNoneWhenSubscribingToDisabledSentry()
        {
            var subject = new SubjectSentry("subject", SentryStatus.Success);
            var sentry = new DisableSentry(subject, enabled: false);
            var mock = new MockObserver();
            using (sentry.Subscribe(mock)) {
                Assert.NotEqual(subject.Status, sentry.Status);
                Assert.Equal(1, mock.ObservedValues);
                Assert.Equal(SentryStatus.None, mock.LatestValue);
            }
        }
    }
}
