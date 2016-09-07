using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Text
{
    [TestClass]
    public class SuffixArrayTests
    {
        [TestMethod]
        public void CanCreateSuffixArray() {
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
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
