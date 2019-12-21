using System;
using System.Linq;
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

        [Fact]
        public void CanEnumerateDates()
        {
            var newYear = new Date(2000, 1, 1);
            var newYearEve = new Date(2000, 12, 31);
            var labourDay = new Date(2000, 5, 1);
            var xmas = new Date(2000, 12, 25);

            var specification = new DatesSpecification(new[] { newYear, labourDay, xmas, newYearEve });
            Assert.Equal(4, specification.EnumerateBetween(newYear.AddYears(-1), newYear.AddYears(+1)).Count());
            Assert.Equal(4, specification.EnumerateBetween(newYear, newYearEve).Count());
            Assert.Equal(2, specification.EnumerateBetween(labourDay, xmas).Count());
            Assert.Equal(2, specification.EnumerateBetween(new Date(2000, 2, 1), new Date(2000, 12, 30)).Count());
        }

        [Fact]
        public void CanEnumerateDatesBetweenBounds()
        {
            var months = Enumerable.Range(1, 12).Select(m => new Date(2000, m, 01)).ToArray();
            var dates = new DatesSpecification(months);
            Assert.Equal(months, dates.EnumerateBetween(new Date(2000, 01, 01), new Date(2000, 12, 31)));
        }

        [Theory]
        [InlineData(06, 15)]
        [InlineData(07, 01)]
        public void CanApplyASpecificationSinceAGivenDate(int month, int day)
        {
            var months = Enumerable.Range(1, 12).Select(m => new Date(2000, m, 01)).ToArray();
            var dates = new DatesSpecification(months);
            var decorated = dates.ApplicableSince(new Date(2000, month, day));
            Assert.Equal(months.Skip(6), decorated.EnumerateBetween(new Date(2000, 01, 01), new Date(2000, 12, 31)));
        }

        [Theory]
        [InlineData(06, 15)]
        [InlineData(06, 01)]
        public void CanApplyASpecificationUntilAGivenDate(int month, int day)
        {
            var months = Enumerable.Range(1, 12).Select(m => new Date(2000, m, 01)).ToArray();
            var dates = new DatesSpecification(months);
            var decorated = dates.ApplicableUntil(new Date(2000, month, day));
            Assert.Equal(months.Take(6), decorated.EnumerateBetween(new Date(2000, 01, 01), new Date(2000, 12, 31)));
        }
    }
}
