using System.Collections.Generic;
using Xunit;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Numerics.Tests
{
    public class BigFractionTests
    {
        [Fact]
        public void CanAddWithRelativePrimeDenominators()
        {
            var x = new BigFraction(1, 2);
            var y = new BigFraction(1, 3);

            Assert.Equal(new BigFraction(5, 6), x + y);
        }

        [Fact]
        public void CanCompareBigFractions()
        {
            var data = new[] {
                new BigFraction(-3,2),
                new BigFraction(-1,2),
                BigFraction.Zero,
                new BigFraction(1,2),
                new BigFraction(3,2),
            };

            Assert.True(data.IsSorted());
            Assert.True(data.Backwards().IsSorted(Comparer<BigFraction>.Default.Reverse()));
        }

        [Fact]
        public void CanSubtractRelativePrimeDenominators()
        {
            var x = new BigFraction(1, 2);
            var y = new BigFraction(1, 3);

            Assert.Equal(new BigFraction(1, 6), x - y);
        }

        [Fact]
        public void CanAddFractions()
        {
            var x = new BigFraction(1, 4);
            var y = new BigFraction(1, 6);

            Assert.Equal(new BigFraction(5, 12), x + y);
        }

        [Fact]
        public void CanSubtractFractions()
        {
            var x = new BigFraction(1, 4);
            var y = new BigFraction(1, 6);

            Assert.Equal(new BigFraction(1, 12), x - y);
        }

        [Fact]
        public void CanMultiplyFractions()
        {
            var x = new BigFraction(1, 2);
            var y = new BigFraction(1, 3);

            Assert.Equal(new BigFraction(1, 6), x * y);
        }

        [Fact]
        public void CanDivideFractions()
        {
            var x = new BigFraction(1, 2);
            var y = new BigFraction(1, 3);

            Assert.Equal(new BigFraction(3, 2), x / y);
        }

        [Fact]
        public void CanSimplifyFraction()
        {
            var x = new BigFraction(2, 6);
            Assert.Equal(new BigFraction(1, 3), x);
        }

        [Fact]
        public void CanDeconstructBigFraction()
        {
            var q = new BigFraction(4, 3);
            var (n, d) = q;

            Assert.Equal(4, n);
            Assert.Equal(3, d);
        }
    }
}