using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class SuidTests
    {
        static bool IsValid(string input) {
            Suid dummy;
            return Suid.TryParseExact(input, "S", out dummy);
        }

        [TestMethod]
        public void CanParseStraightSuid() {
            Assert.IsTrue(IsValid("AAAAAAAAAAAAAAAAAAAAA0"));
            Assert.IsTrue(IsValid("AaBb0123456789-_AAAAA0"));
            Assert.IsFalse(IsValid("AAAAAAàAAAAAAAAAAAAAA0"));
            Assert.IsFalse(IsValid("AAA)AAAAAAAAAAAAAAAAA0"));
            Assert.IsFalse(IsValid("AAAAAAAA+AAAAAAAAAAAA0"));
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

            var b = new Suid("AAAAAAAAAAAAAAAAAAAAA0");
            Assert.IsTrue(empty.CompareTo(b) < 0);

            var c = new Suid("---------------------G");
            Assert.IsTrue(empty.CompareTo(c) < 0);
        }

        [TestMethod]
        public void CanRoundTrip() {
            var data = new string[] { "AAAAAAAAAAAAAAAAAAAAA0", "AazZ09-_ko1664OKmNoPq0" };
            foreach (var expected in data) {
                var actual = new Suid(expected).ToString();
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void CanRoundTripOnAllLastByteValue() {
            unchecked {
                var bytes = new byte[16];
                for (int i = 0; i < 256; i++) {
                    bytes[15] = (byte)i;
                    var s = new Suid(bytes);
                    var expected = s.ToString();
                    var actual = new Suid(expected).ToString();
                    Assert.AreEqual(expected, actual);
                }
            }
        }
    }
}
