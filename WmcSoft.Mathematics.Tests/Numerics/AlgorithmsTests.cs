using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Numerics
{
    [TestClass]
    public class AlgorithmsTests
    {
        [TestMethod]
        public void CanFindGreatestCommonDivisor() {
            var gcd = Algorithms.GreatestCommonDivisor(14, 42, 21);
            Assert.AreEqual(7, gcd);
        }
    }
}
