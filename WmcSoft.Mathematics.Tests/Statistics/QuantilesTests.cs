using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Statistics.Tests
{
    [TestClass]
    public class QuantilesTests
    {
        [TestMethod]
        public void CanComputeQuartiles() {
            var q = new Quantiles<int>(3, 6, 7, 8, 8, 10, 13, 15, 16, 20);

            Assert.AreEqual(7, q[0.25]);
            Assert.AreEqual(8, q[0.50]);
            Assert.AreEqual(15, q[0.75]);
        }
    }
}
