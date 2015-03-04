using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Arithmetics;

namespace WmcSoft.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void CanAdd() {
            var calc = new Int32Arithmetics();

            Assert.AreEqual(2, calc.Add(1, 1));
            Assert.AreEqual(0, calc.Add(1, -1));
            Assert.AreEqual(1, calc.Add(1, 0));
        }
    }

}
