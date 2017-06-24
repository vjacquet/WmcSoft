using System.Linq;
using Xunit;

namespace WmcSoft.Text
{
    public class SuffixArrayTests
    {
        [Fact]
        public void CanCreateSuffixArray()
        {
            var suffixes = new SuffixArray("aacaagtttacaagc");
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
            Assert.Equal(expected, actual);
        }
    }
}
