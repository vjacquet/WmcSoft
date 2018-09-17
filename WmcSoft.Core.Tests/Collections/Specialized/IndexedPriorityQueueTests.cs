using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Collections.Specialized
{
    public class IndexedPriorityQueueTests
    {
        IEnumerator<T> Enumerate<T>(params T[] values)
        {
            return values.GetEnumerator(0, values.Length);
        }

        IEnumerable<T> Multiway<T>(params IEnumerator<T>[] enumerators)
        {
            var comparer = Comparer<T>.Default.Reverse();
            var length = enumerators.Length;
            var pq = new IndexedPriorityQueue<T>(comparer, length);
            for (int i = 0; i < length; i++) {
                if (enumerators[i].MoveNext())
                    pq.Enqueue(i, enumerators[i].Current);
            }
            while (pq.Count > 0) {
                T value;
                int i = pq.Dequeue(out value);
                yield return value;
                if (enumerators[i].MoveNext())
                    pq.Enqueue(i, enumerators[i].Current);
            }
        }

        [Fact]
        public void CanSortMultiway()
        {
            var streams = new[] {
                Enumerate("A","B","C","F","G","I","I","Z"),
                Enumerate("B","D","H","P","Q","Q"),
                Enumerate("A","B","E","F","J","N"),
            };
            var actual = string.Concat(Multiway(streams));
            var expected = "AABBBCDEFFGHIIJNPQQZ";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckContainsOnIndex()
        {
            var pq = new IndexedPriorityQueue<string>(4);
            pq.Enqueue(0, "zero");
            pq.Enqueue(2, "two");

            Assert.True(pq.Contains(0));
            Assert.True(pq.Contains(2));
            Assert.False(pq.Contains(1));
            pq.Enqueue(1, "one");
            pq.Enqueue(3, "three");
            Assert.True(pq.Contains(1));
            Assert.True(pq.Contains(3));
        }

        [Fact]
        public void CheckOutOfRangeOnContains()
        {
            var pq = new IndexedPriorityQueue<string>(4);
            pq.Enqueue(0, "zero");
            pq.Enqueue(2, "two");

            Assert.Throws<IndexOutOfRangeException>(() => pq.Contains(5));
        }

        [Fact]
        public void CanClone()
        {
            var pq = new IndexedPriorityQueue<string>(4);
            pq.Enqueue(0, "zero");
            pq.Enqueue(2, "two");

            var clone = pq.Clone();
            Assert.Equal(pq, clone);

            pq.Enqueue(1, "one");
            Assert.NotEqual(pq, clone);

            clone.Enqueue(1, "one");
            Assert.Equal(pq, clone);
        }

        [Fact]
        public void CanChangePriority()
        {
            var pq = new IndexedPriorityQueue<decimal>(4);
            pq.Enqueue(1, 1m);
            pq.Enqueue(3, 3m);
            pq.Enqueue(2, 2m);
            Assert.Equal(3, pq.Count);
            Assert.Equal(3, pq.Peek());
            pq.ChangeValue(3, -3m);
            Assert.Equal(2, pq.Peek());
        }
    }
}
