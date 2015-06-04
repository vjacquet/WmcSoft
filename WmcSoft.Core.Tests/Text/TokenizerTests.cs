using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Text
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void CanTokenizeString() {
            var expected = new[] { "a", "b", "c" };
            var actual = "a b c".Tokenize().ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanTokenizeStringWithEmptyTokens() {
            var expected = new[] { "a", "b", "c" };
            var actual = " a  b c ".Tokenize().ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanTokenizeOnChar() {
            var expected = new[] { "a", "b", "c" };
            var actual = " a  b c ".Tokenize(' ').ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
