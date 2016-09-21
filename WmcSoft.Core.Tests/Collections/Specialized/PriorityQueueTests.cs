using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Specialized
{
    [TestClass]
    public class PriorityQueueTests
    {
        [TestMethod]
        public void CheckGrow() {
            var priorityQueue = new PriorityQueue<int>(4);
            priorityQueue.Enqueue(15);
            priorityQueue.Enqueue(3);
            priorityQueue.Enqueue(17);
            priorityQueue.Enqueue(7);
            priorityQueue.Enqueue(11);
        }

        [TestMethod]
        public void CheckSparsedDequeueOrder() {
            var priorityQueue = new PriorityQueue<int>();
            priorityQueue.Enqueue(15);
            priorityQueue.Enqueue(3);
            priorityQueue.Enqueue(17);
            priorityQueue.Enqueue(7);
            priorityQueue.Enqueue(11);
            Assert.AreEqual(17, priorityQueue.Dequeue());
            Assert.AreEqual(15, priorityQueue.Dequeue());
            Assert.AreEqual(11, priorityQueue.Dequeue());
            Assert.AreEqual(7, priorityQueue.Dequeue());
            Assert.AreEqual(3, priorityQueue.Dequeue());
        }

        [TestMethod]
        public void CheckDenseDequeueOrder() {
            var random = new Random(1664);
            var priorityQueue = new PriorityQueue<int>(1024);
            var data = Enumerable.Range(1, 1024).ToArray();
            data.Shuffle(random);
            foreach (var datum in data) {
                priorityQueue.Enqueue(datum);
            }

            int expected = 1024;
            while (priorityQueue.Count > 0) {
                Assert.AreEqual(expected--, priorityQueue.Dequeue());
            }
        }

        [TestMethod]
        public void CheckEnumerator() {
            var list = new List<int> { 15, 3, 17, 7, 11 };

            var priorityQueue = new PriorityQueue<int>();
            priorityQueue.Enqueue(15);
            priorityQueue.Enqueue(3);
            priorityQueue.Enqueue(17);
            priorityQueue.Enqueue(7);
            priorityQueue.Enqueue(11);

            Assert.AreEqual(list.Count, priorityQueue.Count);
            foreach (int value in priorityQueue) {
                int index = list.IndexOf(value);
                Assert.IsTrue(index >= 0);
                list.RemoveAt(index);
            }

            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void CheckCopyTo() {
            var list = new List<int> { 15, 3, 17, 7, 11 };
            var priorityQueue = new PriorityQueue<int>(list);
            var actual = new int[5];
            priorityQueue.CopyTo(actual, 0);
            var expected = new[] { 17, 15, 11, 7, 3 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CheckChangingWhileEnumerating() {
            var priorityQueue = new PriorityQueue<int>();
            priorityQueue.Enqueue(15);
            priorityQueue.Enqueue(3);

            foreach (int value in priorityQueue)
                priorityQueue.Dequeue();
        }
    }
}