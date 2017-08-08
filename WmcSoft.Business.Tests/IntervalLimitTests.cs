using Xunit;

namespace WmcSoft
{
    public class IntervalLimitTests
    {
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
