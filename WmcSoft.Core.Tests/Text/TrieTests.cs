using System;
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
        }
    }

    public static class TrieExtensions
    {
        public static void Add<T>(this Trie<char, T> trie, string key, T value) {
            trie.Add(key.AsReadOnlyList(), value);
        }
    }
}
