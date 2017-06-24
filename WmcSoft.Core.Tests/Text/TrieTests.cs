using System;
using System.Collections.Generic;
using System.Linq;
using WmcSoft.Collections.Generic;
using Xunit;

namespace WmcSoft.Text
{
    public class TrieTests
    {
        [Fact]
        public void CanCreateTrie()
        {
            var trie = new Trie<char, int> {
                { "she", 0 },
                { "sells", 1 },
                { "sea", 2 },
                { "shells", 3 },
                { "by", 4 },
                { "the", 5 },
                { "shore", 7 },
            };

            Assert.Equal(3, trie["shells".AsReadOnlyList()]);
            Assert.Equal(0, trie["she".AsReadOnlyList()]);
            Assert.Equal(7, trie["shore".AsReadOnlyList()]);
        }

        [Fact]
        public void CanRemoveFromTrie()
        {
            var trie = new Trie<char, int> {
                { "she", 0 },
                { "sells", 1 },
                { "sea", 2 },
                { "shells", 3 },
                { "by", 4 },
                { "the", 5 },
                { "shore", 7 },
            };

            Assert.Equal(7, trie.Count);
            Assert.True(trie.ContainsKey("sells"));
            Assert.True(trie.ContainsKey("shells"));

            var removed = trie.Remove("sells");
            Assert.True(removed);
            Assert.Equal(6, trie.Count);
            Assert.False(trie.ContainsKey("sells"));
            Assert.True(trie.ContainsKey("shells"));
        }

        [Fact]
        public void CanGetKeysWithPrefix()
        {
            var trie = new Trie<char, int> {
                { "she", 0 },
                { "sells", 1 },
                { "sea", 2 },
                { "shells", 3 },
                { "by", 4 },
                { "the", 5 },
                { "shore", 7 },
            };

            var actual = trie.GetKeysWithPrefix("sh").ToArray();
            var expected = new[] { "she", "shells", "shore" };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanMatch()
        {
            var trie = new Trie<char, int> {
                { "she", 0 },
                { "sells", 1 },
                { "sea", 2 },
                { "shells", 3 },
                { "by", 4 },
                { "the", 5 },
                { "shore", 7 },
            };

            var actual = trie.Match(".he").ToArray();
            var expected = new[] { "she", "the" };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEnumerateTrie()
        {
            var expected = new Dictionary<string, int>{
                { "she", 0 },
                { "sells", 1 },
                { "sea", 2 },
                { "shells", 3 },
                { "by", 4 },
                { "the", 5 },
                { "shore", 7 },
            };
            var trie = new Trie<char, int> {
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
        public static void Add<T>(this IDictionary<char[], T> trie, string key, T value)
        {
            trie.Add(key.ToCharArray(), value);
        }
        public static void Add<T>(this Trie<char, T> trie, string key, T value)
        {
            trie.Add(key.AsReadOnlyList(), value);
        }

        public static bool ContainsKey<T>(this Trie<char, T> trie, string key)
        {
            return trie.ContainsKey(key.AsReadOnlyList());
        }

        public static bool Remove<T>(this Trie<char, T> trie, string key)
        {
            return trie.Remove(key.AsReadOnlyList());
        }

        public static IEnumerable<string> GetKeysWithPrefix<T>(this Trie<char, T> trie, string key)
        {
            return trie.GetKeysWithPrefix(key.AsReadOnlyList())
                .Select(s => new string(s.ToArray()));
        }

        public static IEnumerable<string> Match<T>(this Trie<char, T> trie, string key)
        {
            return trie.Match(key.Select(c => c == '.' ? (char?)null : (char?)c).ToList())
                .Select(s => new string(s.ToArray()));
        }
    }
}
