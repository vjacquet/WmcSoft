using System;
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
    }
}
