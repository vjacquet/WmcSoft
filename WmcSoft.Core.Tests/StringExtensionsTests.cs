using System;
using System.Globalization;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Text;

namespace WmcSoft
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
        public void CheckSubstringAfter() {
            Assert.AreEqual(null, "".SubstringAfter("q"));
            Assert.AreEqual("aa-bb", "aa-bb".SubstringAfter(""));
            Assert.AreEqual("bb", "aa-bb".SubstringAfter("-"));
            Assert.AreEqual("a-bb", "aa-bb".SubstringAfter("a"));
            Assert.AreEqual("b", "aa-bb".SubstringAfter("b"));
            Assert.AreEqual(null, "aa-bb".SubstringAfter("q"));
            Assert.AreEqual("z", "aa-bb".SubstringAfter("q") ?? "z");
        }

        [TestMethod]
        public void CheckSubstringBefore() {
            Assert.AreEqual(null, "".SubstringBefore("q"));
            Assert.AreEqual("aa-bb", "aa-bb".SubstringBefore(""));
            Assert.AreEqual("aa", "aa-bb".SubstringBefore("-"));
            Assert.AreEqual("", "aa-bb".SubstringBefore("a"));
            Assert.AreEqual("aa-", "aa-bb".SubstringBefore("b"));
            Assert.AreEqual(null, "aa-bb".SubstringBefore("q"));
        }

        [TestMethod]
        public void CheckLeft() {
            Assert.AreEqual("abc", "abcdef".Left(3));
            Assert.AreEqual("abc", "abc".Left(5));
        }

        [TestMethod]
        public void CheckRight() {
            Assert.AreEqual("def", "abcdef".Right(3));
            Assert.AreEqual("abc", "abc".Right(5));
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

        [TestMethod]
        public void CheckTruncate() {
            Assert.AreEqual("abc\u2026", "abcdef".Truncate(4));
            Assert.AreEqual("abc", "abc".Truncate(4));
        }

        [TestMethod]
        public void CanReplaceWord() {
            const string expected = "You do not buy a dog in a catalog.";
            var phrase = "You do not buy a cat in a catalog.";
            var actual = phrase.ReplaceWord("cat", "dog");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanReplaceWordPreserveReferenceWhenNotFound() {
            var data = "abd def ghi jkl mno pqr";
            var result = data.ReplaceWord("xyz", "stu");
            Assert.AreEqual(data, result);
            Assert.IsTrue(Object.ReferenceEquals(data, result));
        }

        [TestMethod]
        public void CanReplaceWordInComplexPhrase() {
            var eq = "y=ax+b";
            Assert.AreEqual("y=ax+b", eq.ReplaceWord("a", "A"));
            Assert.AreEqual("y=ax+B", eq.ReplaceWord("b", "B"));
        }
    }
}
