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
            Strip b = null;
            Assert.AreEqual(1, Strip.Compare(a, b));
        }

        [TestMethod]
        public void CanTrim() {
            var data = new Strip("  abc   ");
            var expected = "abc";
            Assert.AreEqual<string>(expected, data.Trim());
        }

        [TestMethod]
        public void CanTrimEmpty() {
            var data = new Strip("");
            var expected = "";
            Assert.AreEqual<string>(expected, data.Trim());
        }

        [TestMethod]
        public void CanTrimWhitespaces() {
            var data = new Strip("    ");
            var expected = "";
            Assert.AreEqual<string>(expected, data.Trim());
        }

        [TestMethod]
        public void CanTrimChar() {
            var data = new Strip("    abc    ");
            var expected = "abc";
            Assert.AreEqual<string>(expected, data.Trim(' '));
        }

        [TestMethod]
        public void CanTrimChars() {
            var data = new Strip("  ..abc..  ");
            var expected = "abc";
            Assert.AreEqual<string>(expected, data.Trim('.', ' '));
        }

        [TestMethod]
        public void CanTrimStartWhitespaces() {
            var data = new Strip("   abc");
            var expected = "abc";
            Assert.AreEqual<string>(expected, data.TrimStart());
        }

        [TestMethod]
        public void CanTrimStartChar() {
            var data = new Strip("    abc");
            var expected = "abc";
            Assert.AreEqual<string>(expected, data.TrimStart(' '));
        }

        [TestMethod]
        public void CanTrimStartChars() {
            var data = new Strip("  ..abc");
            var expected = "abc";
            Assert.AreEqual<string>(expected, data.TrimStart('.', ' '));
        }

        [TestMethod]
        public void CanTrimEndWhitespaces() {
            var data = new Strip("abc   ");
            var expected = "abc";
            Assert.AreEqual<string>(expected, data.TrimEnd());
        }

        [TestMethod]
        public void CanTrimEndChar() {
            var data = new Strip("abc    ");
            var expected = "abc";
            Assert.AreEqual<string>(expected, data.TrimEnd(' '));
        }

        [TestMethod]
        public void CanTrimEndChars() {
            var data = new Strip("abc..  ");
            var expected = "abc";
            Assert.AreEqual<string>(expected, data.TrimEnd('.', ' '));
        }

        [TestMethod]
        public void CheckStartsWith() {
            var data = new Strip("abcdefghijklmnopqrstuvwxyz");
            Assert.IsTrue(data.StartsWith("abc"));
        }

        [TestMethod]
        public void CheckEndsWith() {
            var data = new Strip("abcdefghijklmnopqrstuvwxyz");
            Assert.IsTrue(data.EndsWith("xyz"));
        }

        [TestMethod]
        public void CheckIndexOf() {
            var data = "...abc...";
            var s = new Strip(data, 3, 3);
            Assert.AreEqual(1, s.IndexOf('b'));
        }
    }
}
