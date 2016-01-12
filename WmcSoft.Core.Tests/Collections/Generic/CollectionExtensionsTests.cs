using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
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

        [TestMethod]
        public void CheckBackwards() {
            var sequence = new[] { 1, 2, 3, 4 };
            var actual = sequence.Backwards().ToArray();
            var expected = new[] { 4, 3, 2, 1 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckConvertAll() {
            var sequence = new[] { 1, 2, 3, 4 };
            IList<int> list = new List<int>(sequence);
            var actual = list.ConvertAll(x => x * x);
            Assert.IsInstanceOfType(actual, typeof(IList<int>));
        }

        [TestMethod]
        public void CheckRemoveIfOnList() {
            var sequence = new List<int> { 1, 2, 3, 4, 5 };
            var count = sequence.RemoveIf(i => i % 2 == 1);
            Assert.AreEqual(3, count);
            var expected = new[] { 2, 4 };
            CollectionAssert.AreEqual(expected, sequence);
        }

        [TestMethod]
        public void CheckRemoveIfOnCollection() {
            var sequence = new SortedSet<int> { 1, 2, 3, 4, 5 };
            var count = sequence.RemoveIf(i => i % 2 == 1);
            Assert.AreEqual(3, count);
            var expected = new[] { 2, 4 };
            CollectionAssert.AreEqual(expected, sequence);
        }

        static Func<T, int> Find<T>(T value) where T : IComparable<T> {
            return x => -value.CompareTo(x);
        }

        [TestMethod]
        public void CheckBinaryFind() {
            var sequence = new[] { 1, 3, 5, 7 };
            var expected = 5;
            Assert.AreEqual(expected, sequence.BinaryFind(Find(5)));
        }

        [TestMethod]
        public void CheckLowerBound() {
            var sequence = new[] { 1, 3, 5, 7 };
            Assert.AreEqual(3, sequence.LowerBound(Find(4)));
            Assert.AreEqual(5, sequence.LowerBound(Find(5)));
            Assert.AreEqual(7, sequence.LowerBound(Find(9)));
            Assert.AreEqual(0, sequence.LowerBound(Find(-1)));
        }

        [TestMethod]
        public void CheckUpperBound() {
            var sequence = new[] { 1, 3, 5, 7 };
            Assert.AreEqual(5, sequence.UpperBound(Find(4)));
            Assert.AreEqual(5, sequence.UpperBound(Find(5)));
            Assert.AreEqual(0, sequence.UpperBound(Find(9)));
            Assert.AreEqual(1, sequence.UpperBound(Find(-1)));
        }

        [TestMethod]
        public void CheckBound() {
            var sequence = new[] { 1, 3, 5, 7 };
            Assert.AreEqual(Tuple.Create(3, 5), sequence.Bounds(Find(4)));
            Assert.AreEqual(Tuple.Create(5, 5), sequence.Bounds(Find(5)));
            Assert.AreEqual(Tuple.Create(7, 0), sequence.Bounds(Find(9)));
            Assert.AreEqual(Tuple.Create(0, 1), sequence.Bounds(Find(-1)));
        }

        [TestMethod]
        public void CheckInterpolatedSearch() {
            var sequence = new[] { 1, 2, 3, 5, 7, 9, 11, 12, 23, 42, 51 };
            Assert.AreEqual(4, sequence.InterpolatedSearch(7, new Int32Ordinal()));
        }

        [TestMethod]
        public void CheckMinElement() {
            var sequence = new[] { "dog", "cat", "bird", "lion" };
            Assert.AreEqual(2, sequence.MinElement());
        }

        [TestMethod]
        public void CheckMaxElement() {
            var sequence = new[] { "dog", "cat", "bird", "lion" };
            Assert.AreEqual(3, sequence.MaxElement());
        }

        [TestMethod]
        public void CheckMinMaxElement() {
            var sequence = new[] { "dog", "cat", "bird", "lion" };
            var actual = sequence.MinMaxElement();
            Assert.AreEqual(2, actual.Item1);
            Assert.AreEqual(3, actual.Item2);
        }

        [TestMethod]
        public void CanPop() {
            var actual = new List<string> { "one", "two", "three" };
            var expected = new[] { "one", "two" };
            Assert.AreEqual("three", actual.Pop());
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void CanPopAtIndex() {
            var actual = new List<string> { "one", "two", "three" };
            var expected = new[] { "one", "three" };
            Assert.AreEqual("two", actual.Pop(1));
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), AllowDerivedTypes = true)]
        public void CannotPopWhenIndexIsOutOfBound() {
            var actual = new List<string> { "one", "two", "three" };
            actual.Pop(4);
        }

        [TestMethod]
        public void CanToggle() {
            var actual = new List<string> { "one", "two", "three" };
            Assert.IsTrue(actual.Toggle("four"));
            Assert.IsFalse(actual.Toggle("one"));
            var expected = new[] { "two", "three", "four" };
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
