using System;
using System.Linq;
using Xunit;

namespace WmcSoft
{
    public class IntExtensionsTests
    {
        [Fact]
        public void CheckToRangesWhenLastIsSingleton()
        {
            var items = new[] { 1, 2, 3, 5, 7, 8, 9, 10, 12 };
            var ranges = items.ToRanges().ToArray();

            Assert.Equal(4, ranges.Length);
            Assert.Equal(Tuple.Create(1, 3), ranges[0]);
            Assert.Equal(Tuple.Create(5, 5), ranges[1]);
            Assert.Equal(Tuple.Create(7, 10), ranges[2]);
            Assert.Equal(Tuple.Create(12, 12), ranges[3]);
        }

        [Fact]
        public void CheckToRanges()
        {
            var items = new[] { 1, 2, 3, 5, 7, 8, 9, 10, 12, 13, 14, 15 };
            var ranges = items.ToRanges().ToArray();

            Assert.Equal(4, ranges.Length);
            Assert.Equal(Tuple.Create(1, 3), ranges[0]);
            Assert.Equal(Tuple.Create(5, 5), ranges[1]);
            Assert.Equal(Tuple.Create(7, 10), ranges[2]);
            Assert.Equal(Tuple.Create(12, 15), ranges[3]);
        }
    }
}
