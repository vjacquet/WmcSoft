using Xunit;

using static WmcSoft.Time.Dates;

namespace WmcSoft.Time
{
    public class DatesTests
    {
        [Fact]
        public void TheDayBeforeTomorrowIsToday()
        {
            Assert.Equal(Today, DayBefore(Tomorrow));
        }

        [Fact]
        public void TheDayAfterYesterdayIsToday()
        {
            Assert.Equal(Today, DayAfter(Yesterday));
        }
    }
}
