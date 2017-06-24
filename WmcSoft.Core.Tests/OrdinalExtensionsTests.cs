using System;
using System.Linq;
using Xunit;

namespace WmcSoft
{
    public class OrdinalExtensionsTests
    {
        [Fact]
        public void CanCollate()
        {
            var sequence = new[] { 1, 2, 5, 7, 8, 9 };
            Int32Ordinal ordinal;

            var actual = sequence.Collate(ordinal, Tuple.Create).ToArray();
            var expected = new[] { Tuple.Create(1, 2), Tuple.Create(5, 5), Tuple.Create(7, 9) };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCollateWithSingleElementAtEnd()
        {
            var sequence = new[] { 1, 2, 5, 7, 8, 9, 11 };
            Int32Ordinal ordinal;

            var actual = sequence.Collate(ordinal, Tuple.Create).ToArray();
            var expected = new[] { Tuple.Create(1, 2), Tuple.Create(5, 5), Tuple.Create(7, 9), Tuple.Create(11, 11) };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCollateOneElement()
        {
            var sequence = new[] { 1 };
            Int32Ordinal ordinal;

            var actual = sequence.Collate(ordinal, Tuple.Create).ToArray();
            var expected = new[] { Tuple.Create(1, 1) };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCollateOneRange()
        {
            var sequence = new[] { 1, 2, 3 };
            Int32Ordinal ordinal;

            var actual = sequence.Collate(ordinal, Tuple.Create).ToArray();
            var expected = new[] { Tuple.Create(1, 3) };

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanExpand()
        {
            var data = new[] { new Pair<int>(1, 3), new Pair<int>(5, 8) };
            Int32Ordinal ordinal;

            var actual = ordinal.Expand(data).ToArray();
            var expected = new[] { 1, 2, 3, 5, 6, 7, 8 };

            Assert.Equal(expected, actual);
        }
    }
}
