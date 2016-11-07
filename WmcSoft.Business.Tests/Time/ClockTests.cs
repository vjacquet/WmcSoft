using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Time
{
    [TestClass]
    public class ClockTests
    {
        [TestMethod]
        [Ignore]
        public void CanQueryNistTime() {
            using (var client = new NistClient()) {
                var today = DateTime.UtcNow.Date;
                var dateTime = client.Query();

                Assert.AreEqual(DateTimeKind.Utc, dateTime.Kind);
                Assert.AreEqual(today, dateTime.Date);
            }
        }

        [TestMethod]
        [Ignore]
        public void CanQuerySntpTime() {
            using (var client = new SntpClient()) {
                var today = DateTime.UtcNow.Date;
                var dateTime = client.Query();

                Assert.AreEqual(DateTimeKind.Utc, dateTime.Kind);
                Assert.AreEqual(today, dateTime.Date);
            }
        }
    }
}
