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
        public void CanCreateBusinessCalendarWithoutWeekEnds()
        {
            var calendar = new BusinessCalendar(
                new Date(2015, 1, 1),
                TimeSpan.FromDays(100),
                BusinessCalendar.Saturdays,
                BusinessCalendar.Sundays);

            Assert.True(calendar.IsBusinessDay(new Date(2015, 1, 2)));
            Assert.False(calendar.IsBusinessDay(new Date(2015, 1, 3)));
            Assert.False(calendar.IsBusinessDay(new Date(2015, 1, 4)));
            Assert.True(calendar.IsBusinessDay(new Date(2015, 1, 5)));

            var since = new DateTime(2015, 1, 5);
            var until = new DateTime(2015, 1, 1).AddDays(100);
            while(since <= until) {
                Assert.Equal(since.DayOfWeek == DayOfWeek.Saturday || since.DayOfWeek == DayOfWeek.Sunday, !calendar.IsBusinessDay(since));
                since = since.AddDays(1);
            }
        }

        [Fact]
        public void CanCreateBusinessCalendarWithWeekends()
        {
            var calendar = new BusinessCalendar(
                new Date(2015, 1, 1),
                TimeSpan.FromDays(100),
                new WeekendSpecification(DayOfWeek.Saturday, DayOfWeek.Sunday));

            Assert.True(calendar.IsBusinessDay(new Date(2015, 1, 2)));
            Assert.False(calendar.IsBusinessDay(new Date(2015, 1, 3)));
            Assert.False(calendar.IsBusinessDay(new Date(2015, 1, 4)));
            Assert.True(calendar.IsBusinessDay(new Date(2015, 1, 5)));

            var since = new DateTime(2015, 1, 5);
            var until = new DateTime(2015, 1, 1).AddDays(100);
            while (since <= until) {
                Assert.Equal(since.DayOfWeek == DayOfWeek.Saturday || since.DayOfWeek == DayOfWeek.Sunday, !calendar.IsBusinessDay(since));
                since = since.AddDays(1);
            }
        }
    }
}
