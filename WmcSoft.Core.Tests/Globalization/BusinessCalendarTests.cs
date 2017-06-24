using System;
using Xunit;

namespace WmcSoft.Globalization
{
    public class BusinessCalendarTests
    {
        [Fact]
        public void CanCreateBusinessCalendarWithoutWeekEnds()
        {
            var calendar = new BusinessCalendar(new DateTime(2015, 1, 1), TimeSpan.FromDays(100), BusinessCalendar.Saturdays, BusinessCalendar.Sundays);
            Assert.True(calendar.IsBusinessDay(new DateTime(2015, 1, 2)));
            Assert.False(calendar.IsBusinessDay(new DateTime(2015, 1, 3)));
            Assert.False(calendar.IsBusinessDay(new DateTime(2015, 1, 4)));
            Assert.True(calendar.IsBusinessDay(new DateTime(2015, 1, 5)));
        }
    }
}
