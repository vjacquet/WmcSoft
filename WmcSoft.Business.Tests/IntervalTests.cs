using Xunit;

namespace WmcSoft
{
    public class IntervalTests
    {
        [Fact]
        public void CheckIsEmpty()
        {
            var empty = Interval.Open(1, 1);
            Assert.True(empty.IsOpen());
            Assert.True(empty.IsEmpty());
        }

        [Fact]
        public void CheckIsSingleElement()
        {
            var empty = Interval.Closed(1, 1);
            Assert.True(empty.IsClosed());
            Assert.True(empty.IsSingleElement());
        }

        [Fact]
        public void CheckMixedIsEmpty()
        {
            var actual = new Interval<int>(IntervalLimit.Lower(1, true), IntervalLimit.Upper(1, false));
            Assert.False(actual.IsOpen());
            Assert.False(actual.IsEmpty());
        }

        [Fact]
        public void CheckIncludesOnOpenInterval()
        {
            var open = Interval.Open(1, 10);

            Assert.True(open.Includes(4));
            Assert.True(open.Includes(8));
            Assert.False(open.Includes(1));
            Assert.False(open.Includes(10));
            Assert.False(open.Includes(11));
            Assert.False(open.Includes(0));
        }

        [Fact]
        public void CheckIncludesOnClosedInterval()
        {
            var open = Interval.Closed(1, 10);

            Assert.True(open.Includes(4));
            Assert.True(open.Includes(8));
            Assert.True(open.Includes(1));
            Assert.True(open.Includes(10));
            Assert.False(open.Includes(11));
            Assert.False(open.Includes(0));
        }
    }
}