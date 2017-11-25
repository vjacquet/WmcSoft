using System;
using Xunit;

namespace WmcSoft
{
    public class PairTests
    {
        [Fact]
        public void CanComparePair()
        {
            var p1 = Pair.Create(1, 2);
            var p2 = Pair.Create(1, 5);
            var p3 = Pair.Create(2, 2);

            Assert.True(p1 < p2);
            Assert.True(p2 <= p3);
            Assert.True(p3 > p1);
            Assert.True(p3 >= p2);

            Assert.False(p1 == p2);
            Assert.True(p2 != p3);
        }

        [Fact]
        public void CanConvertPairToTuple()
        {
            var p = Pair.Create(1, 2);
            Tuple<int, int> t = p;

            Assert.Equal(p.Item1, t.Item1);
            Assert.Equal(p.Item2, t.Item2);
        }

        [Fact]
        public void CanConvertPairToFromTuple()
        {
            var t = Tuple.Create(1, 2);
            Pair<int> p = t;

            Assert.Equal(t.Item1, p.Item1);
            Assert.Equal(t.Item2, p.Item2);
        }

        [Fact]
        public void CanDeconstructPair()
        {
            var p = Pair.Create(16, 64);
            var (first, last) = p;

            Assert.Equal(16, first);
            Assert.Equal(64, last);
        }

        [Fact]
        public void CheckPairToString()
        {
            var p = Pair.Create(16, 64);
            Assert.Equal("(16, 64)", p.ToString());
        }

        [Fact]
        public void CanConvertPairToValueTuple()
        {
            var vt = (1, 2);
            Pair<int> p = vt;

            Assert.Equal(vt.Item1, p.Item1);
            Assert.Equal(vt.Item2, p.Item2);
        }

        [Fact]
        public void CanConvertValueTupleToPair()
        {
            var p = Pair.Create(1, 2);
            ValueTuple<int, int> vt = p;

            Assert.Equal(vt.Item1, p.Item1);
            Assert.Equal(vt.Item2, p.Item2);
        }
    }
}
