using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Numerics.Tests
{
    [TestClass]
    public class FractionTests
    {
        [TestMethod]
        public void CanAddWithRelativePrimeDenominators()
        {
            var x = new Fraction(1, 2);
            var y = new Fraction(1, 3);

            Assert.AreEqual(new Fraction(5, 6), x + y);
        }

        [TestMethod]
        public void CanSubtractRelativePrimeDenominators()
        {
            var x = new Fraction(1, 2);
            var y = new Fraction(1, 3);

            Assert.AreEqual(new Fraction(1, 6), x - y);
        }

        [TestMethod]
        public void CanAddFractions()
        {
            var x = new Fraction(1, 4);
            var y = new Fraction(1, 6);

            Assert.AreEqual(new Fraction(5, 12), x + y);
        }

        [TestMethod]
        public void CanSubtractFractions()
        {
            var x = new Fraction(1, 4);
            var y = new Fraction(1, 6);

            Assert.AreEqual(new Fraction(1, 12), x - y);
        }

        [TestMethod]
        public void CanMultiplyFractions()
        {
            var x = new Fraction(1, 2);
            var y = new Fraction(1, 3);

            Assert.AreEqual(new Fraction(1, 6), x * y);
        }

        [TestMethod]
        public void CanDivideFractions()
        {
            var x = new Fraction(1, 2);
            var y = new Fraction(1, 3);

            Assert.AreEqual(new Fraction(3, 2), x / y);
        }

        [TestMethod]
        public void CanCastFractionToInt()
        {
            var q = new Fraction(4, 2);

            Assert.AreEqual(2, (int)q);
        }

        [TestMethod]
        public void CanSimplifyFraction()
        {
            var x = new Fraction(2, 6);
            Assert.AreEqual(new Fraction(1, 3), x);
        }

        [TestMethod]
        public void CanDeconstructFraction()
        {
            var q = new Fraction(4, 3);
            var (n, d) = q;

            Assert.AreEqual(4, n);
            Assert.AreEqual(3, d);
        }
    }
}
