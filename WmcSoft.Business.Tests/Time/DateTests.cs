using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Time
{
    [TestClass]
    public class DateTests
    {
        [TestMethod]
        public void AssertDateIsOfUnspecifiedKind() {
            var manual = new Date(1973, 5, 2);
            Assert.AreEqual(DateTimeKind.Unspecified, ((DateTime)manual).Kind);

            var dateTime = new DateTime(1973, 5, 2, 15, 30, 25, DateTimeKind.Local);
            var fromDateTime = (Date)dateTime;
            Assert.AreEqual(DateTimeKind.Unspecified, ((DateTime)fromDateTime).Kind);
        }

        [TestMethod]
        public void CheckAsTimePoint() {
            var manual = new Date(1973, 5, 2);
            var zone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            var tp = manual.AsTimePoint(zone);
            var dateTimeOffset = (DateTimeOffset)tp;
            var actual = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.DateTime, zone);
            Assert.AreEqual(1973, actual.Year);
            Assert.AreEqual(5, actual.Month);
            Assert.AreEqual(2, actual.Day);
        }
    }
}
