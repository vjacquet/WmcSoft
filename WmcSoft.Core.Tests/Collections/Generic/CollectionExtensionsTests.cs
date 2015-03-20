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
        public void CheckToString() {
            var list = new List<int> { 1, 2, 3, };

            var actual = list.ToString("g");
            var expected = "1;2;3";
            Assert.AreEqual(expected, actual);
        }

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
        public void CheckCollateRepeat() {
            var array = new[] { "a", "b", "c" };
            var actual = String.Concat(array.Repeat(3));
            var expected = "abcabcabc";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckGroupedRepeat() {
            var array = new[] { "a", "b", "c" };
            var actual = String.Concat(array.Repeat(3, collate: false));
            var expected = "aaabbbccc";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckCollateRepeatToListOptimization() {
            const int Repeat = 3;
            var array = new[] { "a", "b", "c" };
            var actual = array.Repeat(Repeat, collate: false).ToList();
            var expected = new List<string>();
            for (int i = 0; i < Repeat; i++) {
                expected.AddRange(array);
            }
            Assert.AreNotEqual(expected.Capacity, actual.Capacity);
            Assert.AreEqual(expected.Count, actual.Capacity);
        }

        [TestMethod]
        public void CheckInterleave() {
            var letters = new[] { 'a', 'b', 'c' };
            var numbers = new[] { '1', '2' };
            var expected = "a1b2c";
            var actual = String.Concat(letters.Interleave(numbers));
            Assert.AreEqual(expected, actual);
        }
    }
}
