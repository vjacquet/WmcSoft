using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Text
{
    [TestClass]
    public class StripTests
    {
        [TestMethod]
        public void CanCompareIgnoreCase() {
            var a = new Strip("abcdef", 1, 3);
            var b = new Strip("aBCdef", 1, 3);
            Assert.AreEqual(0, Strip.Compare(a, b, true));
        }

        [TestMethod]
        public void CanCompareWithNull() {
            var a = new Strip("abcdef", 1, 3);
            var b = new Strip(null, 0, 0);
            Assert.AreEqual(1, Strip.Compare(a, b));
        }
    }
}
