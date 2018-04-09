using System;
using Xunit;

namespace WmcSoft.Time
{
    public class ClockTests
    {
        [Fact(Skip = "Uses network requests.")]
        public void CanQueryNistTime()
        {
            #region QueryNistTime

            using (var client = new NistClient()) {
                var today = DateTime.UtcNow.Date;
                var dateTime = client.Query();

                Assert.Equal(DateTimeKind.Utc, dateTime.Kind);
                Assert.Equal(today, dateTime.Date);
            }

            #endregion
        }

        [Fact(Skip = "Uses network requests.")]
        public void CanQuerySntpTime()
        {
            #region QuerySntpTime

            using (var client = new SntpClient()) {
                var today = DateTime.UtcNow.Date;
                var dateTime = client.Query();

                Assert.Equal(DateTimeKind.Utc, dateTime.Kind);
                Assert.Equal(today, dateTime.Date);
            }

            #endregion
        }
    }
}
