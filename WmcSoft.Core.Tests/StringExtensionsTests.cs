using System;
using System.Globalization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Text;

namespace WmcSoft.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void CheckAnyOnChar() {
            var c = 'j';
            Assert.IsTrue(c.Any('i', 'j', 'k'));
            Assert.IsFalse(c.Any('a', 'b', 'c'));
        }

        [TestMethod]
        public void CheckBinaryAnyOnChar() {
            var c = 'j';
            Assert.IsTrue(c.Any('i', 'j', 'k'));
            Assert.IsFalse(c.Any('a', 'b', 'c'));
        }

        [TestMethod]
        public void CheckEqualsAnyOnString() {
            var c = "j";
            Assert.IsTrue(c.EqualsAny("i", "j", "k"));
            Assert.IsFalse(c.EqualsAny("a", "b", "c"));
        }

        [TestMethod]
        public void AssertToTitleCase() {
            var fr_fr = CultureInfo.GetCultureInfo("fr-FR");
            var title = "le bourgeois gentilhomme";
            var expected = "Le Bourgeois Gentilhomme";
            var actual = title.ToTitleCase(fr_fr);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanPadEnds() {
            var s = "abc";
            Assert.AreEqual(" abc  ", s.PadEnds(6));
        }

        [TestMethod]
        public void CanRemoveChars() {
            var s = " abc  def-ghi,jkl ";

            Assert.AreEqual("abcdefghijkl", s.Remove(' ', '-', ','));
        }

        [TestMethod]
        public void CheckSurroundWith() {
            string n = null;
            Assert.AreEqual("abc", "b".SurroundWith("a", "c"));
            Assert.AreEqual("bc", "b".SurroundWith(null, "c"));
            Assert.AreEqual("ab", "b".SurroundWith("a", null));
            Assert.AreEqual("b", "b".SurroundWith(null, null));
            Assert.IsNull(n.SurroundWith("a", "c"));
        }
    }
}
