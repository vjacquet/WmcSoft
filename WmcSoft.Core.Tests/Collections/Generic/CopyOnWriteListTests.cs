using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class CopyOnWriteListTests
    {
        [TestMethod]
        public void CanAddToCopyOnWriteList() {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteList<int>(list);
            cow.Add(6);

            Assert.AreEqual(5, list.Count);
            Assert.AreEqual(6, cow.Count);
        }

        [TestMethod]
        public void CanRemoveToCopyOnWriteList() {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteList<int>(list);
            cow.Remove(2);

            Assert.AreEqual(5, list.Count);
            Assert.IsTrue(list.Contains(2));
            Assert.AreEqual(4, cow.Count);
            Assert.IsFalse(cow.Contains(2));
        }

        [TestMethod]
        public void CanRemoveAtToCopyOnWriteList() {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteList<int>(list);
            cow.RemoveAt(1);

            Assert.AreEqual(5, list.Count);
            Assert.IsTrue(list.Contains(2));
            Assert.AreEqual(4, cow.Count);
            Assert.IsFalse(cow.Contains(2));
        }

        [TestMethod]
        public void CanClearCopyOnWriteList() {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteList<int>(list);
            cow.Clear();

            Assert.AreEqual(5, list.Count);
            Assert.AreEqual(0, cow.Count);
        }

        [TestMethod]
        public void CanCopyFromCopyOnWriteList() {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteList<int>(list);
            cow.Remove(2);
            cow.Add(6);

            var buffer = new int[5];

            cow.CopyTo(buffer, 0);
            CollectionAssert.AreEquivalent(buffer, new[] { 1, 3, 4, 5, 6 });

            list.CopyTo(buffer);
            CollectionAssert.AreEquivalent(buffer, new[] { 1, 2, 3, 4, 5 });
        }

        [TestMethod]
        public void CanSetOrGetFromCopyOnWriteList() {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteList<int>(list);
            cow[2] = 6;

            Assert.AreEqual(6, cow[2]);
            Assert.AreEqual(3, list[2]);
        }
    }
}
