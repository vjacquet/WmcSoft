using Xunit;

using static WmcSoft.Globalization.Dates;

namespace WmcSoft.Globalization
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
