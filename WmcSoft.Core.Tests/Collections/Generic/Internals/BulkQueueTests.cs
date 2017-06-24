using System;
using Xunit;

namespace WmcSoft.Collections.Generic.Internals
{
    public class BulkQueueTests
    {
        [Fact]
        public void CanEnqueue()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var queue = new BulkQueue<int>();
            foreach (var item in data)
                queue.Enqueue(item);

            var length = data.Length;
            for (int i = 0; i < length; i++) {
                var expected = data[i];
                var actual = queue.Dequeue();
                Assert.Equal(expected, actual);
            }
            Assert.True(queue.IsEmpty());
        }

        [Fact]
        public void CanBulkEnqueue()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var queue = new BulkQueue<int>();
            queue.BulkEnqueue(data.Length, (array, index) => Array.Copy(data, 0, array, index, data.Length));

            var length = data.Length;
            for (int i = 0; i < length; i++) {
                var expected = data[i];
                var actual = queue.Dequeue();
                Assert.Equal(expected, actual);
            }
            Assert.True(queue.IsEmpty());
        }

        [Fact]
        public void CanEnqueueItems()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var queue = new BulkQueue<int>();
            queue.Enqueue(data);

            var length = data.Length;
            for (int i = 0; i < length; i++) {
                var expected = data[i];
                var actual = queue.Dequeue();
                Assert.Equal(expected, actual);
            }
            Assert.True(queue.IsEmpty());
        }

        [Fact]
        public void CanBulkEnqueueAfterAFewEnqueues()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var queue = new BulkQueue<int>();
            queue.Enqueue(-20);
            queue.Enqueue(-30);
            queue.Enqueue(-40);
            Assert.Equal(-20, queue.Dequeue());
            Assert.Equal(-30, queue.Dequeue());

            queue.BulkEnqueue(data.Length, (array, index) => Array.Copy(data, 0, array, index, data.Length));
            queue.Enqueue(20);

            Assert.Equal(-40, queue.Dequeue());

            var length = data.Length;
            for (int i = 0; i < length; i++) {
                var expected = data[i];
                var actual = queue.Dequeue();
                Assert.Equal(expected, actual);
            }

            Assert.Equal(20, queue.Dequeue());

            Assert.True(queue.IsEmpty());
        }
    }
}