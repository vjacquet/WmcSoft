using Xunit;

namespace WmcSoft.Monitoring.Sentries
{
    public class SentryTests
    {
        [Fact]
        public void ObserveCurrentStatusWhenSubscribing()
        {
            var subject = new SubjectSentry("subject", SentryStatus.Success);
            var mock = new MockObserver();
            using (subject.Subscribe(mock)) {
                Assert.Equal(1, mock.ObservedValues);
                Assert.Equal(mock.LatestValue, subject.Status);
            }
        }
    }
}
