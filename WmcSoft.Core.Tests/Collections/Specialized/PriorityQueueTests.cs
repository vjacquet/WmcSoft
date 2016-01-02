using System;
using System.Collections.Generic;
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
        public void CheckDequeueOrder() {
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