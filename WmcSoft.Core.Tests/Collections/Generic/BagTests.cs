using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class BagTests
    {
        [TestMethod]
        public void CanAddToBag() {
            var bag = new Bag<int>() { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(6, bag.Count);
            Assert.IsFalse(bag.Contains(0));
            Assert.IsFalse(bag.Contains(8));
            Assert.IsTrue(bag.Contains(2));
        }

        [TestMethod]
        public void CanRemoveFromBag() {
            var bag = new Bag<int>() { 1, 2, 3, 4, 5, 6 };
            Assert.AreEqual(6, bag.Count);

            Assert.IsTrue(bag.Remove(4));
            Assert.IsFalse(bag.Remove(0));
            Assert.AreEqual(5, bag.Count);
        }

        [TestMethod]
        public void CanRemoveAllFromBag() {
            var bag = new Bag<int>() { 8, 1, 2, 7, 3, 4, 5, 6 };

            var expected = new[] { 1, 2, 3 };
            var removed = bag.RemoveAll(i => i >= 4);
            var actual = bag.ToArray();
            Assert.AreEqual(5, removed);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void CanEnumerateBag() {
            var bag = new Bag<int>() { 1, 2, 3, 4, 5, 6 };

            var expected = new[] { 1, 2, 3 };
            var actual = bag.Where(i => i < 4).ToArray();
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void CanAddNullItems() {
            var bag = new Bag<string> { "a", "b", null, "d", "e" };

            Assert.IsTrue(bag.Contains("a"));
            Assert.IsFalse(bag.Contains("c"));
            Assert.IsTrue(bag.Contains(null));
        }
    }
}