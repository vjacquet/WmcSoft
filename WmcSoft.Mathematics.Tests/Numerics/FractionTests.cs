using Xunit;

namespace WmcSoft.Numerics
{
    public class FractionTests
    {
        [Fact]
        public void CanAddWithRelativePrimeDenominators()
        {
            var x = new Fraction(1, 2);
            var y = new Fraction(1, 3);

            Assert.Equal(new Fraction(5, 6), x + y);
        }

        [Fact]
        public void CanSubtractRelativePrimeDenominators()
        {
            var x = new Fraction(1, 2);
            var y = new Fraction(1, 3);

            Assert.Equal(new Fraction(1, 6), x - y);
        }

        [Fact]
        public void CanAddFractions()
        {
            var x = new Fraction(1, 4);
            var y = new Fraction(1, 6);

            Assert.Equal(new Fraction(5, 12), x + y);
        }

        [Fact]
        public void CanSubtractFractions()
        {
            var x = new Fraction(1, 4);
            var y = new Fraction(1, 6);

            Assert.Equal(new Fraction(1, 12), x - y);
        }

        [Fact]
        public void CanMultiplyFractions()
        {
            var x = new Fraction(1, 2);
            var y = new Fraction(1, 3);

            Assert.Equal(new Fraction(1, 6), x * y);
        }

        [Fact]
        public void CanDivideFractions()
        {
            var x = new Fraction(1, 2);
            var y = new Fraction(1, 3);

            Assert.Equal(new Fraction(3, 2), x / y);
        }

        [Fact]
        public void CanCastFractionToInt()
        {
            var q = new Fraction(4, 2);

            Assert.Equal(2, (int)q);
        }

        [Fact]
        public void CanSimplifyFraction()
        {
            var x = new Fraction(2, 6);
            Assert.Equal(new Fraction(1, 3), x);
        }

        [Fact]
        public void CanDeconstructFraction()
        {
            var q = new Fraction(4, 3);
            var (n, d) = q;

            Assert.Equal(4, n);
            Assert.Equal(3, d);
        }
    }
}
