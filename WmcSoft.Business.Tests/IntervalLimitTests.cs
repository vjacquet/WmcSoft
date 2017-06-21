using Xunit;

namespace WmcSoft
{
    public class IntervalLimitTests
    {
        [Fact]
        public void CanCompareIntervalLimits()
        {
            var one = IntervalLimit.Lower(1, false);
            var two = IntervalLimit.Upper(2, false);

            Assert.True(one < two);
            Assert.True(two > one);
        }

        [Fact]
        public void CheckLowerAndUpperOfSameValueAreEqual()
        {
            var lower = IntervalLimit.Lower(1, false);
            var upper = IntervalLimit.Upper(1, false);
            Assert.True(lower == upper);
        }

        [Fact]
        public void CanCompareIntervalLimitsWithUndefined()
        {
            var undefined = IntervalLimit<int>.Undefined;
            var lower = IntervalLimit.Lower(1, false);
            var upper = IntervalLimit.Upper(1, false);

            Assert.True(undefined < lower);
            Assert.True(upper < undefined);
        }
    }
}
