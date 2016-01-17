using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic.Algorithms
{
    [TestClass]
    public class FinderTests
    {
        [TestMethod]
        public void CanFindUsingKnuthMorrisPratt() {
            const string p = "10100111";
            const string t = "100111010010100010100111000111";

            var a = new KnuthMorrisPratt<char>(p.AsReadOnlyList(), EqualityComparer<char>.Default);
            var actual = a.Find(t.AsReadOnlyList());
            var expected = t.IndexOf(p);
            Assert.AreEqual(expected, actual);
        }
    }
}
