using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Statistics
{
    [TestClass]
    public class AccumulatorTests
    {
        [TestMethod]
        public void CanGetPropertiesWhenEmpty() {
            var a = new Accumulator();
            Assert.AreEqual(0d, a.Mean);
            Assert.AreEqual(0d, a.Variance);
            Assert.AreEqual(0d, a.Sigma);
        }

        [TestMethod]
        public void CanGetPropertiesWhenAddedOne() {
            var a = new SamplesAccumulator(1d);
            Assert.AreEqual(1d, a.Mean);
            Assert.AreEqual(0d, a.Variance);
            Assert.AreEqual(0d, a.Sigma);
        }

        [TestMethod]
        public void CheckAccumulator() {
            var a = new Accumulator(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.AreEqual(5.5d, a.Mean);
            Assert.AreEqual(8.25d, a.Variance);
            Assert.AreEqual(2.872281323d, a.Sigma, 0.00000001d);
        }

        [TestMethod]
        public void CheckStraightForwardAccumulator() {
            var a = new StraightforwardAccumulator(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.AreEqual(5.5d, a.Mean);
            Assert.AreEqual(8.25d, a.Variance);
            Assert.AreEqual(2.872281323d, a.Sigma, 0.00000001d);
        }

        [TestMethod]
        public void CheckSamplesAccumulator() {
            var a = new SamplesAccumulator(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.AreEqual(5.5d, a.Mean);
            Assert.AreEqual(9.166666667d, a.Variance, 0.00000001d);
            Assert.AreEqual(3.027650354d, a.Sigma, 0.00000001d);
        }
    }
}