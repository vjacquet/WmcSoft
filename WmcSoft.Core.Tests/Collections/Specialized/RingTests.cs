using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Specialized
{
    [TestClass]
    public class RingTests
    {
        [TestMethod]
        public void CheckRingIsCollection() {
            ContractAssert.Collection(new Ring<int>(5));
        }

        [TestMethod]
        public void CanRemoveWhenGapIsAtThenEnd() {
            var ring = new Ring<int>(8);
            for (int i = 0; i < 10; i++) {
                ring.Enqueue(i);
            }
            Assert.IsTrue(ring.Remove(5));
            var actual = ring.ToArray();
            CollectionAssert.AreEqual(new[] { 2, 3, 4, 6, 7, 8, 9 }, actual);
        }

        [TestMethod]
        public void CanRemoveWhenGapIsAtThenStart() {
            var ring = new Ring<int>(8);
            for (int i = 0; i < 12; i++) {
                ring.Enqueue(i);
            }
            Assert.IsTrue(ring.Remove(9));
            var actual = ring.ToArray();
            CollectionAssert.AreEqual(new[] { 4, 5, 6, 7, 8, 10, 11 }, actual);
        }

        [TestMethod]
        public void CanOverwrite() {
            var ring = new Ring<int>(3);
            Assert.AreEqual(default(int), ring.Enqueue(1));
            Assert.AreEqual(default(int), ring.Enqueue(2));
            Assert.AreEqual(default(int), ring.Enqueue(3));
            Assert.AreEqual(1, ring.Enqueue(4));
            Assert.AreEqual(3, ring.Count);
            Assert.AreEqual(2, ring.Dequeue());
            Assert.AreEqual(3, ring.Dequeue());
            Assert.AreEqual(4, ring.Dequeue());
            Assert.AreEqual(0, ring.Count);
        }

        [TestMethod]
        public void CanUnderwrite() {
            var ring = new Ring<int>(5);
            Assert.AreEqual(default(int), ring.Enqueue(1));
            Assert.AreEqual(default(int), ring.Enqueue(2));
            Assert.AreEqual(default(int), ring.Enqueue(3));
            Assert.AreEqual(3, ring.Count);
            Assert.AreEqual(1, ring.Dequeue());
            Assert.AreEqual(2, ring.Dequeue());
            Assert.AreEqual(3, ring.Dequeue());
            Assert.AreEqual(0, ring.Count);
        }

        [TestMethod]
        public void CanIncreaseCapacityWhenFull() {
            var ring = new Ring<int>(3);
            Assert.AreEqual(default(int), ring.Enqueue(1));
            Assert.AreEqual(default(int), ring.Enqueue(2));
            Assert.AreEqual(default(int), ring.Enqueue(3));
            Assert.AreEqual(3, ring.Count);
            ring.Capacity = 4;
            Assert.AreEqual(3, ring.Count);
            Assert.AreEqual(1, ring.Dequeue());
            Assert.AreEqual(2, ring.Dequeue());
            Assert.AreEqual(3, ring.Dequeue());
            Assert.AreEqual(0, ring.Count);
        }

        [TestMethod]
        public void CanDecreaseCapacityWhenFull() {
            var ring = new Ring<int>(3);
            Assert.AreEqual(default(int), ring.Enqueue(1));
            Assert.AreEqual(default(int), ring.Enqueue(2));
            Assert.AreEqual(default(int), ring.Enqueue(3));
            Assert.AreEqual(3, ring.Count);
            ring.Capacity = 2;
            Assert.AreEqual(2, ring.Count);
            Assert.AreEqual(1, ring.Dequeue());
            Assert.AreEqual(2, ring.Dequeue());
            Assert.AreEqual(0, ring.Count);
        }

        [TestMethod]
        public void CanEnumerateWhenFull() {
            var ring = new Ring<int>(3);
            Assert.AreEqual(default(int), ring.Enqueue(1));
            Assert.AreEqual(default(int), ring.Enqueue(2));
            Assert.AreEqual(default(int), ring.Enqueue(3));
            var i = 0;
            foreach (var value in ring)
                Assert.AreEqual(++i, value);
            Assert.AreEqual(i, 3);
        }

        [TestMethod]
        public void CanEnumerateWhenOverflown() {
            var ring = new Ring<int>(3);
            ring.Enqueue(1);
            ring.Enqueue(2);
            ring.Enqueue(3);
            ring.Enqueue(4);
            var i = 1;
            foreach (var value in ring)
                Assert.AreEqual(++i, value);
        }

        [TestMethod]
        public void CanEnumerateWhenPartiallyFull() {
            var ring = new Ring<int>(3);
            Assert.AreEqual(default(int), ring.Enqueue(1));
            Assert.AreEqual(default(int), ring.Enqueue(2));
            var i = 0;
            foreach (var value in ring)
                Assert.AreEqual(++i, value);
            Assert.AreEqual(i, 2);
        }

        [TestMethod]
        public void CanEnumerateWhenEmpty() {
            var ring = new Ring<int>(3);
            foreach (var value in ring)
                Assert.Fail("Unexpected Value: " + value);
        }
    }
}
