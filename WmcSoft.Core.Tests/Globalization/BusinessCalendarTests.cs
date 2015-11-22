using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Globalization
{
    [TestClass]
    public class BusinessCalendarTests
    {
        [TestMethod]
        public void CanCreateBusinessCalendarWithoutWeekEnds() {
            var calendar = new BusinessCalendar(new DateTime(2015, 1, 1), TimeSpan.FromDays(100), BusinessCalendar.NoSaturdays, BusinessCalendar.NoSundays);
            Assert.IsTrue(calendar.IsBusinessDay(new DateTime(2015, 1, 2)));
            Assert.IsFalse(calendar.IsBusinessDay(new DateTime(2015, 1, 3)));
            Assert.IsFalse(calendar.IsBusinessDay(new DateTime(2015, 1, 4)));
            Assert.IsTrue(calendar.IsBusinessDay(new DateTime(2015, 1, 5)));
        }
    }
}
