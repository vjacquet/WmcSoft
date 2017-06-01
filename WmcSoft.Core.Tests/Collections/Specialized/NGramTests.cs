using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Specialized
{
    [TestClass]
    public class NGramTests
    {
        [TestMethod]
        public void CanEqualNGrams()
        {
            var x = new NGram<int>(1, 2, 3);
            var y = new NGram<int>(new List<int> { 0, 1, 2, 3, 4 }, 1, 3);
            var z = new NGram<int>(new List<int> { 0, 1, 2, 3, 4 }, 1, 2);
            var e = default(NGram<int>);

            Assert.IsTrue(x.Equals(y));
            Assert.IsFalse(x.Equals(z));
            Assert.IsFalse(x.Equals(e));
        }

        [TestMethod]
        public void CanCompareNGrams()
        {
            var x = new NGram<int>(1, 2, 3);
            var y = new NGram<int>(new List<int> { 0, 1, 2, 3, 4 }, 1, 3);
            var z = new NGram<int>(new List<int> { 0, 1, 2, 3, 4 }, 0, 2);
            var e = default(NGram<int>);

            Assert.AreEqual(-1, e.CompareTo(x));
            Assert.AreEqual(0, x.CompareTo(y));
            Assert.AreEqual(1, x.CompareTo(z));
        }

        [TestMethod]
        public void CanDecomposeNGram()
        {
            var ngrams = new NGram<char>('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H');
            var actual = ngrams.Decompose(3).Select(g => string.Join("", g)).ToList();
            var expected = new[] { "ABC", "BCD", "CDE", "DEF", "EFG", "FGH" };
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
