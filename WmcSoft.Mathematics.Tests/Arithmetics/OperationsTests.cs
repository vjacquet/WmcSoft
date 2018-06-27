using System;
using Xunit;
using WmcSoft.Algebra;
using static WmcSoft.Arithmetics.Operations;

namespace WmcSoft.Arithmetics
{
    public class OperationsTests
    {
        [Fact]
        public void CheckDotProduct()
        {
            double[] x = { 1d, 3d, 5d };
            double[] y = { 2d, 4d, 6d };
            Assert.Equal(44d, DotProduct(x, y));
        }

        [Fact]
        public void CheckDotProductWithDifferentLengthVectors()
        {
            double[] x = { 1d };
            double[] y = { 2d, 4d };
            Assert.Equal(2d, DotProduct(x, y));
        }

        [Fact]
        public void CheckDotProductWithIntArithmetics()
        {
            int[] x = { 1, 3, 5 };
            int[] y = { 2, 4, 6 };
            var a = new Int32Arithmetics();
            var actual = a.DotProduct(x, y);
            Assert.IsType<int>(actual);
            Assert.Equal(44, actual);
        }

        [Fact]
        public void CheckPower()
        {
            var g = new Multiplies();
            var actual = Operations.Power(g, 10d, 5);
            var expected = 100000d;
            Assert.Equal(expected, actual);
        }
    }

    class Multiplies : IGroupLike<double>
    {
        #region IGroupLike<double> Members

        public double Identity {
            get { return 1d; }
        }

        public double Eval(double x, double y)
        {
            return x * y;
        }

        public double Inverse(double x)
        {
            return 1d / x;
        }

        #endregion

        #region IGroupLikeTraits Members

        public bool SupportIdentity {
            get { return true; }
        }

        public bool SupportInverse {
            get { return true; }
        }

        public bool IsAssociative {
            get { return true; }
        }

        public bool IsCommutative {
            get { return true; }
        }

        public bool IsIdempotent {
            get { return true; }
        }

        #endregion
    }
}
