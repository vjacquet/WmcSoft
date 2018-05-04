#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using WmcSoft.Collections;
using WmcSoft.Collections.Generic;

namespace WmcSoft.TestTools.UnitTesting
{
    public static partial class ContractAssert
    {
        public const int CollectionMinValue = 1;
        public const int CollectionMaxValue = 5;

        class CollectionAdapter<T> : ICollection, IEnumerable<T>
        {
            readonly ICollection<T> _storage;

            public CollectionAdapter(ICollection<T> collection)
            {
                _storage = collection;
            }

            public int Count => _storage.Count;
            public bool IsSynchronized => false;
            public object SyncRoot => null;

            public void CopyTo(Array array, int index)
            {
                foreach (var item in _storage)
                    array.SetValue(item, index++);
            }

            public IEnumerator GetEnumerator()
            {
                return _storage.GetEnumerator();
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                return _storage.GetEnumerator();
            }
        }

        public static void Disposable(IDisposable disposable)
        {
            disposable.Dispose();
            disposable.Dispose();
        }

        public static void Collection<TCollection>(TCollection collection)
            where TCollection : ICollection<int>
        {
            var adapter = new CollectionAdapter<int>(collection);

            collection.Clear();
            Assert.Equal(0, collection.Count);

            collection.Add(1);
            Assert.Equal(1, collection.Count);
            Assert.True(collection.Contains(1));
            Assert.False(collection.Contains(2));

            collection.Add(2);
            Assert.Equal(2, collection.Count);
            Assert.True(collection.Contains(1));
            Assert.True(collection.Contains(2));

            var buffer = new[] { -1, -2, -3, -4 };
            collection.CopyTo(buffer, 2);
            Assert.Equal(-1, buffer[0]);
            Assert.Equal(-2, buffer[1]);
            Assert.True(buffer.Equivalent(new[] { -1, -2, 2, 1 }));
            Assert.True(adapter.Equivalent(new[] { 1, 2 }));

            Assert.True(collection.Remove(2));
            Assert.Equal(1, collection.Count);

            Assert.False(collection.Remove(3));
            Assert.Equal(1, collection.Count);

            collection.Clear();
            Assert.Equal(0, collection.Count);
        }

        public static void Set<TSet>(TSet set)
            where TSet : ISet<int>
        {
            Collection(set);
            var adapter = new CollectionAdapter<int>(set);

            Assert.True(set.Add(1));
            Assert.True(set.Add(2));
            Assert.False(set.Add(1));
            Assert.Equal(2, set.Count);

            set.Add(3);
            set.Add(4);
            set.Add(5);
            Assert.Equal(5, set.Count);

            set.ExceptWith(new[] { 2, 4 });
            Assert.True(adapter.Equivalent(new[] { 1, 3, 5 }));

            set.UnionWith(new[] { 2, 3, 4 });
            Assert.True(adapter.Equivalent(new[] { 1, 2, 3, 4, 5 }));

            set.IntersectWith(new[] { 2, 4, 6 });
            Assert.Equal(new int[] { 2, 4 }, adapter);

            Assert.False(set.Overlaps(new[] { 3, 5 }));
            Assert.True(set.Overlaps(new[] { 3, 4 }));

            set.Clear();
            set.Add(1);
            set.Add(2);
            set.Add(3);
            Assert.True(adapter.Equivalent(new[] { 1, 2, 3 }));
            set.SymmetricExceptWith(new[] { 2, 4 });
            Assert.True(adapter.Equivalent(new[] { 1, 3, 4 }));

            CheckSubsetAndProperSubsetOnDifferentSets(set);
            CheckSubsetAndProperSubsetOnEquivalentSets(set);
        }

        static void CheckSubsetAndProperSubsetOnDifferentSets<TSet>(TSet set)
            where TSet : ISet<int>
        {
            set.Clear();
            set.AddRange(Enumerable.Range(1, 4));

            var superset = Enumerable.Range(0, 10).ToArray();
            Assert.False(set.SetEquals(superset));
            Assert.True(set.IsSubsetOf(superset));
            Assert.True(set.IsProperSubsetOf(superset));

            var subset = Enumerable.Range(1, 2).ToArray();
            Assert.True(set.IsSupersetOf(subset));
            Assert.True(set.IsProperSupersetOf(subset));
        }

        static void CheckSubsetAndProperSubsetOnEquivalentSets<TSet>(TSet set)
            where TSet : ISet<int>
        {
            set.Clear();
            set.AddRange(Enumerable.Range(1, 4));

            var other = new HashSet<int>(Enumerable.Range(1, 4));
            Assert.True(set.SetEquals(other));
            Assert.True(set.IsSubsetOf(other));
            Assert.False(set.IsProperSubsetOf(other));
            Assert.True(set.IsSupersetOf(other));
            Assert.False(set.IsProperSupersetOf(other));
        }
    }
}
