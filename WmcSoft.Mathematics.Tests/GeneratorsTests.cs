using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Tests
{
    [TestClass]
    public class GeneratorsTests
    {
        [TestMethod]
        public void CheckUnarySeries() {
            var series = Generators.Series(x => x + 2, 0).GetEnumerator();
            Assert.AreEqual(0, series.Read());
            Assert.AreEqual(2, series.Read());
            Assert.AreEqual(4, series.Read());
            Assert.AreEqual(6, series.Read());
        }

        [TestMethod]
        public void CheckBinarySeries() {
            var series = Generators.Series((x, y) => x + y, 0, 1).GetEnumerator();
            Assert.AreEqual(0, series.Read());
            Assert.AreEqual(1, series.Read());
            Assert.AreEqual(1, series.Read());
            Assert.AreEqual(2, series.Read());
            Assert.AreEqual(3, series.Read());
            Assert.AreEqual(5, series.Read());
            Assert.AreEqual(8, series.Read());
            Assert.AreEqual(13, series.Read());
            Assert.AreEqual(21, series.Read());
        }

        [TestMethod]
        public void CheckPermutations() {
            var permutations = Generators.Permutations("a", "b", "c", "d", "e", "f");
            var expected = new HashSet<string>();
            var sequence = new List<string>();
            foreach (var p in permutations) {
                var s = p.Join("");
                Assert.IsTrue(expected.Add(s));
                sequence.Add(s);
            }

            Func<int, int> factorial = n => Enumerable.Range(1, n).Aggregate((x, y) => x * y);

            var f = factorial(sequence.First().Length);
            Assert.AreEqual(f, expected.Count);
        }
    }
}
