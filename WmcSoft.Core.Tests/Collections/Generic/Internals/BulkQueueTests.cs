using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic.Internals
{
    [TestClass]
    [Ignore]
    public class BulkQueueTests
    {
        [TestMethod]
        public void CanEnqueue() {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var queue = new BulkQueue<int>();
            foreach (var item in data)
                queue.Enqueue(item);

            var length = data.Length;
            for (int i = 0; i < length; i++) {
                var expected = data[i];
                var actual = queue.Dequeue();
                Assert.AreEqual(expected, actual);
            }
            Assert.IsTrue(queue.IsEmpty());
        }

        [TestMethod]
        public void CanBulkEnqueue() {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var queue = new BulkQueue<int>();
            queue.BulkEnqueue(data.Length, (array, index) => Array.Copy(data, 0, array, index, data.Length));

            var length = data.Length;
            for (int i = length - 1; i >= 0; i--) {
                var expected = data[i];
                var actual = queue.Dequeue();
                Assert.AreEqual(expected, actual);
            }
            Assert.IsTrue(queue.IsEmpty());
        }

        [TestMethod]
        public void CanBulkEnqueueAfterAFewEnqueues() {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var queue = new BulkQueue<int>();
            queue.Enqueue(-20);
            queue.BulkEnqueue(data.Length, (array, index) => Array.Copy(data, 0, array, index, data.Length));
            queue.Enqueue(20);

            Assert.AreEqual(-20, queue.Dequeue());

            var length = data.Length;
            for (int i = length - 1; i >= 0; i--) {
                var expected = data[i];
                var actual = queue.Dequeue();
                Assert.AreEqual(expected, actual);
            }

            Assert.AreEqual(20, queue.Dequeue());

            Assert.IsTrue(queue.IsEmpty());
        }
    }
}
