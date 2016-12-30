using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections;
using WmcSoft.Collections.Generic;

namespace WmcSoft.TestTools.UnitTesting
{
    public static class ContractAssert
    {
        public const int CollectionMinValue = 1;
        public const int CollectionMaxValue = 5;

        class CollectionAdapter<T> : ICollection
        {
            readonly ICollection<T> _storage;

            public CollectionAdapter(ICollection<T> collection) {
                _storage = collection;
            }

            public int Count { get { return _storage.Count; } }

            public bool IsSynchronized {
                get { return false; }
            }

            public object SyncRoot {
                get { return null; }
            }

            public void CopyTo(Array array, int index) {
                foreach (var item in _storage)
                    array.SetValue(item, index++);
            }

            public IEnumerator GetEnumerator() {
                return _storage.GetEnumerator();
            }
        }

        public static void Disposable(IDisposable disposable) {
            disposable.Dispose();
            disposable.Dispose();
        }

        public static void Collection<TCollection>(TCollection collection)
            where TCollection : ICollection<int> {
            var adapter = new CollectionAdapter<int>(collection);

            collection.Clear();
            Assert.AreEqual(0, collection.Count);

            collection.Add(1);
            Assert.AreEqual(1, collection.Count);
            Assert.IsTrue(collection.Contains(1));
            Assert.IsFalse(collection.Contains(2));

            collection.Add(2);
            Assert.AreEqual(2, collection.Count);
            Assert.IsTrue(collection.Contains(1));
            Assert.IsTrue(collection.Contains(2));

            var buffer = new int[] { -1, -2, -3, -4 };
            collection.CopyTo(buffer, 2);
            Assert.AreEqual(-1, buffer[0]);
            Assert.AreEqual(-2, buffer[1]);
            CollectionAssert.AreEquivalent(new int[] { -1, -2, 2, 1 }, buffer);
            CollectionAssert.AreEquivalent(new int[] { 1, 2 }, adapter);

            Assert.IsTrue(collection.Remove(2));
            Assert.AreEqual(1, collection.Count);

            Assert.IsFalse(collection.Remove(3));
            Assert.AreEqual(1, collection.Count);

            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        public static void Set<TSet>(TSet set)
            where TSet : ISet<int> {
            Collection(set);
            var adapter = new CollectionAdapter<int>(set);

            Assert.IsTrue(set.Add(1));
            Assert.IsTrue(set.Add(2));
            Assert.IsFalse(set.Add(1));
            Assert.AreEqual(2, set.Count);

            set.Add(3);
            set.Add(4);
            set.Add(5);
            Assert.AreEqual(5, set.Count);

            set.ExceptWith(new[] { 2, 4 });
            CollectionAssert.AreEquivalent(new int[] { 1, 3, 5 }, adapter);

            set.UnionWith(new[] { 2, 3, 4 });
            CollectionAssert.AreEquivalent(new int[] { 1, 2, 3, 4, 5 }, adapter);

            set.IntersectWith(new[] { 2, 4, 6 });
            CollectionAssert.AreEquivalent(new int[] { 2, 4 }, adapter);

            Assert.IsFalse(set.Overlaps(new int[] { 3, 5 }));
            Assert.IsTrue(set.Overlaps(new int[] { 3, 4 }));

            set.Clear();
            set.Add(1);
            set.Add(2);
            set.Add(3);
            CollectionAssert.AreEquivalent(new int[] { 1, 2, 3 }, adapter);
            set.SymmetricExceptWith(new int[] { 2, 4 });
            CollectionAssert.AreEquivalent(new int[] { 1, 3, 4 }, adapter);

            CheckSubsetAndProperSubsetOnDifferentSets(set);
            CheckSubsetAndProperSubsetOnEquivalentSets(set);
        }

        static void CheckSubsetAndProperSubsetOnDifferentSets<TSet>(TSet set)
            where TSet : ISet<int> {
            set.Clear();
            set.AddRange(Enumerable.Range(1, 4));

            var superset = Enumerable.Range(0, 10).ToArray();
            Assert.IsFalse(set.SetEquals(superset));
            Assert.IsTrue(set.IsSubsetOf(superset));
            Assert.IsTrue(set.IsProperSubsetOf(superset));

            var subset = Enumerable.Range(1, 2).ToArray();
            Assert.IsTrue(set.IsSupersetOf(subset));
            Assert.IsTrue(set.IsProperSupersetOf(subset));
        }

        static void CheckSubsetAndProperSubsetOnEquivalentSets<TSet>(TSet set)
            where TSet : ISet<int> {
            set.Clear();
            set.AddRange(Enumerable.Range(1, 4));

            var other = new SortedSequenceSet<int>(Enumerable.Range(1, 4));
            Assert.IsTrue(set.SetEquals(other));
            Assert.IsTrue(set.IsSubsetOf(other));
            Assert.IsFalse(set.IsProperSubsetOf(other));
            Assert.IsTrue(set.IsSupersetOf(other));
            Assert.IsFalse(set.IsProperSupersetOf(other));
        }

        public static void Ordinal<TOrdinal, T>(TOrdinal ordinal, T startValue, T endValue, int distance)
            where TOrdinal : IOrdinal<T> {
            var actual = ordinal.Advance(startValue, distance);
            Assert.AreEqual(endValue, actual);
            Assert.AreEqual(distance, ordinal.Distance(startValue, endValue));
        }
    }
}
