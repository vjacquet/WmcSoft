using System;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void CheckSubstringBetween() {
            Assert.AreEqual(null, "".SubstringBetween("q", "r"));
            Assert.AreEqual("a-b", "a[a-b]b".SubstringBetween("[", "]"));
            Assert.AreEqual(null, "a[a-b]b".SubstringBetween("]", "["));
            Assert.AreEqual("a-b", "a]a-b]b".SubstringBetween("]", "]"));
            Assert.AreEqual("", "a[]b".SubstringBetween("[", "]"));
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
            Assert.IsTrue(ReferenceEquals(data, result));
        }

        [TestMethod]
        public void CanReplaceWordInComplexPhrase() {
            var eq = "y=ax+b";
            Assert.AreEqual("y=ax+b", eq.ReplaceWord("a", "A"));
            Assert.AreEqual("y=ax+B", eq.ReplaceWord("b", "B"));
        }

        [TestMethod]
        public void CanRemovePrefix() {
            Assert.AreEqual("defghi", "abcdefghi".RemovePrefix("abc"));
            Assert.AreEqual("defghi", "abcdefghi".RemoveAffixes("abc"));
        }

        [TestMethod]
        public void CanRemoveSuffix() {
            Assert.AreEqual("abcdef", "abcdefghi".RemoveSuffix("ghi"));
            Assert.AreEqual("abcdef", "abcdefghi".RemoveAffixes("ghi"));
        }

        [TestMethod]
        public void CanRemoveAffixes() {
            Assert.AreEqual("def", "abcdefghi".RemoveAffixes("abc", "ghi"));
            Assert.AreEqual("b", "aabaa".RemoveAffixes("aa"));
        }

        [TestMethod]
        public void CanRemoveAffixesWhenOverlaps() {
            var expected = "";
            var actual = "aabaa".RemoveAffixes("aab", "baa");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanEnsurePrefix() {
            Assert.AreEqual("mytest", "test".EnsurePrefix("my"));
            Assert.AreEqual("mytest", "mytest".EnsurePrefix("my"));
            Assert.AreEqual("mytest", "test".EnsureAffixes("my", ""));
        }

        [TestMethod]
        public void CanEnsureSuffix() {
            Assert.AreEqual("test*", "test".EnsureSuffix("*"));
            Assert.AreEqual("test*", "test*".EnsureSuffix("*"));
            Assert.AreEqual("/test*", "test".EnsureAffixes("/", "*"));
        }

        [TestMethod]
        public void CheckGlob() {
            Assert.IsTrue("abaaba".Glob("a?aaba"));
            Assert.IsTrue("abaaba".Glob("ab*a"));
            Assert.IsTrue("abaaba".Glob("ab*ba"));
            Assert.IsTrue("abaaba".Glob("*aba"));
            Assert.IsFalse("abaaba".Glob("abaabac"));
        }

        [TestMethod]
        public void CheckAsWords() {
            CheckWords("abc", "abc");
            CheckWords("abc def", "abc", "def");
            CheckWords("abc def", "abc", "def");
            CheckWords("abc-def", "abc", "def");
            CheckWords("abc_def", "abc", "def");
            CheckWords("AbcDef", "Abc", "Def");
            CheckWords("ABcDefGHI", "ABc", "Def", "GHI");
            CheckWords("abcDefGHI", "abc", "Def", "GHI");
        }

        static void CheckWords(string sentence, params string[] words) {
            var actual = sentence.AsWords().ToArray();
            var expected = words;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckToCamelCase() {
            Assert.AreEqual("checkToCamelCase", "CheckToCamelCase".ToCamelCase());
        }

        [TestMethod]
        public void CheckToSnakeCase() {
            Assert.AreEqual("check_to_snake_case", "CheckToSnakeCase".ToSnakeCase());
        }

        [TestMethod]
        public void CheckToKebabCase() {
            Assert.AreEqual("check-to-kebab-case", "CheckToKebabCase".ToKebabCase());
        }

    }
}
