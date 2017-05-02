using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class TripleTests
    {
        [TestMethod]
        public void CanCompareTriple()
        {
            var t1 = Triple.Create(1, 2, 3);
            var t2 = Triple.Create(1, 2, 5);
            var t3 = Triple.Create(2, 2, 2);

            Assert.IsTrue(t1 < t2);
            Assert.IsTrue(t2 <= t3);
            Assert.IsTrue(t3 > t1);
            Assert.IsTrue(t3 >= t2);

            Assert.IsFalse(t1 == t2);
            Assert.IsTrue(t2 != t3);
        }

        [TestMethod]
        public void CanConvertTripleToTuple()
        {
            var p = Triple.Create(1, 2, 3);
            Tuple<int, int, int> t = p;

            Assert.AreEqual(p.Item1, t.Item1);
            Assert.AreEqual(p.Item2, t.Item2);
            Assert.AreEqual(p.Item3, t.Item3);
        }

        [TestMethod]
        public void CanConvertTripleToFromTuple()
        {
            var t = Tuple.Create(1, 2, 3);
            Triple<int> p = t;

            Assert.AreEqual(t.Item1, p.Item1);
            Assert.AreEqual(t.Item2, p.Item2);
            Assert.AreEqual(p.Item3, t.Item3);
        }

        [TestMethod]
        public void CanDeconstructTriple()
        {
            var p = Triple.Create(1, 66, 4);
            var (first, middle, last) = p;

            Assert.AreEqual(1, first);
            Assert.AreEqual(66, middle);
            Assert.AreEqual(4, last);
        }

        [TestMethod]
        public void CheckTripleToString()
        {
            var p = Triple.Create(1, 66, 4);
            Assert.AreEqual("(1, 66, 4)", p.ToString());
        }
    }
}
