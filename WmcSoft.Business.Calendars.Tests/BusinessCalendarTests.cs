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
            var since = new DateTime(2015, 1, 1);
            var until = since.AddDays(100);

            var calendar = new BusinessCalendar(
                since,
                until,
                BusinessCalendar.Saturdays,
                BusinessCalendar.Sundays);

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
    }
}
