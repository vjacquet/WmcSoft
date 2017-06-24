using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class DictionaryExtensionsTests
    {
        // <http://www.math.cornell.edu/~mec/2003-2004/cryptography/subs/frequencies.html>
        static readonly Dictionary<char, int> VowelFrequencies = new Dictionary<char, int>{
            {'a', 812},
            {'e', 1202},
            {'i', 731},
            {'o', 768},
            {'u', 288},
        };

        static char NthLowerASCIILetter(int n)
        {
            return (char)((short)'a' + n - 1);
        }

        [Fact]
        public void CheckExceptWith()
        {
            var vowels = VowelFrequencies;
            var dictionary = Enumerable.Range(1, 6).ToDictionary(NthLowerASCIILetter);
            dictionary.ExceptWith(vowels);
            Assert.Equal(4, dictionary.Count);
            Assert.False(dictionary.ContainsKey('a'));
            Assert.True(dictionary.ContainsKey('b'));
            Assert.True(dictionary.ContainsKey('c'));
            Assert.True(dictionary.ContainsKey('d'));
            Assert.False(dictionary.ContainsKey('e'));
            Assert.True(dictionary.ContainsKey('f'));
        }

        [Fact]
        public void CheckExceptWithWithPredicate()
        {
            var other = new Dictionary<char, int> {
                { 'a', 2 },
                { 'e', 5 },
            };
            var dictionary = Enumerable.Range(1, 6).ToDictionary(NthLowerASCIILetter);
            dictionary.ExceptWith(other, EqualityComparer<int>.Default);
            Assert.Equal(5, dictionary.Count);
            Assert.True(dictionary.ContainsKey('a'));
            Assert.True(dictionary.ContainsKey('b'));
            Assert.True(dictionary.ContainsKey('c'));
            Assert.True(dictionary.ContainsKey('d'));
            Assert.False(dictionary.ContainsKey('e'));
            Assert.True(dictionary.ContainsKey('f'));
        }

        [Fact]
        public void CheckIntersectWith()
        {
            var vowels = VowelFrequencies;
            var dictionary = Enumerable.Range(1, 6).ToDictionary(NthLowerASCIILetter);
            dictionary.IntersectWith(vowels);
            Assert.Equal(2, dictionary.Count);
            Assert.Equal(1, dictionary['a']);
            Assert.Equal(5, dictionary['e']);
        }

        [Fact]
        public void CheckIntersectWithWithMerger()
        {
            var vowels = VowelFrequencies;
            var dictionary = Enumerable.Range(1, 6).ToDictionary(NthLowerASCIILetter);
            dictionary.IntersectWith(vowels, (x, y) => y);
            Assert.Equal(2, dictionary.Count);
            Assert.Equal(812, dictionary['a']);
            Assert.Equal(1202, dictionary['e']);
        }

        [Fact]
        public void CheckUnionWith()
        {
            var vowels = VowelFrequencies;
            var dictionary = Enumerable.Range(1, 6).ToDictionary(NthLowerASCIILetter);
            dictionary.UnionWith(vowels);
            Assert.Equal(9, dictionary.Count);
            Assert.Equal(2, dictionary['b']);
            Assert.Equal(812, dictionary['a']);
            Assert.Equal(1202, dictionary['e']);
            Assert.Equal(731, dictionary['i']);
            Assert.Equal(768, dictionary['o']);
            Assert.Equal(288, dictionary['u']);
        }

        [Fact]
        public void CheckUnionWithWithMerger()
        {
            var vowels = VowelFrequencies;
            var dictionary = Enumerable.Range(1, 6).ToDictionary(NthLowerASCIILetter);
            dictionary.UnionWith(vowels, (x, y) => x);
            Assert.Equal(9, dictionary.Count);
            Assert.Equal(2, dictionary['b']);
            Assert.Equal(1, dictionary['a']);
            Assert.Equal(5, dictionary['e']);
            Assert.Equal(731, dictionary['i']);
            Assert.Equal(768, dictionary['o']);
            Assert.Equal(288, dictionary['u']);
        }

        [Fact]
        public void CheckKeySymmetricDifferences()
        {
            var x = new Dictionary<int, string> {
                { 0, "zero" },
                { 2, "two" },
                { 3, "three" },
                { 5, "five" },
            };
            var y = new Dictionary<int, string> {
                { 0, "naught" },
                { 1, "one" },
                { 2, "two" },
                { 4, "four" },
                { 5, "five" },
            };
            var actual = x.KeySymmetricDifferences(y);
            var expected = new[] { 0, 1, 3, 4 };
            Assert.True(expected.CollectionEquivalent(actual));
        }

        [Fact]
        public void CanRenameExistingKey()
        {
            var dictionary = new Dictionary<int, string> {
                { 1, "zero" },
                { 2, "two" },
                { 3, "three" },
                { 5, "five" },
            };

            var result = dictionary.RenameKey(1, 0);
            Assert.True(result);
            Assert.True(dictionary.ContainsKey(0));
            Assert.False(dictionary.ContainsKey(1));
            Assert.Equal("zero", dictionary[0]);
        }

        [Fact]
        public void CheckRenamingNonExistingKeyReturnsFalse()
        {
            var dictionary = new Dictionary<int, string> {
                { 0, "zero" },
                { 2, "two" },
                { 3, "three" },
                { 5, "five" },
            };
            var result = dictionary.RenameKey(1, 0);
            Assert.False(result);
            Assert.True(dictionary.ContainsKey(0));
            Assert.False(dictionary.ContainsKey(1));
        }

        [Fact]
        public void CheckConflictingKeyRenamesThrows()
        {
            var dictionary = new Dictionary<int, string> {
                { 0, "zero" },
                { 2, "two" },
                { 3, "three" },
                { 5, "five" },
            };
            Assert.Throws<ArgumentException>(() => dictionary.RenameKey(0, 2));
        }
    }
}