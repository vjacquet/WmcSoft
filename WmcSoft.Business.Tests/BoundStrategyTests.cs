using Xunit;

namespace WmcSoft.Business
{
    public class BoundStrategyTests
    {
        [Theory]
        [InlineData(-1, 1, 2, 5)]
        [InlineData(0, 2, 2, 5)]
        [InlineData(0, 4, 2, 5)]
        [InlineData(0, 5, 2, 5)]
        [InlineData(1, 6, 2, 5)]
        public void CheckCompareOnInclusiveStrategy(int expected, int value, int lo, int hi)
        {
            var strategy = BoundStrategy<int>.Inclusive;

            Assert.Equal(expected, strategy.Compare(value, lo, hi));
        }

        [Theory]
        [InlineData(false, 1, 2, 5)]
        [InlineData(true, 2, 2, 5)]
        [InlineData(true, 4, 2, 5)]
        [InlineData(true, 5, 2, 5)]
        [InlineData(false, 6, 2, 5)]
        public void CheckIsWithinRangeOnInclusiveStrategy(bool expected, int value, int lo, int hi)
        {
            var strategy = BoundStrategy<int>.Inclusive;

            Assert.Equal(expected, strategy.IsWithinRange(value, lo, hi));
        }

        [Theory]
        [InlineData(-1, 1, 2, 5)]
        [InlineData(-1, 2, 2, 5)]
        [InlineData(0, 4, 2, 5)]
        [InlineData(1, 5, 2, 5)]
        [InlineData(1, 6, 2, 5)]
        public void CheckCompareOnExclusiveStrategy(int expected, int value, int lo, int hi)
        {
            var strategy = BoundStrategy<int>.Exclusive;

            Assert.Equal(expected, strategy.Compare(value, lo, hi));
        }

        [Theory]
        [InlineData(false, 1, 2, 5)]
        [InlineData(false, 2, 2, 5)]
        [InlineData(true, 4, 2, 5)]
        [InlineData(false, 5, 2, 5)]
        [InlineData(false, 6, 2, 5)]
        public void CheckIsWithinRangeOnExclusiveStrategy(bool expected, int value, int lo, int hi)
        {
            var strategy = BoundStrategy<int>.Exclusive;

            Assert.Equal(expected, strategy.IsWithinRange(value, lo, hi));
        }

        [Theory]
        [InlineData(-1, 1, 2, 5)]
        [InlineData(-1, 2, 2, 5)]
        [InlineData(0, 4, 2, 5)]
        [InlineData(0, 5, 2, 5)]
        [InlineData(1, 6, 2, 5)]
        public void CheckCompareOnLowerExclusiveStrategy(int expected, int value, int lo, int hi)
        {
            var strategy = BoundStrategy<int>.LowerExclusive;

            Assert.Equal(expected, strategy.Compare(value, lo, hi));
        }

        [Theory]
        [InlineData(false, 1, 2, 5)]
        [InlineData(false, 2, 2, 5)]
        [InlineData(true, 4, 2, 5)]
        [InlineData(true, 5, 2, 5)]
        [InlineData(false, 6, 2, 5)]
        public void CheckIsWithinRangeOnLowerExclusiveStrategy(bool expected, int value, int lo, int hi)
        {
            var strategy = BoundStrategy<int>.LowerExclusive;

            Assert.Equal(expected, strategy.IsWithinRange(value, lo, hi));
        }

        [Theory]
        [InlineData(-1, 1, 2, 5)]
        [InlineData(0, 2, 2, 5)]
        [InlineData(0, 4, 2, 5)]
        [InlineData(1, 5, 2, 5)]
        [InlineData(1, 6, 2, 5)]
        public void CheckCompareOnLowerUpperExclusiveStrateg(int expected, int value, int lo, int hi)
        {
            var strategy = BoundStrategy<int>.UpperExclusive;

            Assert.Equal(expected, strategy.Compare(value, lo, hi));
        }

        [Theory]
        [InlineData(false, 1, 2, 5)]
        [InlineData(true, 2, 2, 5)]
        [InlineData(true, 4, 2, 5)]
        [InlineData(false, 5, 2, 5)]
        [InlineData(false, 6, 2, 5)]
        public void CheckIsWithinRangeOnUpperExclusiveStrateg(bool expected, int value, int lo, int hi)
        {
            var strategy = BoundStrategy<int>.UpperExclusive;

            Assert.Equal(expected, strategy.IsWithinRange(value, lo, hi));
        }
    }
}