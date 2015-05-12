using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Tests
{
    [TestClass]
    public class Int32Tests
    {
        [TestMethod]
        public void CheckToInt32() {
            var digits = new[] { 1, 2, 3 };
            Assert.AreEqual(123, digits.ToInt32());
        }
    }
}
