using System.Globalization;
using System.Linq;
using Xunit;

namespace WmcSoft
{
    public class StringExtensionsTests
    {
        [Fact]
        public void CheckAnyOnChar()
        {
            var c = 'j';
            Assert.True(c.Any('i', 'j', 'k'));
            Assert.False(c.Any('a', 'b', 'c'));
        }

        [Fact]
        public void CheckBinaryAnyOnChar()
        {
            var c = 'j';
            Assert.True(c.Any('i', 'j', 'k'));
            Assert.False(c.Any('a', 'b', 'c'));
        }

        [Fact]
        public void CheckEqualsAnyOnString()
        {
            var c = "j";
            Assert.True(c.EqualsAny("i", "j", "k"));
            Assert.False(c.EqualsAny("a", "b", "c"));
        }

        [Fact]
        public void AssertToTitleCase()
        {
            var fr_fr = CultureInfo.GetCultureInfo("fr-FR");
            var title = "le bourgeois gentilhomme";
            var expected = "Le Bourgeois Gentilhomme";
            var actual = title.ToTitleCase(fr_fr);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanPadEnds()
        {
            var s = "abc";
            Assert.Equal(" abc  ", s.PadEnds(6));
        }

        [Fact]
        public void CanRemoveChars()
        {
            var s = " abc  def-ghi,jkl ";

            Assert.Equal("abcdefghijkl", s.Remove(' ', '-', ','));
        }

        [Fact]
        public void CheckSubstringAfter()
        {
            Assert.Equal(null, "".SubstringAfter("q"));
            Assert.Equal("aa-bb", "aa-bb".SubstringAfter(""));
            Assert.Equal("bb", "aa-bb".SubstringAfter("-"));
            Assert.Equal("a-bb", "aa-bb".SubstringAfter("a"));
            Assert.Equal("b", "aa-bb".SubstringAfter("b"));
            Assert.Equal(null, "aa-bb".SubstringAfter("q"));
            Assert.Equal("z", "aa-bb".SubstringAfter("q") ?? "z");
        }

        [Fact]
        public void CheckSubstringBefore()
        {
            Assert.Equal(null, "".SubstringBefore("q"));
            Assert.Equal("aa-bb", "aa-bb".SubstringBefore(""));
            Assert.Equal("aa", "aa-bb".SubstringBefore("-"));
            Assert.Equal("", "aa-bb".SubstringBefore("a"));
            Assert.Equal("aa-", "aa-bb".SubstringBefore("b"));
            Assert.Equal(null, "aa-bb".SubstringBefore("q"));
        }

        [Fact]
        public void CheckSubstringBetween()
        {
            Assert.Equal(null, "".SubstringBetween("q", "r"));
            Assert.Equal("a-b", "a[a-b]b".SubstringBetween("[", "]"));
            Assert.Equal(null, "a[a-b]b".SubstringBetween("]", "["));
            Assert.Equal("a-b", "a]a-b]b".SubstringBetween("]", "]"));
            Assert.Equal("", "a[]b".SubstringBetween("[", "]"));
        }

        [Fact]
        public void CheckLeft()
        {
            Assert.Equal("abc", "abcdef".Left(3));
            Assert.Equal("abc", "abc".Left(5));
        }

        [Fact]
        public void CheckRight()
        {
            Assert.Equal("def", "abcdef".Right(3));
            Assert.Equal("abc", "abc".Right(5));
        }

        [Fact]
        public void CheckSurroundWith()
        {
            string n = null;
            Assert.Equal("abc", "b".SurroundWith("a", "c"));
            Assert.Equal("bc", "b".SurroundWith(null, "c"));
            Assert.Equal("ab", "b".SurroundWith("a", null));
            Assert.Equal("b", "b".SurroundWith(null, null));
            Assert.Null(n.SurroundWith("a", "c"));
        }

        [Fact]
        public void CheckTruncate()
        {
            Assert.Equal("abc\u2026", "abcdef".Truncate(4));
            Assert.Equal("abc", "abc".Truncate(4));
        }

        [Fact]
        public void CanReplaceWord()
        {
            const string expected = "You do not buy a dog in a catalog.";
            var phrase = "You do not buy a cat in a catalog.";
            var actual = phrase.ReplaceWord("cat", "dog");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanReplaceWordPreserveReferenceWhenNotFound()
        {
            var data = "abd def ghi jkl mno pqr";
            var result = data.ReplaceWord("xyz", "stu");
            Assert.Equal(data, result);
            Assert.True(ReferenceEquals(data, result));
        }

        [Fact]
        public void CanReplaceWordInComplexPhrase()
        {
            var eq = "y=ax+b";
            Assert.Equal("y=ax+b", eq.ReplaceWord("a", "A"));
            Assert.Equal("y=ax+B", eq.ReplaceWord("b", "B"));
        }

        [Fact]
        public void CanRemovePrefix()
        {
            Assert.Equal("defghi", "abcdefghi".RemovePrefix("abc"));
            Assert.Equal("defghi", "abcdefghi".RemoveAffixes("abc"));
        }

        [Fact]
        public void CanRemoveSuffix()
        {
            Assert.Equal("abcdef", "abcdefghi".RemoveSuffix("ghi"));
            Assert.Equal("abcdef", "abcdefghi".RemoveAffixes("ghi"));
        }

        [Fact]
        public void CanRemoveAffixes()
        {
            Assert.Equal("def", "abcdefghi".RemoveAffixes("abc", "ghi"));
            Assert.Equal("b", "aabaa".RemoveAffixes("aa"));
        }

        [Fact]
        public void CanRemoveAffixesWhenOverlaps()
        {
            var expected = "";
            var actual = "aabaa".RemoveAffixes("aab", "baa");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEnsurePrefix()
        {
            Assert.Equal("mytest", "test".EnsurePrefix("my"));
            Assert.Equal("mytest", "mytest".EnsurePrefix("my"));
            Assert.Equal("mytest", "test".EnsureAffixes("my", ""));
        }

        [Fact]
        public void CanEnsureSuffix()
        {
            Assert.Equal("test*", "test".EnsureSuffix("*"));
            Assert.Equal("test*", "test*".EnsureSuffix("*"));
            Assert.Equal("/test*", "test".EnsureAffixes("/", "*"));
        }

        [Fact]
        public void CheckGlob()
        {
            Assert.True("abaaba".Glob("a?aaba"));
            Assert.True("abaaba".Glob("ab*a"));
            Assert.True("abaaba".Glob("ab*ba"));
            Assert.True("abaaba".Glob("*aba"));
            Assert.False("abaaba".Glob("abaabac"));
        }

        [Theory]
        [InlineData("abc", new[] { "abc" })]
        [InlineData("ABC", new[] { "ABC" })]
        [InlineData("abc def", new[] { "abc", "def" })]
        [InlineData("abc-def", new[] { "abc", "def" })]
        [InlineData("abc_def", new[] { "abc", "def" })]
        [InlineData("AbcDef", new[] { "Abc", "Def" })]
        [InlineData("ABcDefGHI", new[] { "A", "Bc", "Def", "GHI" })]
        [InlineData("abcDefGHI", new[] { "abc", "Def", "GHI" })]
        public void CheckAsWords(string sentence, string[] words)
        {
            var actual = sentence.AsWords().ToArray();
            var expected = words;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("CheckToCamelCase", "checkToCamelCase")]
        [InlineData("ISOLetter", "isoLetter")]
        [InlineData("ISO", "iso")]
        [InlineData("ALetter", "aLetter")]
        public void CheckToCamelCase(string source, string expected)
        {
            Assert.Equal(expected, source.ToCamelCase());
        }

        [Theory]
        [InlineData("CheckToSnakeCase", "check_to_snake_case")]
        [InlineData("ISOLetter", "iso_letter")]
        public void CheckToSnakeCase(string source, string expected)
        {
            Assert.Equal(expected, source.ToSnakeCase());
        }

        [Theory]
        [InlineData("CheckToKebabCase", "check-to-kebab-case")]
        [InlineData("ISOLetter", "iso-letter")]
        public void CheckToKebabCase(string source, string expected)
        {
            Assert.Equal(expected, source.ToKebabCase());
        }

        [Theory]
        [InlineData("may", "june", false)]
        [InlineData("june", "june", true)]
        [InlineData("may|june", "june", true)]
        [InlineData("may|june|july", "june", true)]
        [InlineData("june|july", "june", true)]
        [InlineData("may|july", "june", false)]
        public void CheckContainsWord(string value, string word, bool expected)
        {
            Assert.Equal(expected, value.ContainsWord(word, '|'));
        }
    }
}
