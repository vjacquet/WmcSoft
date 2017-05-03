using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Numerics.Tests
{
    [TestClass]
    public class BigFractionTests
    {
        [TestMethod]
        public void CanAddWithRelativePrimeDenominators()
        {
            var x = new BigFraction(1, 2);
            var y = new BigFraction(1, 3);

            Assert.AreEqual(new BigFraction(5, 6), x + y);
        }

        [TestMethod]
        public void CanCompareBigFractions()
        {
            var data = new[] {
                new BigFraction(-3,2),
                new BigFraction(-1,2),
                BigFraction.Zero,
                new BigFraction(1,2),
                new BigFraction(3,2),
            };

            Assert.IsTrue(data.IsSorted());
            Assert.IsTrue(data.Backwards().IsSorted(Comparer<BigFraction>.Default.Reverse()));
        }

        [TestMethod]
        public void CanSubtractRelativePrimeDenominators()
        {
            var x = new BigFraction(1, 2);
            var y = new BigFraction(1, 3);

            Assert.AreEqual(new BigFraction(1, 6), x - y);
        }

        [TestMethod]
        public void CanAddFractions()
        {
            var x = new BigFraction(1, 4);
            var y = new BigFraction(1, 6);

            Assert.AreEqual(new BigFraction(5, 12), x + y);
        }

        [TestMethod]
        public void CanSubtractFractions()
        {
            var x = new BigFraction(1, 4);
            var y = new BigFraction(1, 6);

            Assert.AreEqual(new BigFraction(1, 12), x - y);
        }

        [TestMethod]
        public void CanMultiplyFractions()
        {
            var x = new BigFraction(1, 2);
            var y = new BigFraction(1, 3);

            Assert.AreEqual(new BigFraction(1, 6), x * y);
        }

        [TestMethod]
        public void CanDivideFractions()
        {
            var x = new BigFraction(1, 2);
            var y = new BigFraction(1, 3);

            Assert.AreEqual(new BigFraction(3, 2), x / y);
        }

        [TestMethod]
        public void CanSimplifyFraction()
        {
            var x = new BigFraction(2, 6);
            Assert.AreEqual(new BigFraction(1, 3), x);
        }

        [TestMethod]
        public void CanDeconstructBigFraction()
        {
            var q = new BigFraction(4, 3);
            var (n, d) = q;

            Assert.AreEqual(4, n);
            Assert.AreEqual(3, d);
        }
    }
}
