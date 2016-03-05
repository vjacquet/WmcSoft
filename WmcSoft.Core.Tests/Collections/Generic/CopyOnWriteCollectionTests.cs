using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class CopyOnWriteCollectionTests
    {
        [TestMethod]
        public void CanAddToCopyOnWriteCollection() {
            var collection = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteCollection<int>(collection);
            cow.Add(6);

            Assert.AreEqual(5, collection.Count);
            Assert.AreEqual(6, cow.Count);
        }

        [TestMethod]
        public void CanRemoveToCopyOnWriteCollection() {
            var collection = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteCollection<int>(collection);
            cow.Remove(2);

            Assert.AreEqual(5, collection.Count);
            Assert.IsTrue(collection.Contains(2));
            Assert.AreEqual(4, cow.Count);
            Assert.IsFalse(cow.Contains(2));
        }

        [TestMethod]
        public void CanClearCopyOnWriteCollection() {
            var collection = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteCollection<int>(collection);
            cow.Clear();

            Assert.AreEqual(5, collection.Count);
            Assert.AreEqual(0, cow.Count);
        }

        [TestMethod]
        public void CanCopyFromCopyOnWriteCollection() {
            var collection = new List<int>() { 1, 2, 3, 4, 5 };
            var cow = new CopyOnWriteCollection<int>(collection);
            cow.Remove(2);
            cow.Add(6);

            var buffer = new int[5];

            cow.CopyTo(buffer, 0);
            CollectionAssert.AreEquivalent(buffer, new[] { 1, 3, 4, 5, 6 });

            collection.CopyTo(buffer);
            CollectionAssert.AreEquivalent(buffer, new[] { 1, 2, 3, 4, 5 });
        }
    }
}
