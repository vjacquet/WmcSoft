using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void CheckSurroundWithOnString() {
            string n = null;
            Assert.AreEqual("abc", "b".SurroundWith("a", "c"));
            Assert.AreEqual("bc", "b".SurroundWith(null, "c"));
            Assert.AreEqual("ab", "b".SurroundWith("a", null));
            Assert.AreEqual("b", "b".SurroundWith(null, null));
            Assert.IsNull(n.SurroundWith("a", "c"));
        }

        [TestMethod]
        public void CheckSurroundWithOnStringBuilder() {
            string n = null;
            Assert.AreEqual("abc", new StringBuilder("b").SurroundWith("a", "c").ToString());
            Assert.AreEqual("bc", new StringBuilder("b").SurroundWith(null, "c").ToString());
            Assert.AreEqual("ab", new StringBuilder("b").SurroundWith("a", null).ToString());
            Assert.AreEqual("b", new StringBuilder("b").SurroundWith(null, null).ToString());
        }
    }
}
