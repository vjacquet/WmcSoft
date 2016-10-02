using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Collections.Specialized
{
    [TestClass]
    public class IndexedPriorityQueueTests
    {
        IEnumerator<T> Enumerate<T>(params T[] values) {
            return values.GetEnumerator(0, values.Length);
        }

        IEnumerable<T> Multiway<T>(params IEnumerator<T>[] enumerators) {
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

        [TestMethod]
        public void CanSortMultiway() {
            var streams = new[] {
                Enumerate("A","B","C","F","G","I","I","Z"),
                Enumerate("B","D","H","P","Q","Q"),
                Enumerate("A","B","E","F","J","N"),
            };
            var actual = string.Concat(Multiway(streams));
            var expected = "AABBBCDEFFGHIIJNPQQZ";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckContainsOnIndex() {
            var pq = new IndexedPriorityQueue<string>(4);
            pq.Enqueue(0, "zero");
            pq.Enqueue(2, "two");

            Assert.IsTrue(pq.Contains(0));
            Assert.IsTrue(pq.Contains(2));
            Assert.IsFalse(pq.Contains(1));
            pq.Enqueue(1, "one");
            pq.Enqueue(3, "three");
            Assert.IsTrue(pq.Contains(1));
            Assert.IsTrue(pq.Contains(3));
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void CheckOutOfRangeOnContains() {
            var pq = new IndexedPriorityQueue<string>(4);
            pq.Enqueue(0, "zero");
            pq.Enqueue(2, "two");

            Assert.IsFalse(pq.Contains(5));
        }
    }
}