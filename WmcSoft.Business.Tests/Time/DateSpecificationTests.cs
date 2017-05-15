using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Time
{
    [TestClass]
    public class DateSpecificationTests
    {
        [TestMethod]
        public void CanSpecifyChristmas()
        {
            var christmas = DateSpecification.Fixed(12, 25);

            var actual = christmas.OfYear(2016);
            var expected = new DateTime(2016, 12, 25);
            Assert.AreEqual(expected, actual);

            Assert.IsTrue(christmas.IsSatisfiedBy(actual));

            var newYearEve = new Date(2016, 12, 31);
            Assert.IsFalse(christmas.IsSatisfiedBy(newYearEve));
        }

        [TestMethod]
        public void CanSpecifySecondTuesday()
        {
            var february = DateSpecification.NthOccuranceOfWeekdayInMonth(2, DayOfWeek.Tuesday, 2);
            var february2016 = february.OfYear(2016);
            Assert.AreEqual(new DateTime(2016, 2, 9), february2016);
            Assert.IsTrue(february.IsSatisfiedBy(february2016));

            var april = DateSpecification.NthOccuranceOfWeekdayInMonth(4, DayOfWeek.Tuesday, 2);
            var april2016 = april.OfYear(2016);
            Assert.AreEqual(new DateTime(2016, 4, 12), april2016);
            Assert.IsTrue(april.IsSatisfiedBy(april2016));
        }
    }
}
