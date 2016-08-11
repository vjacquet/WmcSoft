using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
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

        static char NthLowerASCIILetter(int n) {
            return (char)((short)'a' + n - 1);
        }

        [TestMethod]
        public void CheckExceptWith() {
            var vowels = VowelFrequencies;
            var dictionary = Enumerable.Range(1, 6).ToDictionary(NthLowerASCIILetter);
            dictionary.ExceptWith(vowels);
            Assert.AreEqual(4, dictionary.Count);
            Assert.IsFalse(dictionary.ContainsKey('a'));
            Assert.IsTrue(dictionary.ContainsKey('b'));
            Assert.IsTrue(dictionary.ContainsKey('c'));
            Assert.IsTrue(dictionary.ContainsKey('d'));
            Assert.IsFalse(dictionary.ContainsKey('e'));
            Assert.IsTrue(dictionary.ContainsKey('f'));
        }

        [TestMethod]
        public void CheckExceptWithWithPredicate() {
            var other = new Dictionary<char, int> {
                { 'a', 2 },
                { 'e', 5 },
            };
            var dictionary = Enumerable.Range(1, 6).ToDictionary(NthLowerASCIILetter);
            dictionary.ExceptWith(other, EqualityComparer<int>.Default);
            Assert.AreEqual(5, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey('a'));
            Assert.IsTrue(dictionary.ContainsKey('b'));
            Assert.IsTrue(dictionary.ContainsKey('c'));
            Assert.IsTrue(dictionary.ContainsKey('d'));
            Assert.IsFalse(dictionary.ContainsKey('e'));
            Assert.IsTrue(dictionary.ContainsKey('f'));
        }

        [TestMethod]
        public void CheckIntersectWith() {
            var vowels = VowelFrequencies;
            var dictionary = Enumerable.Range(1, 6).ToDictionary(NthLowerASCIILetter);
            dictionary.IntersectWith(vowels);
            Assert.AreEqual(2, dictionary.Count);
            Assert.AreEqual(1, dictionary['a']);
            Assert.AreEqual(5, dictionary['e']);
        }

        [TestMethod]
        public void CheckIntersectWithWithMerger() {
            var vowels = VowelFrequencies;
            var dictionary = Enumerable.Range(1, 6).ToDictionary(NthLowerASCIILetter);
            dictionary.IntersectWith(vowels, (x, y) => y);
            Assert.AreEqual(2, dictionary.Count);
            Assert.AreEqual(812, dictionary['a']);
            Assert.AreEqual(1202, dictionary['e']);
        }

        [TestMethod]
        public void CheckUnionWith() {
            var vowels = VowelFrequencies;
            var dictionary = Enumerable.Range(1, 6).ToDictionary(NthLowerASCIILetter);
            dictionary.UnionWith(vowels);
            Assert.AreEqual(9, dictionary.Count);
            Assert.AreEqual(2, dictionary['b']);
            Assert.AreEqual(812, dictionary['a']);
            Assert.AreEqual(1202, dictionary['e']);
            Assert.AreEqual(731, dictionary['i']);
            Assert.AreEqual(768, dictionary['o']);
            Assert.AreEqual(288, dictionary['u']);
        }

        [TestMethod]
        public void CheckUnionWithWithMerger() {
            var vowels = VowelFrequencies;
            var dictionary = Enumerable.Range(1, 6).ToDictionary(NthLowerASCIILetter);
            dictionary.UnionWith(vowels, (x, y) => x);
            Assert.AreEqual(9, dictionary.Count);
            Assert.AreEqual(2, dictionary['b']);
            Assert.AreEqual(1, dictionary['a']);
            Assert.AreEqual(5, dictionary['e']);
            Assert.AreEqual(731, dictionary['i']);
            Assert.AreEqual(768, dictionary['o']);
            Assert.AreEqual(288, dictionary['u']);
        }
    }
}