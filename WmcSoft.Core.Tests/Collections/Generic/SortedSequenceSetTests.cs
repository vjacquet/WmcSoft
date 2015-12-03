using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class SortedSequenceSetTests
    {
        [TestMethod]
        public void CheckIntersectWith() {
            var actual = new SortedSequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SortedSequenceSet<char>();
            other.AddRange('b', 'd');
            actual.IntersectWith(other);

            var expected = new[] { 'b' };
            CollectionAssert.AreEqual(expected, actual.ToArray());
        }

        [TestMethod]
        public void CheckIntersectWithCollectionWithDuplicates() {
            var actual = new SortedSequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SortedSequence<char>();
            other.AddRange('b', 'b', 'd');
            actual.IntersectWith(other);

            var expected = new[] { 'b' };
            CollectionAssert.AreEqual(expected, actual.ToArray());
        }

        [TestMethod]
        public void CheckUnionWith() {
            var actual = new SortedSequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SortedSequenceSet<char>();
            other.AddRange('b', 'd');
            actual.UnionWith(other);

            var expected = new[] { 'a', 'b', 'c', 'd' };
            CollectionAssert.AreEqual(expected, actual.ToArray());
        }

        [TestMethod]
        public void CheckExceptWith() {
            var actual = new SortedSequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SortedSequenceSet<char>();
            other.AddRange('b', 'd');
            actual.ExceptWith(other);

            var expected = new[] { 'a', 'c' };
            CollectionAssert.AreEqual(expected, actual.ToArray());
        }

        [TestMethod]
        public void CheckSymmetricExceptWith() {
            var actual = new SortedSequenceSet<char>();
            actual.AddRange('a', 'b', 'c');
            var other = new SortedSequenceSet<char>();
            other.AddRange('b', 'd');
            actual.SymmetricExceptWith(other);

            var expected = new[] { 'a', 'c', 'd' };
            CollectionAssert.AreEqual(expected, actual.ToArray());
        }

        [TestMethod]
        public void CheckOverlaps() {
            var low = new SortedSequenceSet<int>(Enumerable.Range(1, 4));
            var all = new SortedSequenceSet<int>(Enumerable.Range(0, 10));

            Assert.IsTrue(low.Overlaps(all));
        }

        [TestMethod]
        public void CheckSubsetAndProperSubsetOnDifferentSets() {
            var set1 = new SortedSequenceSet<int>(Enumerable.Range(1, 4));
            var set2 = new SortedSequenceSet<int>(Enumerable.Range(0, 10));

            Assert.IsFalse(set1.SetEquals(set2));
            Assert.IsTrue(set1.IsSubsetOf(set2));
            Assert.IsTrue(set1.IsProperSubsetOf(set2));
            Assert.IsTrue(set2.IsSupersetOf(set1));
            Assert.IsTrue(set2.IsProperSupersetOf(set1));
        }

        [TestMethod]
        public void CheckSubsetAndProperSubsetOnEquivalentSets() {
            var set1 = new SortedSequenceSet<int>(Enumerable.Range(1, 4));
            var set2 = new SortedSequenceSet<int>(Enumerable.Range(1, 4));

            Assert.IsTrue(set1.SetEquals(set2));
            Assert.IsTrue(set1.IsSubsetOf(set2));
            Assert.IsFalse(set1.IsProperSubsetOf(set2));
            Assert.IsTrue(set2.IsSupersetOf(set1));
            Assert.IsFalse(set2.IsProperSupersetOf(set1));
        }
    }
}
