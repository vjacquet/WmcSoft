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

            Assert.Equal(new Date(2017, 9, 22), calendar.NextBusinessDay(new Date(2017, 9, 21)));
            Assert.Equal(new Date(2017, 9, 25), calendar.NextBusinessDay(new Date(2017, 9, 22)));
        }

        [Theory]
        [InlineData(2002, 3, 31)]
        [InlineData(2017, 4, 16)]
        [InlineData(2019, 4, 21)]
        [InlineData(2042, 4, 6)]
        public void CanComputeEaster(int year, int month, int day)
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
        public void CanComputeEasterFriday(int year, int month, int day)
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
        public void CanComputeEasterMonday(int year, int month, int day)
        {
            var specification = KnownHolidays.GregorianEasterMonday;
            var expected = new Date(year, month, day);
            var actual = specification.OfYear(year);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCreateBespokeCalendar()
        {
            var reference = new BusinessCalendar(new Date(2000, 1, 1), new Date(2002, 1, 1)
                , Weekends.Every(DayOfWeek.Saturday, DayOfWeek.Sunday)
            );
            var newYear = new Date(2001, 01, 01);
            var xmas = new Date(2001, 12, 25);

            var calendar1 = new BespokeCalendar<BusinessCalendar>("xmas", reference);
            calendar1.Add(KnownHolidays.Christmas);
            Assert.False(calendar1.IsHoliday(newYear));
            Assert.True(calendar1.IsHoliday(xmas));

            var calendar2 = new BespokeCalendar<BusinessCalendar>("new year", reference);
            calendar2.Add(KnownHolidays.NewYearDay);
            Assert.True(calendar2.IsHoliday(newYear));
            Assert.False(calendar2.IsHoliday(xmas));
        }

        [Fact]
        public void CanJoinHolidays()
        {
            var reference = new BusinessCalendar(new Date(2000, 1, 1), new Date(2002, 1, 1)
                , Weekends.Every(DayOfWeek.Saturday, DayOfWeek.Sunday)
            );
            var newYear = new Date(2001, 01, 01);
            var xmas = new Date(2001, 12, 25);

            var calendar1 = new BespokeCalendar<BusinessCalendar>("xmas", reference);
            calendar1.Add(KnownHolidays.Christmas);

            var calendar2 = new BespokeCalendar<BusinessCalendar>("new year", reference);
            calendar2.Add(KnownHolidays.NewYearDay);

            var calendar = new JoinHolidaysCalendar(calendar1, calendar2);
            Assert.True(calendar.IsHoliday(newYear));
            Assert.True(calendar.IsHoliday(xmas));
        }

        [Fact]
        public void CanJoinBusinessDay()
        {
            var reference = new BusinessCalendar(new Date(2000, 1, 1), new Date(2002, 1, 1)
                , Weekends.Every(DayOfWeek.Saturday, DayOfWeek.Sunday)
            );

            var calendar1 = new BespokeCalendar<BusinessCalendar>("xmas", reference);
            calendar1.Add(KnownHolidays.Christmas);

            var calendar2 = new BespokeCalendar<BusinessCalendar>("new year", reference);
            calendar2.Add(KnownHolidays.NewYearDay);

            var newYear = new Date(2001, 01, 01);
            var xmas = new Date(2001, 12, 25);

            var calendar = new JoinBusinessDaysCalendar(calendar1, calendar2);
            Assert.False(calendar.IsHoliday(newYear));
            Assert.False(calendar.IsHoliday(xmas));
        }

        [Fact]
        public void CanJoinQuorumBusinessDays()
        {
            var reference = new BusinessCalendar(new Date(2000, 1, 1), new Date(2002, 1, 1)
                , Weekends.Every(DayOfWeek.Saturday, DayOfWeek.Sunday)
            );
            var newYear = new Date(2001, 01, 01);
            var labour = new Date(2001, 05, 01);
            var xmas = new Date(2001, 12, 25);

            var calendar1 = new BespokeCalendar<BusinessCalendar>("xmas", reference);
            calendar1.Add(KnownHolidays.Christmas);

            var calendar2 = new BespokeCalendar<BusinessCalendar>("new year", reference);
            calendar2.Add(KnownHolidays.NewYearDay);

            var calendar3 = new BespokeCalendar<BusinessCalendar>("all", reference);
            calendar3.Add(KnownHolidays.Christmas);
            calendar3.Add(KnownHolidays.NewYearDay);
            calendar3.Add(KnownHolidays.LabourDay);

            var calendar = new JoinQuorumBusinessDaysCalendar(2, calendar1, calendar2, calendar3);

            Assert.True(calendar.IsHoliday(newYear));
            Assert.False(calendar.IsHoliday(labour));
            Assert.True(calendar.IsHoliday(xmas));
        }
    }
}
