using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class OrdinalTests
    {
        [TestMethod]
        public void CheckInt32Ordinal() {
            var ordinal = new Int32Ordinal();

            Assert.AreEqual(5, ordinal.Advance(2, 3));
            Assert.AreEqual(3, ordinal.Distance(2, 5));
        }
    }
}
