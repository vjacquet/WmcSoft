using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        public void CheckToString() {
            var list = new List<int> { 1, 2, 3, };

            var actual = list.ToString("g");
            var expected = "1;2;3";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckQuorum() {
            var list = new[] { 1, 2, 3, 4, 5, 6, 7 };
            Assert.IsTrue(list.Quorum(3, i => (i & 1) == 1));
        }

        [TestMethod]
        public void CheckElectedOrDefaultWithPredicate() {
            string[] array;

            array = new[] { "A", "B", null, "", "A", "C", "B", "A" };
            Assert.AreEqual("A", array.ElectedOrDefault(s => !String.IsNullOrEmpty(s)));

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B", "B" };
            Assert.AreEqual("B", array.ElectedOrDefault(s => !String.IsNullOrEmpty(s)));

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B" };
            Assert.AreEqual("A", array.ElectedOrDefault(s => !String.IsNullOrEmpty(s)));
        }

        [TestMethod]
        public void CheckElectedOrDefaultWithoutPredicate() {
            string[] array;

            array = new[] { "A", "B", null, "", "A", "C", "B", "A" };
            Assert.AreEqual("A", array.ElectedOrDefault());

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B", "B" };
            Assert.AreEqual("B", array.ElectedOrDefault());

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B" };
            Assert.AreEqual("A", array.ElectedOrDefault());
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
        public void CheckInterleavesWithIdenticalCount() {
            char[] s1 = { 'a', 'b', 'c'};
            char[] s2 = { 'M', 'N', 'P' };
            char[] s3 = { '1', '2', '3' };
            char[][] s = { s1, s2, s3 };
            var actual = new String(s.Interleave().ToArray(s.Sum(i=>i.Length)));
            Assert.AreEqual("aM1bN2cP3", actual);
        }

        [TestMethod]
        public void CheckInterleaves() {
            char[] s1 = { 'a', 'b', 'c', 'd', 'e' };
            char[] s2 = { 'M', 'N', 'P' };
            char[] s3 = { '1', '2', '3', '4' };
            char[][] s = { s1, s2, s3 };
            var actual = new String(s.Interleave().ToArray(s.Sum(i => i.Length)));
            Assert.AreEqual("aM1bN2cP3d4e", actual);
        }

        [TestMethod]
        public void CheckTail() {
            var collection = new[] { 1, 2, 3, 4, 5 };
            var expected = new[] { 3, 4, 5 };
            var actual = collection.Tail(3).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckTailWhenCollectionHasLessElements() {
            var expected = new[] { 1, 2, 3, 4, 5 };
            var actual = expected.Tail(6).ToArray(); ;
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
