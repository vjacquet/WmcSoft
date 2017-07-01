using System.Linq;
using Xunit;

namespace WmcSoft.Text
{
    public class SuffixArrayTests : IClassFixture<SuffixArrayTests.Fixture>
    {
        public class Fixture
        {
            public Fixture()
            {
                FirstSet = new SuffixArray("aacaagtttacaagc");
                SecondSet = new SuffixArray("it was the best of times it was the");
            }

            public SuffixArray FirstSet;
            public SuffixArray SecondSet;
        }

        readonly Fixture _fixture;

        public SuffixArrayTests(Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateSuffixArray()
        {
            var suffixes = _fixture.FirstSet;
            var actual = suffixes.ToArray();
            var expected = new[] {
                "aacaagtttacaagc",
                "aagc",
                "aagtttacaagc",
                "acaagc",
                "acaagtttacaagc",
                "agc",
                "agtttacaagc",
                "c",
                "caagc",
                "caagtttacaagc",
                "gc",
                "gtttacaagc",
                "tacaagc",
                "ttacaagc",
                "tttacaagc",
            };
            Assert.Equal(expected.Length, suffixes.Count);
            Assert.Equal(expected, actual);

            for (int i = 0; i < suffixes.Count; i++) {
                Assert.True(expected[i].Length == suffixes.GetLengthOf(i), $"GetLengthOf({i})={suffixes.GetLengthOf(i)} invalid for '{expected[i]}'.");
            }
        }

        [Theory]
        [InlineData("b", 7)]
        [InlineData("aagc", 1)]
        [InlineData("aagcb", 2)]
        public void CanRank(string key, int rank)
        {
            var suffixes = _fixture.FirstSet;
            Assert.Equal(rank, suffixes.Rank(key));
        }

        [Fact]
        public void CanGetLongestCommonPrefix()
        {
            var suffixes = _fixture.SecondSet;
            var actual = suffixes.GetLongestCommonPrefix(20);
            Assert.Equal(10, actual);
            Assert.Equal(10, "it was the".Length);
        }

        [Theory]
        [InlineData(0, 10, 0)]
        [InlineData(1, 24, 1)]
        [InlineData(2, 15, 1)]
        [InlineData(3, 31, 1)]
        [InlineData(4, 6, 4)]
        [InlineData(5, 18, 2)]
        [InlineData(6, 27, 1)]
        [InlineData(7, 2, 8)]
        [InlineData(8, 29, 0)]
        [InlineData(9, 4, 6)]
        [InlineData(10, 11, 0)]
        [InlineData(11, 34, 0)]
        [InlineData(12, 9, 1)]
        [InlineData(13, 22, 1)]
        [InlineData(14, 12, 2)]
        [InlineData(15, 17, 0)]
        [InlineData(16, 33, 0)]
        [InlineData(17, 8, 2)]
        [InlineData(18, 20, 0)]
        [InlineData(19, 25, 1)]
        [InlineData(20, 0, 10)]
        [InlineData(21, 21, 0)]
        [InlineData(22, 16, 0)]
        [InlineData(23, 23, 0)]
        [InlineData(24, 30, 2)]
        [InlineData(25, 5, 5)]
        [InlineData(26, 13, 1)]
        [InlineData(27, 14, 0)]
        [InlineData(28, 26, 2)]
        [InlineData(29, 1, 9)]
        [InlineData(30, 32, 1)]
        [InlineData(31, 7, 3)]
        [InlineData(32, 19, 1)]
        [InlineData(33, 28, 0)]
        [InlineData(34, 3, 7)]
        public void CompareResultsToSedgewick(int i, int indexOf, int lcp)
        {
            // See Algorithms, Fourth edition, Robert Sedgewick & Kevin Wayne, Page 878.
            var suffixes = _fixture.SecondSet;
            Assert.Equal(indexOf, suffixes.IndexOf(i));
            Assert.Equal(lcp, suffixes.GetLongestCommonPrefix(i));
        }
    }
}
