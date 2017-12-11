#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

using static WmcSoft.Collections.Generic.CollectionExtensions;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Implements a generic ordreded collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class OrderedCollection<T> : IOrderedCollection<T>
    {
        static void ThrowNoSuchElement()
        {
            throw new InvalidOperationException("No such element in the collection.");
        }

        private readonly List<T> _storage;

        private List<T> NotEmpty {
            get {
                if (_storage.Count == 0)
                    ThrowNoSuchElement();
                return _storage;
            }
        }

        #region Lifecycle

        public OrderedCollection(IComparer<T> comparer = null)
        {
            _storage = new List<T>();
            Comparer = comparer ?? Comparer<T>.Default;
        }

        public OrderedCollection(int capacity, IComparer<T> comparer = null)
        {
            _storage = new List<T>(capacity);
            Comparer = comparer ?? Comparer<T>.Default;
        }

        public OrderedCollection(IEnumerable<T> enumerable, IComparer<T> comparer = null)
        {
            _storage = new List<T>(enumerable);
            Comparer = comparer ?? Comparer<T>.Default;
            _storage.Sort(Comparer);
        }

        #endregion

        public IComparer<T> Comparer { get; }

        public T Min => NotEmpty[0];

        public T Max => NotEmpty[Count - 1];

        public int Count => _storage.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            var found = _storage.BinarySearch(item, Comparer);
            if (found < 0) {
                found = ~found;
                _storage.Insert(found, item);
            }
        }

        public T Floor(T value)
        {
            var found = _storage.BinarySearch(value, Comparer);
            if (found >= 0)
                return _storage[found];
            return _storage[~found - 1];
        }

        public T Ceiling(T value)
        {
            var found = _storage.BinarySearch(value, Comparer);
            if (found >= 0)
                return _storage[found];
            return _storage[~found];
        }

        public void Clear()
        {
            _storage.Clear();
        }

        public bool Contains(T item)
        {
            return _storage.BinarySearch(item, Comparer) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _storage.CopyTo(array, arrayIndex);
        }

        public int CountBetween(T lo, T hi)
        {
            var m = UnguardedLowerBound(_storage, 0, _storage.Count, lo, Comparer);
            var n = UnguardedUpperBound(_storage, 0, _storage.Count, hi, Comparer);
            return n - m;
        }

        public IReadOnlyCollection<T> EnumerateBetween(T lo, T hi)
        {
            var m = UnguardedLowerBound(_storage, 0, _storage.Count, lo, Comparer);
            var n = UnguardedUpperBound(_storage, 0, _storage.Count, hi, Comparer);
            return _storage.Sublist(m, m - n).AsReadOnly();
        }

        public List<T>.Enumerator GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        public int Rank(T value)
        {
            return _storage.BinaryRank(value, Comparer);
        }

        public bool Remove(T item)
        {
            var found = _storage.BinarySearch(item);
            if (found < 0)
                return false;

            _storage.RemoveAt(found);
            return true;
        }

        public void RemoveMax()
        {
            if (Count > 0)
                _storage.RemoveAt(Count - 1);
        }

        public void RemoveMin()
        {
            if (Count > 0)
                _storage.RemoveAt(0);
        }

        public T Select(int k)
        {
            if (k < 0 | k > Count) throw new ArgumentOutOfRangeException(nameof(k));

            return _storage[k];
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
