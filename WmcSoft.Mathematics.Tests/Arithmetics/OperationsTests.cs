using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Algebra;

namespace WmcSoft.Arithmetics
{
    [TestClass]
    public class OperationsTests
    {
        [TestMethod]
        public void CheckDotProduct() {
            double[] x = { 1d, 3d, 5d };
            double[] y = { 2d, 4d, 6d };
            Assert.AreEqual(44d, x.DotProduct(y));
        }

        [TestMethod]
        public void CheckDotProductWithDifferentLengthVectors() {
            double[] x = { 1d };
            double[] y = { 2d, 4d };
            Assert.AreEqual(2d, x.DotProduct(y));
        }

        [TestMethod]
        public void CheckDotProductWithIntArithmetics() {
            int[] x = { 1, 3, 5 };
            int[] y = { 2, 4, 6 };
            var a = new Int32Arithmetics();
            var actual = a.DotProduct(x, y);
            Assert.IsInstanceOfType(actual, typeof(Int32));
            Assert.AreEqual(44, actual);
        }

        [TestMethod]
        public void CheckPower() {
            var g = new Multiplies();
            var actual = Operations.Power(g, 10d, 5);
            var expected = 100000d;
            Assert.AreEqual(expected, actual);
        }
    }

    class Multiplies : IGroupLike<double>
    {

        #region IGroupLike<double> Members

        public double Identity {
            get { return 1d; }
        }

        public double Eval(double x, double y) {
            return x * y;
        }

        public double Inverse(double x) {
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
