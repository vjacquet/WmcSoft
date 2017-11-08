using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WmcSoft.Collections.Specialized
{
    public class PriorityQueueTests
    {
        [Fact]
        public void CheckGrow()
        {
            var priorityQueue = new PriorityQueue<int>(4);
            priorityQueue.Enqueue(15);
            priorityQueue.Enqueue(3);
            priorityQueue.Enqueue(17);
            priorityQueue.Enqueue(7);
            priorityQueue.Enqueue(11);
        }

        [Fact]
        public void CheckSparsedDequeueOrder()
        {
            var priorityQueue = new PriorityQueue<int>();
            priorityQueue.Enqueue(15);
            priorityQueue.Enqueue(3);
            priorityQueue.Enqueue(17);
            priorityQueue.Enqueue(7);
            priorityQueue.Enqueue(11);
            Assert.Equal(17, priorityQueue.Dequeue());
            Assert.Equal(15, priorityQueue.Dequeue());
            Assert.Equal(11, priorityQueue.Dequeue());
            Assert.Equal(7, priorityQueue.Dequeue());
            Assert.Equal(3, priorityQueue.Dequeue());
        }

        [Fact]
        public void CheckDenseDequeueOrder()
        {
            var random = new Random(1664);
            var priorityQueue = new PriorityQueue<int>(1024);
            var data = Enumerable.Range(1, 1024).ToArray();
            data.Shuffle(random);
            foreach (var datum in data) {
                priorityQueue.Enqueue(datum);
            }

            int expected = 1024;
            while (priorityQueue.Count > 0) {
                Assert.Equal(expected--, priorityQueue.Dequeue());
            }
        }

        [Fact]
        public void CheckEnumerator()
        {
            var list = new List<int> { 15, 3, 17, 7, 11 };

            var priorityQueue = new PriorityQueue<int>();
            priorityQueue.Enqueue(15);
            priorityQueue.Enqueue(3);
            priorityQueue.Enqueue(17);
            priorityQueue.Enqueue(7);
            priorityQueue.Enqueue(11);

            Assert.Equal(list.Count, priorityQueue.Count);
            foreach (int value in priorityQueue) {
                int index = list.IndexOf(value);
                Assert.True(index >= 0);
                list.RemoveAt(index);
            }

            Assert.Empty(list);
        }

        [Fact]
        public void CheckCopyTo()
        {
            var list = new List<int> { 15, 3, 17, 7, 11 };
            var priorityQueue = new PriorityQueue<int>(list);
            var actual = new int[5];
            priorityQueue.CopyTo(actual, 0);
            var expected = new[] { 17, 15, 11, 7, 3 };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckChangingWhileEnumerating()
        {
            var priorityQueue = new PriorityQueue<int>();
            priorityQueue.Enqueue(15);
            priorityQueue.Enqueue(3);

            Assert.Throws<InvalidOperationException>(() => {
                foreach (int value in priorityQueue)
                    priorityQueue.Dequeue();
            });
        }
    }
}
