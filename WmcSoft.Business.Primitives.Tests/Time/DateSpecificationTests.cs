using System;
using Xunit;

namespace WmcSoft.Time
{
    public class DateSpecificationTests
    {
        [Fact]
        public void CanSpecifyChristmas()
        {
            var christmas = DateSpecification.Fixed(12, 25);

            var actual = christmas.OfYear(2016);
            var expected = new Date(2016, 12, 25);
            Assert.Equal(expected, actual);

            Assert.True(christmas.IsSatisfiedBy(actual));

            var newYearEve = new Date(2016, 12, 31);
            Assert.False(christmas.IsSatisfiedBy(newYearEve));
        }

        [Fact]
        public void CanSpecifySecondTuesday()
        {
            var february = DateSpecification.NthOccuranceOfWeekdayInMonth(2, DayOfWeek.Tuesday, 2);
            var february2016 = february.OfYear(2016);
            Assert.Equal(new Date(2016, 2, 9), february2016);
            Assert.True(february.IsSatisfiedBy(february2016));

            var april = DateSpecification.NthOccuranceOfWeekdayInMonth(4, DayOfWeek.Tuesday, 2);
            var april2016 = april.OfYear(2016);
            Assert.Equal(new Date(2016, 4, 12), april2016);
            Assert.True(april.IsSatisfiedBy(april2016));
        }
    }
}
