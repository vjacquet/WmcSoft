using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class SymbolTests
    {
        [TestMethod]
        public void CanEqualsSymbols() {
            Symbol<int> x = "symbol1";
            Symbol<int> y = "symbol" + 1;

            Assert.AreEqual(x, y);
        }

        [TestMethod]
        public void CannotCompareDifferentSymbols() {
            Symbol<int> x = "symbol";
            Symbol<long> y = "symbol";

            Assert.AreNotEqual(x, y);
            Assert.AreEqual((string)x, (string)y);
        }
    }
}
