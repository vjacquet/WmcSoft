using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic.Tests
{
    [TestClass]
    public class CollectionExtensionsTests
    {
        [TestMethod]
        public void CheckToArray() {
            var list = new List<Tuple<int, string>> {
                Tuple.Create(1, "a"),
                Tuple.Create(2, "b"),
                Tuple.Create(3, "c"),
            };
            var expected = new[] { 1, 2, 3 };
            var actual = list.ToArray(i => i.Item1);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckBinarySearch() {
            var list = new List<Tuple<int, string>> {
                Tuple.Create(2, "a"),
                Tuple.Create(4, "b"),
                Tuple.Create(6, "c"),
                Tuple.Create(8, "d"),
            };

            Assert.AreEqual(1, list.BinarySearch(t => Comparer.DefaultInvariant.Compare(t.Item1, 4)));
            Assert.AreEqual(2, ~list.BinarySearch(t => Comparer.DefaultInvariant.Compare(t.Item1, 5)));
        }

        [TestMethod]
        public void CheckToTwoDimensionalArray() {
            var list = new List<Tuple<int, int, int>>(2);
            list.Add(Tuple.Create(1, 2, 3));
            list.Add(Tuple.Create(4, 5, 6));

            var expected = new[,] {
               { 1, 2, 3},
               { 4, 5, 6},
            };

            var comparer = new ArrayEqualityComparer<int>();
            var actual = list.ToTwoDimensionalArray(i => i.Item1, i => i.Item2, i => i.Item3);

            Assert.IsTrue(comparer.Equals(expected, actual));
        }

        [TestMethod]
        public void CheckNotEqualsOnCell() {
            var x = new[, ,] {
               { {11, 12, 13}, {14, 15, 16} },
               { {21, 22, 23}, {24, 25, 26} },
               { {31, 32, 33}, {34, 35, 36} },
               { {41, 42, 43}, {44, 45, 46} },
            };
            var y = new[, ,] {
               { {11, 12, 13}, {14, 15, 16} },
               { {21, 22, 23}, {24, 25, 26} },
               { {31, 32, 33}, {99, 35, 36} },
               { {41, 42, 43}, {44, 45, 46} },
            };

            var comparer = new ArrayEqualityComparer<int>();
            Assert.IsFalse(comparer.Equals(x, y));
        }

        [TestMethod]
        public void CheckNotEqualsOnRank() {
            var x = new[, ,] {
               { {11, 12, 13}, {14, 15, 16} },
               { {21, 22, 23}, {24, 25, 26} },
               { {31, 32, 33}, {34, 35, 36} },
               { {41, 42, 43}, {44, 45, 46} },
            };
            var y = new[,] {
                {11, 12, 13}, 
                {21, 22, 23}, 
                {31, 32, 33}, 
                {41, 42, 43}, 
            };

            var comparer = new ArrayEqualityComparer<int>();
            Assert.IsFalse(comparer.Equals(x, y));
        }

        [TestMethod]
        public void CheckNotEqualsOnDimensions() {
            var x = new[, ,] {
               { {11, 12, 13}, {14, 15, 16} },
               { {21, 22, 23}, {24, 25, 26} },
               { {31, 32, 33}, {34, 35, 36} },
               { {41, 42, 43}, {44, 45, 46} },
            };
            var y = new[, ,] {
               { {11, 12, 13}, {14, 15, 16} },
               { {21, 22, 23}, {24, 25, 26} },
               { {31, 32, 33}, {34, 35, 36} },
            };

            var comparer = new ArrayEqualityComparer<int>();
            Assert.IsFalse(comparer.Equals(x, y));
        }

        [TestMethod]
        public void CheckSortOnIndexedCollection() {
            var array = new[] { "a", "c", "e", "d", "f", "b" };
            var comparer = new SourceComparer<string>(array, StringComparer.InvariantCulture);
            var actual = new[] { 0, 1, 2, 3, 4, 5 };
            Array.Sort(actual, comparer);
            var expected = new[] { 0, 5, 1, 3, 2, 4 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckSortedCombine() {
            var array = "aabbbc";
            var expected = "abc";
            var actual = new String(array.SortedCombine((char c, char g) => c).ToArray());
            Assert.AreEqual(expected, actual);
        }
    }
}
