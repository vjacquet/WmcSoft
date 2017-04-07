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
        [Ignore]
        public void CheckRingIsCollection() {
            ContractAssert.Collection(new Ring<int>(5));
        }

        [TestMethod]
        public void CanOverwrite() {
            var buffer = new Ring<int>(3);
            Assert.AreEqual(default(int), buffer.Enqueue(1));
            Assert.AreEqual(default(int), buffer.Enqueue(2));
            Assert.AreEqual(default(int), buffer.Enqueue(3));
            Assert.AreEqual(1, buffer.Enqueue(4));
            Assert.AreEqual(3, buffer.Count);
            Assert.AreEqual(2, buffer.Dequeue());
            Assert.AreEqual(3, buffer.Dequeue());
            Assert.AreEqual(4, buffer.Dequeue());
            Assert.AreEqual(0, buffer.Count);
        }

        [TestMethod]
        public void CanUnderwrite() {
            var buffer = new Ring<int>(5);
            Assert.AreEqual(default(int), buffer.Enqueue(1));
            Assert.AreEqual(default(int), buffer.Enqueue(2));
            Assert.AreEqual(default(int), buffer.Enqueue(3));
            Assert.AreEqual(3, buffer.Count);
            Assert.AreEqual(1, buffer.Dequeue());
            Assert.AreEqual(2, buffer.Dequeue());
            Assert.AreEqual(3, buffer.Dequeue());
            Assert.AreEqual(0, buffer.Count);
        }

        [TestMethod]
        public void CanIncreaseCapacityWhenFull() {
            var buffer = new Ring<int>(3);
            Assert.AreEqual(default(int), buffer.Enqueue(1));
            Assert.AreEqual(default(int), buffer.Enqueue(2));
            Assert.AreEqual(default(int), buffer.Enqueue(3));
            Assert.AreEqual(3, buffer.Count);
            buffer.Capacity = 4;
            Assert.AreEqual(3, buffer.Count);
            Assert.AreEqual(1, buffer.Dequeue());
            Assert.AreEqual(2, buffer.Dequeue());
            Assert.AreEqual(3, buffer.Dequeue());
            Assert.AreEqual(0, buffer.Count);
        }

        [TestMethod]
        public void CanDecreaseCapacityWhenFull() {
            var buffer = new Ring<int>(3);
            Assert.AreEqual(default(int), buffer.Enqueue(1));
            Assert.AreEqual(default(int), buffer.Enqueue(2));
            Assert.AreEqual(default(int), buffer.Enqueue(3));
            Assert.AreEqual(3, buffer.Count);
            buffer.Capacity = 2;
            Assert.AreEqual(2, buffer.Count);
            Assert.AreEqual(1, buffer.Dequeue());
            Assert.AreEqual(2, buffer.Dequeue());
            Assert.AreEqual(0, buffer.Count);
        }

        [TestMethod]
        public void CanEnumerateWhenFull() {
            var buffer = new Ring<int>(3);
            Assert.AreEqual(default(int), buffer.Enqueue(1));
            Assert.AreEqual(default(int), buffer.Enqueue(2));
            Assert.AreEqual(default(int), buffer.Enqueue(3));
            var i = 0;
            foreach (var value in buffer)
                Assert.AreEqual(++i, value);
            Assert.AreEqual(i, 3);
        }

        [TestMethod]
        public void CanEnumerateWhenOverflown() {
            var buffer = new Ring<int>(3);
            buffer.Enqueue(1);
            buffer.Enqueue(2);
            buffer.Enqueue(3);
            buffer.Enqueue(4);
            var i = 1;
            foreach (var value in buffer)
                Assert.AreEqual(++i, value);
        }

        [TestMethod]
        public void CanEnumerateWhenPartiallyFull() {
            var buffer = new Ring<int>(3);
            Assert.AreEqual(default(int), buffer.Enqueue(1));
            Assert.AreEqual(default(int), buffer.Enqueue(2));
            var i = 0;
            foreach (var value in buffer)
                Assert.AreEqual(++i, value);
            Assert.AreEqual(i, 2);
        }

        [TestMethod]
        public void CanEnumerateWhenEmpty() {
            var buffer = new Ring<int>(3);
            foreach (var value in buffer)
                Assert.Fail("Unexpected Value: " + value);
        }
    }
}
