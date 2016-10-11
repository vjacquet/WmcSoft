using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    /// <summary>
    /// Summary description for DecimalExtensionsTests
    /// </summary>
    [TestClass]
    public class DecimalExtensionsTests
    {
        [TestMethod]
        public void CanGetScale() {
            Assert.AreEqual(0, 1m.Scale());
            Assert.AreEqual(4, 1.2345m.Scale());
            Assert.AreEqual(8, 1.23456789m.Scale());
            Assert.AreEqual(3, 12.345m.Scale());
            Assert.AreEqual(1, 12.3456E+3m.Scale());
        }
    }
}
