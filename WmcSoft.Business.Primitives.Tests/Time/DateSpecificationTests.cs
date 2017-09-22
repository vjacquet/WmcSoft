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
            var february = DateSpecification.NthOccurenceOfWeekdayInMonth(2, DayOfWeek.Tuesday, 2);
            var february2016 = february.OfYear(2016);
            Assert.Equal(new Date(2016, 2, 9), february2016);
            Assert.True(february.IsSatisfiedBy(february2016));

            var april = DateSpecification.NthOccurenceOfWeekdayInMonth(4, DayOfWeek.Tuesday, 2);
            var april2016 = april.OfYear(2016);
            Assert.Equal(new Date(2016, 4, 12), april2016);
            Assert.True(april.IsSatisfiedBy(april2016));
        }

        [Theory]
        [InlineData(2014, 5, 26)]
        [InlineData(2015, 5, 25)]
        [InlineData(2016, 5, 30)]
        [InlineData(2017, 5, 29)]
        [InlineData(2018, 5, 28)]
        [InlineData(2019, 5, 27)]
        [InlineData(2020, 5, 25)]
        [InlineData(2021, 5, 31)]
        public void CanSpecifyMemorialDay(int year, int month, int day)
        {
            var memorialDay = DateSpecification.NthOccurenceOfWeekdayInMonth(5, DayOfWeek.Monday, -1);
            var expected = new Date(year, month, day);
            Assert.Equal(expected, memorialDay.OfYear(year));
        }
}
}
