using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Text
{
    [TestClass]
    public class StringBuilderExtensionsTests
    {
        [TestMethod]
        public void CanPrependNullString() {
            string prefix = null;
            var sb = new StringBuilder("abc");
            sb.Prepend(prefix);
            Assert.AreEqual("abc", sb.ToString());
        }

        [TestMethod]
        public void CheckSurroundWith() {
            Assert.AreEqual("abc", new StringBuilder("b").SurroundWith("a", "c").ToString());
            Assert.AreEqual("bc", new StringBuilder("b").SurroundWith(null, "c").ToString());
            Assert.AreEqual("ab", new StringBuilder("b").SurroundWith("a", null).ToString());
            Assert.AreEqual("b", new StringBuilder("b").SurroundWith(null, null).ToString());
        }
    }
}
