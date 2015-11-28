using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class ConvertibleExtensionsTests
    {
        [TestMethod]
        public void CanConvertTo() {
            var expected = 5m;
            var n = 5;
            var actual = n.ConvertTo<decimal>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanConvertNullableTo() {
            decimal? expected = 5m;
            int? n = 5;
            var actual = n.ConvertTo<decimal?>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void CannotConvertNullToNonNullable() {
            int? n = null;
            var actual = n.ConvertTo<decimal>();
        }
    }
}
