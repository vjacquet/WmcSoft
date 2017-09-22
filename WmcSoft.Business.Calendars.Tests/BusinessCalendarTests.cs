using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Business.Calendars.Specifications;
using WmcSoft.Time;
using Xunit;

namespace WmcSoft.Business.Calendars
{
    public class BusinessCalendarTests
    {
        [Fact]
        public void CanCreateBusinessCalendarWithWeekends()
        {
            var since = new DateTime(2015, 1, 1);
            var until = since.AddDays(100);

            var calendar = new BusinessCalendar(
                since,
                until,
                new WeekendSpecification(DayOfWeek.Saturday, DayOfWeek.Sunday));

            Assert.True(calendar.IsBusinessDay(new Date(2015, 1, 2)));
            Assert.False(calendar.IsBusinessDay(new Date(2015, 1, 3)));
            Assert.False(calendar.IsBusinessDay(new Date(2015, 1, 4)));
            Assert.True(calendar.IsBusinessDay(new Date(2015, 1, 5)));

            while (since <= until) {
                Assert.Equal(since.DayOfWeek == DayOfWeek.Saturday || since.DayOfWeek == DayOfWeek.Sunday, !calendar.IsBusinessDay(since));
                since = since.AddDays(1);
            }
        }

        [Fact]
        public void CanGetNextBusinessDay()
        {
            var since = new DateTime(2017, 9, 1);
            var until = since.AddDays(100);

            var calendar = new BusinessCalendar(
                since,
                until,
                new WeekendSpecification(DayOfWeek.Saturday, DayOfWeek.Sunday));

            Assert.Equal(new Date(2017, 9, 22), calendar.NextBusinessDay( new Date(2017, 9, 21)));
            Assert.Equal(new Date(2017, 9, 25), calendar.NextBusinessDay(new Date(2017, 9, 22)));
        }

        [Theory]
        [InlineData(2002, 3, 31)]
        [InlineData(2017, 4, 16)]
        [InlineData(2019, 4, 21)]
        [InlineData(2042, 4, 6)]
        public void CanComputerEaster(int year, int month, int day)
        {
            var specification = KnownHolidays.GregorianEaster;
            var expected = new Date(year, month, day);
            var actual = specification.OfYear(year);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2002, 3, 29)]
        [InlineData(2017, 4, 14)]
        [InlineData(2019, 4, 19)]
        [InlineData(2042, 4, 4)]
        public void CanComputerEasterFriday(int year, int month, int day)
        {
            var specification = KnownHolidays.GregorianEasterFriday;
            var expected = new Date(year, month, day);
            var actual = specification.OfYear(year);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2002, 4, 1)]
        [InlineData(2017, 4, 17)]
        [InlineData(2019, 4, 22)]
        [InlineData(2042, 4, 7)]
        public void CanComputerEasterMonday(int year, int month, int day)
        {
            var specification = KnownHolidays.GregorianEasterMonday;
            var expected = new Date(year, month, day);
            var actual = specification.OfYear(year);
            Assert.Equal(expected, actual);
        }
    }
}
