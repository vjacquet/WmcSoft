using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WmcSoft.Collections.Specialized
{
    public class NGramTests
    {
        [Fact]
        public void CanEqualNGrams()
        {
            var x = new NGram<int>(1, 2, 3);
            var y = new NGram<int>(new List<int> { 0, 1, 2, 3, 4 }, 1, 3);
            var z = new NGram<int>(new List<int> { 0, 1, 2, 3, 4 }, 1, 2);
            var e = default(NGram<int>);

            Assert.True(x.Equals(y));
            Assert.False(x.Equals(z));
            Assert.False(x.Equals(e));
        }

        [Fact]
        public void CanCompareNGrams()
        {
            var x = new NGram<int>(1, 2, 3);
            var y = new NGram<int>(new List<int> { 0, 1, 2, 3, 4 }, 1, 3);
            var z = new NGram<int>(new List<int> { 0, 1, 2, 3, 4 }, 0, 2);
            var e = default(NGram<int>);

            Assert.Equal(-1, e.CompareTo(x));
            Assert.Equal(0, x.CompareTo(y));
            Assert.Equal(1, x.CompareTo(z));
        }

        [Fact]
        public void CanDecomposeNGram()
        {
            var ngrams = new NGram<char>('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H');
            var actual = ngrams.Decompose(3).Select(g => string.Join("", g)).ToList();
            var expected = new[] { "ABC", "BCD", "CDE", "DEF", "EFG", "FGH" };
            Assert.Equal(expected, actual);
        }
    }
}