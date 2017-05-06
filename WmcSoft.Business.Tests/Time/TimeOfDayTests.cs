using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Time
{
    [TestClass]
    public class TimeOfDayTests
    {
        [TestMethod]
        public void CanCreateTimeOfDay()
        {
            var time = new TimeOfDay(8, 30);

            Assert.AreEqual(8, time.Hour);
            Assert.AreEqual(30, time.Minutes);
        }

        [TestMethod]
        public void CanDeconstructTimeOfDay()
        {
            var time = new TimeOfDay(8, 30);
            var (h, m) = time;

            Assert.AreEqual(8, h);
            Assert.AreEqual(30, m);
        }

        [TestMethod]
        public void CanCompareTimes()
        {
            var h0830 = new TimeOfDay(8, 30);
            var h1120 = new TimeOfDay(11, 20);

            Assert.IsFalse(h0830.IsAfter(h1120));
            Assert.IsTrue(h0830.IsBefore(h1120));
        }

        [TestMethod]
        public void CanConvertToDateTime()
        {
            var date = new Date(2016, 10, 1);
            var time = new TimeOfDay(8, 30);
            var zone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            var dateTimeOffset = time.On(date, zone);
            Assert.AreEqual(2016, dateTimeOffset.Year);
            Assert.AreEqual(10, dateTimeOffset.Month);
            Assert.AreEqual(1, dateTimeOffset.Day);
            Assert.AreEqual(8, dateTimeOffset.Hour);
            Assert.AreEqual(30, dateTimeOffset.Minute);
        }
    }
}
