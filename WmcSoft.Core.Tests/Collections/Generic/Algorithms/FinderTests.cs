using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic.Algorithms
{
    [TestClass]
    public class FinderTests
    {
        [TestMethod]
        public void CanFindUsingNaiveFinder() {
            const string p = "10100111";
            const string t = "100111010010100010100111000111";

            var a = new NaiveFinder<char>(p.AsReadOnlyList(), EqualityComparer<char>.Default);
            var actual = a.FindFirstOccurence(t.AsReadOnlyList(), 5);
            var expected = t.IndexOf(p);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanFindUsingKnuthMorrisPratt() {
            const string p = "10100111";
            const string t = "100111010010100010100111000111";

            var a = new KnuthMorrisPratt<char>(p.AsReadOnlyList(), EqualityComparer<char>.Default);
            var actual = a.FindFirstOccurence(t.AsReadOnlyList(), 5);
            var expected = t.IndexOf(p);
            Assert.AreEqual(expected, actual);
        }
    }
}
