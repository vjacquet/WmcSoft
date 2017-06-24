using Xunit;

namespace WmcSoft
{
    public class DateSpanTests
    {
        [Fact]
        public void CheckLessThan()
        {
            var x = new DateSpan(months: 1);
            var y = new DateSpan(days: 31);
            Assert.True(x < y);
        }

        [Fact]
        public void CheckEquals()
        {
            var x = new DateSpan(weeks: 1);
            var y = new DateSpan(days: 7);
            Assert.True(x == y);
        }

        [Fact]
        public void CanDeconstructDateSpan()
        {
            var x = new DateSpan(1, 2, 3, 4);
            var (y, m, w, d) = x;

            Assert.Equal(1, y);
            Assert.Equal(2, m);
            Assert.Equal(3, w);
            Assert.Equal(4, d);
        }
    }
}
