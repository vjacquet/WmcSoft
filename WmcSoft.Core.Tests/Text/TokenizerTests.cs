using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Text.Tests
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void CanTokenizeString() {
            var expected = new[] { "a", "b", "c" };
            var tokenizer = new Tokenizer();
            var actual = tokenizer.Tokenize("a b c").ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanTokenizeStringWithEmptyTokens() {
            var expected = new[] { "a", "b", "c" };
            var tokenizer = new Tokenizer();
            var actual = tokenizer.Tokenize(" a  b c ").ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
