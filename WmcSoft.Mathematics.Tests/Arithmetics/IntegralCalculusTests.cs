﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Arithmetics
{
    [TestClass]
    public class IntegralCalculusTests
    {
        [TestMethod]
        public void CanIntegrateLinearFunction() {
            var rule = new MidOrdinateRule(100);
            var f = new GenericFunction<double>(x => 1d / x);
            var actual = f.Integrate(rule, 1, 2);
            var expected = 0.6931440556283d;
            Assert.AreEqual(expected, actual, 0.00000000000001d);
        }
    }
}
