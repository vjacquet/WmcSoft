using Xunit;

namespace WmcSoft
{
    public class IntervalLimitTests
    {
        [Theory]
        [InlineData(4, null, null)]
        [InlineData(4, 1, null)]
        [InlineData(4, null, 9)]
        [InlineData(4, 1, 9)]
        public void ValueIsInRange(int value, int? lo, int? hi)
        {
            Assert.True(!(value < lo) && !(value >= hi));
        }

        [Theory]
        [InlineData(0, 1, null)]
        [InlineData(10, null, 9)]
        [InlineData(10, 1, 9)]
        [InlineData(0, 1, 9)]
        public void ValueIsNotInRange(int value, int? lo, int? hi)
        {
            Assert.False(!(value < lo) && !(value >= hi));
        }

        [Fact]
        public void CanCompareIntervalLimits()
        {
            var one = Interval.LowerLimit(1, false);
            var two = Interval.UpperLimit(2, false);

            Assert.True(one < two);
            Assert.True(two > one);
        }

        [Fact]
        public void CheckLowerAndUpperOfSameValueAreEqual()
        {
            var lower = Interval.LowerLimit(1, false);
            var upper = Interval.UpperLimit(1, false);
            Assert.True(lower == upper);
        }

        [Fact]
        public void CanCompareIntervalLimitsWithUndefined()
        {
            var undefined = IntervalLimit<int>.Undefined;
            var lower = Interval.LowerLimit(1, false);
            var upper = Interval.UpperLimit(1, false);

            Assert.True(undefined < lower);
            Assert.True(lower > undefined);
            Assert.True(upper < undefined);
            Assert.True(undefined > upper);
        }
    }
}
