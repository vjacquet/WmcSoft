using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
