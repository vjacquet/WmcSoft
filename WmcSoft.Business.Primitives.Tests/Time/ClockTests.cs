using System;
using Xunit;

namespace WmcSoft.Time
{
    public class ClockTests
    {
        [Fact(Skip = "Uses network requests.")]
        public void CanQueryNistTime()
        {
            using (var client = new NistClient()) {
                var today = DateTime.UtcNow.Date;
                var dateTime = client.Query();

                Assert.Equal(DateTimeKind.Utc, dateTime.Kind);
                Assert.Equal(today, dateTime.Date);
            }
        }

        [Fact(Skip = "Uses network requests.")]
        public void CanQuerySntpTime()
        {
            using (var client = new SntpClient()) {
                var today = DateTime.UtcNow.Date;
                var dateTime = client.Query();

                Assert.Equal(DateTimeKind.Utc, dateTime.Kind);
                Assert.Equal(today, dateTime.Date);
            }
        }
    }
}
