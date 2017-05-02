using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class PairTests
    {
        [TestMethod]
        public void CanComparePair()
        {
            var p1 = Pair.Create(1, 2);
            var p2 = Pair.Create(1, 5);
            var p3 = Pair.Create(2, 2);

            Assert.IsTrue(p1 < p2);
            Assert.IsTrue(p2 <= p3);
            Assert.IsTrue(p3 > p1);
            Assert.IsTrue(p3 >= p2);

            Assert.IsFalse(p1 == p2);
            Assert.IsTrue(p2 != p3);
        }

        [TestMethod]
        public void CanConvertPairToTuple()
        {
            var p = Pair.Create(1, 2);
            Tuple<int, int> t = p;

            Assert.AreEqual(p.Item1, t.Item1);
            Assert.AreEqual(p.Item2, t.Item2);
        }

        [TestMethod]
        public void CanConvertPairToFromTuple()
        {
            var t = Tuple.Create(1, 2);
            Pair<int> p = t;

            Assert.AreEqual(t.Item1, p.Item1);
            Assert.AreEqual(t.Item2, p.Item2);
        }

        [TestMethod]
        public void CanDeconstructPair()
        {
            var p = Pair.Create(16, 64);
            var (first, last) = p;

            Assert.AreEqual(16, first);
            Assert.AreEqual(64, last);
        }

        [TestMethod]
        public void CheckPairToString()
        {
            var p = Pair.Create(16, 64);
            Assert.AreEqual("(16, 64)", p.ToString());
        }
    }
}
