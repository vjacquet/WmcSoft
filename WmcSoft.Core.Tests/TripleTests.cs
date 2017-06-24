using System;
using Xunit;

namespace WmcSoft
{
    public class TripleTests
    {
        [Fact]
        public void CanCompareTriple()
        {
            var t1 = Triple.Create(1, 2, 3);
            var t2 = Triple.Create(1, 2, 5);
            var t3 = Triple.Create(2, 2, 2);

            Assert.True(t1 < t2);
            Assert.True(t2 <= t3);
            Assert.True(t3 > t1);
            Assert.True(t3 >= t2);

            Assert.False(t1 == t2);
            Assert.True(t2 != t3);
        }

        [Fact]
        public void CanConvertTripleToTuple()
        {
            var p = Triple.Create(1, 2, 3);
            Tuple<int, int, int> t = p;

            Assert.Equal(p.Item1, t.Item1);
            Assert.Equal(p.Item2, t.Item2);
            Assert.Equal(p.Item3, t.Item3);
        }

        [Fact]
        public void CanConvertTripleToFromTuple()
        {
            var t = Tuple.Create(1, 2, 3);
            Triple<int> p = t;

            Assert.Equal(t.Item1, p.Item1);
            Assert.Equal(t.Item2, p.Item2);
            Assert.Equal(p.Item3, t.Item3);
        }

        [Fact]
        public void CanDeconstructTriple()
        {
            var p = Triple.Create(1, 66, 4);
            var (first, middle, last) = p;

            Assert.Equal(1, first);
            Assert.Equal(66, middle);
            Assert.Equal(4, last);
        }

        [Fact]
        public void CheckTripleToString()
        {
            var p = Triple.Create(1, 66, 4);
            Assert.Equal("(1, 66, 4)", p.ToString());
        }
    }
}