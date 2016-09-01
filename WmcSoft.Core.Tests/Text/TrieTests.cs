using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Text
{
    [TestClass]
    public class TrieTests
    {
        [TestMethod]
        public void CanCreateTrie() {
            var trie = new Trie<char, int> {
                { "she", 0 },
                { "sells", 1 },
                { "sea", 2 },
                { "shells", 3 },
                { "by", 4 },
                { "the", 5 },
                { "shore", 7 },
            };

            Assert.AreEqual(3, trie["shells".AsReadOnlyList()]);
            Assert.AreEqual(0, trie["she".AsReadOnlyList()]);
            Assert.AreEqual(7, trie["shore".AsReadOnlyList()]);
        }

        [TestMethod]
        public void CanRemoveFromTrie() {
            var trie = new Trie<char, int> {
                { "she", 0 },
                { "sells", 1 },
                { "sea", 2 },
                { "shells", 3 },
                { "by", 4 },
                { "the", 5 },
                { "shore", 7 },
            };

            Assert.AreEqual(7, trie.Count);
            Assert.IsTrue(trie.ContainsKey("sells"));
            Assert.IsTrue(trie.ContainsKey("shells"));

            var removed = trie.Remove("sells");
            Assert.IsTrue(removed);
            Assert.AreEqual(6, trie.Count);
            Assert.IsFalse(trie.ContainsKey("sells"));
            Assert.IsTrue(trie.ContainsKey("shells"));
        }

        [TestMethod]
        public void CanGetKeysWithPrefix() {
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
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }

    public static class TrieExtensions
    {
        public static void Add<T>(this Trie<char, T> trie, string key, T value) {
            trie.Add(key.AsReadOnlyList(), value);
        }

        public static bool ContainsKey<T>(this Trie<char, T> trie, string key) {
            return trie.ContainsKey(key.AsReadOnlyList());
        }

        public static bool Remove<T>(this Trie<char, T> trie, string key) {
            return trie.Remove(key.AsReadOnlyList());
        }

        public static IEnumerable<string> GetKeysWithPrefix<T>(this Trie<char, T> trie, string key) {
            return trie.GetKeysWithPrefix(key.AsReadOnlyList())
                .Select(s => new string(s.ToArray()));
        }
    }
}
