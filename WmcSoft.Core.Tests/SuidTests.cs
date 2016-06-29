using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class SuidTests
    {
        [TestMethod]
        public void CanParseStraightSuid() {
            Assert.IsTrue(Suid.IsValid("AAAAAAAAAAAAAAAAAAAAAA"));
            Assert.IsTrue(Suid.IsValid("AaBb0123456789-_AAAAAA"));
            Assert.IsFalse(Suid.IsValid("AAAAAAàAAAAAAAAAAAAAAA"));
            Assert.IsFalse(Suid.IsValid("AAA)AAAAAAAAAAAAAAAAAA"));
            Assert.IsFalse(Suid.IsValid("AAAAAAAA+AAAAAAAAAAAAA"));
        }

        [TestMethod]
        public void CanToString() {
            var empty = Suid.Empty;
            var actual = empty.ToString();
            var expected = "0000000000000000000000";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanCompareToEmpty() {
            var empty = Suid.Empty;

            var a = new Suid(new Guid("{FB6326AE-ED2C-43C3-8E14-A2DD490F6637}"));
            Assert.IsTrue(empty.CompareTo(a) < 0);

            var b = new Suid("AAAAAAAAAAAAAAAAAAAAAA");
            Assert.IsTrue(empty.CompareTo(b) < 0);
        }
    }
}
