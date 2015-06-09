using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Algebra;

namespace WmcSoft.Arithmetics
{
    [TestClass]
    public class InterpolationTests
    {
        [TestMethod]
        public void CheckLinearInterpolation() {
            double[] x = { 1d, 2d, 3d };
            double[] y = { 2d, 4d, 6d };
            var i = new LinearInterpolation(x, y);
            var actual = i.Interpolate(1.5d);
            Assert.AreEqual(3d, actual);
        }

        [TestMethod]
        public void CheckLinearInterpolationWhenExact() {
            double[] x = { 1d, 2d, 3d };
            double[] y = { 2d, 4d, 6d };
            var i = new LinearInterpolation(x, y);
            var actual = i.Interpolate(2d);
            Assert.AreEqual(4d, actual);
        }
        
        [TestMethod]
        public void CheckLinearInterpolationWhenSmaller() {
            double[] x = { 1d, 2d, 3d };
            double[] y = { 2d, 4d, 6d };
            var i = new LinearInterpolation(x, y);
            var actual = i.Interpolate(0d);
            Assert.AreEqual(0d, actual);
        }

        [TestMethod]
        public void CheckLinearInterpolationWhenBigger() {
            double[] x = { 1d, 2d, 3d };
            double[] y = { 2d, 4d, 6d };
            var i = new LinearInterpolation(x, y);
            var actual = i.Interpolate(4d);
            Assert.AreEqual(8d, actual);
        }

    }
}
