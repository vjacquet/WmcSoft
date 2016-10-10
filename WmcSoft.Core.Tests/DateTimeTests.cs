using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class DateTimeTests
    {
        [TestMethod]
        public void CheckWeekOfMonth() {
            DateTime actual;

            actual = new DateTime(2009, 9, 30);
            Assert.AreEqual(5, actual.WeekOfMonth());

            actual = new DateTime(2009, 10, 1);
            Assert.AreEqual(1, actual.WeekOfMonth());

            actual = new DateTime(2009, 10, 3);
            Assert.AreEqual(1, actual.WeekOfMonth());

            actual = new DateTime(2009, 10, 4);
            Assert.AreEqual(2, actual.WeekOfMonth());

            actual = new DateTime(2009, 10, 5);
            Assert.AreEqual(2, actual.WeekOfMonth());

            actual = new DateTime(2009, 10, 12);
            Assert.AreEqual(3, actual.WeekOfMonth());

            actual = new DateTime(2009, 10, 25);
            Assert.AreEqual(5, actual.WeekOfMonth());

            actual = new DateTime(2009, 10, 31);
            Assert.AreEqual(5, actual.WeekOfMonth());

            actual = new DateTime(2009, 11, 1);
            Assert.AreEqual(1, actual.WeekOfMonth());
        }

        [TestMethod]
        public void CanFindFirstDayOfWeek() {
            var date = new DateTime(2016, 08, 10);
            var info = new DateTimeFormatInfo {
                FirstDayOfWeek = DayOfWeek.Monday,
            };
            var expected = new DateTime(2016, 08, 08);
            var actual = date.FirstDayOfWeek(info);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanFindLastDayOfWeek() {
            var date = new DateTime(2016, 08, 10);
            var info = new DateTimeFormatInfo {
                FirstDayOfWeek = DayOfWeek.Monday,
            };
            var expected = new DateTime(2016, 08, 14);
            var actual = date.LastDayOfWeek(info);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanFindFirstDayOfMonth() {
            var date = new DateTime(2016, 08, 10);
            var expected = new DateTime(2016, 08, 01);
            var actual = date.FirstDayOfMonth();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanFindLastDayOfMonth() {
            var date = new DateTime(2016, 02, 10);
            var expected = new DateTime(2016, 02, 29);
            var actual = date.LastDayOfMonth();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AssertUnspecifiedKindOnNewDateTime() {
            var unspecified = new DateTime(2016, 01, 01, 12, 0, 0);
            Assert.AreEqual(DateTimeKind.Unspecified, unspecified.Kind);
        }

        [TestMethod]
        public void CheckAsLocalTime() {
            var unspecified = new DateTime(2016, 01, 01, 12, 0, 0);
            var local = new DateTime(2016, 01, 01, 12, 0, 0, DateTimeKind.Local);
            var utc = new DateTime(2016, 01, 01, 12, 0, 0, DateTimeKind.Utc);

            Assert.AreEqual(local.ToLocalTime(), local.AsLocalTime());
            Assert.AreEqual(utc.ToLocalTime(), utc.AsLocalTime());
            Assert.AreNotEqual(unspecified.ToLocalTime(), unspecified.AsLocalTime());
        }

        [TestMethod]
        public void CheckAsUniversalTime() {
            var unspecified = new DateTime(2016, 01, 01, 12, 0, 0);
            var local = new DateTime(2016, 01, 01, 12, 0, 0, DateTimeKind.Local);
            var utc = new DateTime(2016, 01, 01, 12, 0, 0, DateTimeKind.Utc);

            Assert.AreEqual(local.ToUniversalTime(), local.AsUniversalTime());
            Assert.AreEqual(utc.ToUniversalTime(), utc.AsUniversalTime());
            Assert.AreNotEqual(unspecified.ToUniversalTime(), unspecified.AsUniversalTime());
        }
    }
}
