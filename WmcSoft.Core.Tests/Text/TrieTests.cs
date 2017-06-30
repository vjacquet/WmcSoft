using System.Collections.Generic;
using System.Linq;
using Xunit;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Text
{
    public class TrieTests
    {
        Trie<char, int> GetTrie()
        {
            return new Trie<char, int> {
                { "she", 0 },
                { "sells", 1 },
                { "sea", 2 },
                { "shells", 3 },
                { "by", 4 },
                { "the", 5 },
                { "shore", 7 },
            };
        }

        [Fact]
        public void CanCreateTrie()
        {
            var trie = GetTrie();

            Assert.Equal(3, trie["shells".AsReadOnlyList()]);
            Assert.Equal(0, trie["she".AsReadOnlyList()]);
            Assert.Equal(7, trie["shore".AsReadOnlyList()]);
        }

        [Fact]
        public void CanRemoveFromTrie()
        {
            var trie = GetTrie();

            Assert.Equal(7, trie.Count);
            Assert.True(trie.ContainsKey("sells"));
            Assert.True(trie.ContainsKey("shells"));

            var removed = trie.Remove("sells");
            Assert.True(removed);
            Assert.Equal(6, trie.Count);
            Assert.False(trie.ContainsKey("sells"));
            Assert.True(trie.ContainsKey("shells"));
        }

        [Theory]
        [InlineData("sh", new[] { "she", "shells", "shore" })]
        [InlineData("car", new string[0])]
        public void CanGetKeysWithPrefix(string prefix, string[] expected)
        {
            var trie = GetTrie();

            var actual = trie.GetKeysWithPrefix(prefix).ToArray();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("car", -1)]
        [InlineData("s", -1)]
        [InlineData("she", 3)]
        [InlineData("theme", 3)]
        [InlineData("shoe", -1)]
        public void CanGetLengthLongestPrefixOf(string prefix, int expected)
        {
            var trie = GetTrie();

            var actual = trie.GetLengthLongestPrefixOf(prefix);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(".he", new[] { "she", "the" })]
        [InlineData("s....", new[] { "sells", "shore" })]
        public void CanMatch(string prefix, string[] expected)
        {
            var trie = GetTrie();

            var actual = trie.Match(prefix).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEnumerateTrie()
        {
            var trie = GetTrie();
            var expected = new Dictionary<string, int>{
                { "she", 0 },
                { "sells", 1 },
                { "sea", 2 },
                { "shells", 3 },
                { "by", 4 },
                { "the", 5 },
                { "shore", 7 },
            };
            var actual = trie.Select(p => new KeyValuePair<string, int>(new string(p.Key.ToArray()), p.Value));
            Assert.True(expected.Equivalent(actual));
        }
    }

    public static class TrieExtensions
    {
        public static IEnumerable<string> Match<T>(this Trie<char, T> trie, string key)
        {
            return trie.Match(key.Select(c => c == '.' ? (char?)null : (char?)c).ToList())
                .Select(s => new string(s.ToArray()));
        }
    }
}