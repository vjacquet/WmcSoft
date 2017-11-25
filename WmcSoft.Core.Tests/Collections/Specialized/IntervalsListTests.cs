using System;
using System.Collections.Generic;
using Xunit;

namespace WmcSoft.Collections.Specialized
{
    public class IntervalsListTests
    {
        private IntervalsList<int> BuildIntervals()
        {
            return new IntervalsList<int>(0, 100) {
                (2, 5),
                (3, 6),
                (7, 9),
                (12, 15),
                (15, 16),
                (11, 12)
            };
        }

        [Fact]
        public void CanConstructIntervalsList()
        {
            var intervals = BuildIntervals();

            var expected = new List<ValueTuple<int, int>>() {
                (2, 6),
                (7, 9),
                (11, 16)
            };

            Assert.Equal(expected, intervals);
        }

        [Fact]
        public void CanConstructIntervalsListWithOneOverallInterval()
        {
            var intervals = BuildIntervals();
            intervals.Add(1, 17);

            var expected = new List<ValueTuple<int, int>>() {
                (1, 17)
            };

            Assert.Equal(expected, intervals);
        }

        [Fact]
        public void CanConstructIntervalsListWithIncludedInterval()
        {
            var intervals = BuildIntervals();
            intervals.Add(2, 16);
            intervals.Add(4, 8);

            var expected = new List<ValueTuple<int, int>>() {
                (2, 16)
            };

            Assert.Equal(expected, intervals);
        }

        [Fact]
        public void CanConstructIntervalsListWithJoiningInterval()
        {
            var intervals = BuildIntervals();
            intervals.Add(4, 8);

            var expected = new List<ValueTuple<int, int>>() {
                (2, 9),
                (11, 16)
            };

            Assert.Equal(expected, intervals);
        }
    }
}
