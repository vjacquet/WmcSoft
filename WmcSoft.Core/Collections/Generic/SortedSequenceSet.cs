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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Represents a set of objects that is maintained in sorted order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    [DebuggerTypeProxy(typeof(SortedSequenceDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    public class SortedSequenceSet<T> : ISet<T>
    {
        #region Fields

        private List<T> _storage;
        private readonly IComparer<T> _comparer;

        #endregion

        #region Lifecycle

        public SortedSequenceSet(IComparer<T> comparer) {
            _storage = new List<T>();
            _comparer = comparer;
        }

        public SortedSequenceSet()
            : this(Comparer<T>.Default) {
        }

        public SortedSequenceSet(IEnumerable<T> enumerable, IComparer<T> comparer)
            : this(comparer) {
            _storage.AddRange(enumerable);
            _storage.Sort(comparer);
        }

        public SortedSequenceSet(IEnumerable<T> enumerable)
            : this(enumerable, Comparer<T>.Default) {
        }

        #endregion

        #region Properties

        public IComparer<T> Comparer {
            get { return _comparer; }
        }

        public T Min {
            get {
                if (_storage.Count == 0)
                    return default(T);
                return _storage[1];
            }
        }

        public T Max {
            get {
                if (_storage.Count == 0)
                    return default(T);
                return _storage[_storage.Count - 1];
            }
        }

        #endregion

        #region ISet<T> Membres

        public bool Add(T item) {
            int index = _storage.BinarySearch(item, _comparer);
            if (index >= 0)
                return false;
            _storage.Insert(~index, item);
            return true;
        }

        public void ExceptWith(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            foreach (var item in other) {
                Remove(item);
            }
        }

        public void IntersectWith(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            // handle empty cases
            if (_storage.Count == 0)
                return;
            var traits = new EnumerableTraits<T>(other, Comparer);
            if (traits.HasCount && traits.Count == 0) {
                Clear();
                return;
            }
            if (traits.IsSorted) {
                if (!traits.IsSet)
                    other = SetUtilities.Unique(other, new EqualityComparerAdapter<T>(Comparer));

                var list = new List<T>(Math.Min(traits.Count, Count));
                list.AddRange(SetUtilities.Intersection(this, other));
                _storage = list;
                return;
            }

            // do not know anything about the enumerable
            var tmp = new SortedSequenceSet<T>(Comparer);
            foreach (var item in other) {
                if (Contains(item))
                    tmp.Add(item);
            }
            _storage = tmp._storage;
        }

        public bool IsProperSubsetOf(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var tmp = new SortedSequenceSet<T>(other, Comparer);
            foreach (var item in this) {
                if (!tmp.Remove(item))
                    return false;
            }
            return tmp.Count != 0;
        }

        public bool IsProperSupersetOf(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var tmp = new SortedSequenceSet<T>(other, Comparer);
            return tmp.IsProperSubsetOf(this);
        }

        public bool IsSubsetOf(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var tmp = new SortedSequenceSet<T>(other, Comparer);
            foreach (var item in this) {
                if (!tmp.Remove(item))
                    return false;
            }
            return true;
        }

        public bool IsSupersetOf(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var tmp = new SortedSequenceSet<T>(other, Comparer);
            return tmp.IsSubsetOf(this);
        }

        public bool Overlaps(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            // handle empty cases
            if (_storage.Count == 0)
                return false;
            var traits = new EnumerableTraits<T>(other);
            if (traits.HasCount && traits.Count == 0)
                return false;

            foreach (var item in other) {
                if (Contains(item))
                    return true;
            }

            return false;
        }

        public bool SetEquals(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var tmp = new SortedSequenceSet<T>(this, Comparer);
            foreach (var item in other) {
                if (!tmp.Remove(item))
                    return false;
            }
            return tmp.Count == 0;
        }

        public void SymmetricExceptWith(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var tmp = new SortedSequenceSet<T>(other, Comparer);
            foreach (var item in tmp) {
                if (!Remove(item))
                    Add(item);
            }
        }

        public void UnionWith(IEnumerable<T> other) {
            if (other == null)
                throw new ArgumentNullException("other");

            var traits = new EnumerableTraits<T>(other, Comparer);
            if (traits.HasCount && traits.Count == 0) {
                return;
            }
            if (traits.IsSorted) {
                if (!traits.IsSet)
                    other = SetUtilities.Unique(other, new EqualityComparerAdapter<T>(Comparer));

                var list = new List<T>(traits.Count + Count);
                list.AddRange(SetUtilities.Union(this, other));
                _storage = list;
                return;
            }

            foreach (var item in other)
                Add(item);
        }

        #endregion

        #region ICollection<T> Membres

        void ICollection<T>.Add(T item) {
            Add(item);
        }

        public void Clear() {
            _storage.Clear();
        }

        public bool Contains(T item) {
            return _storage.BinarySearch(item, _comparer) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _storage.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return _storage.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(T item) {
            int index = _storage.BinarySearch(item, _comparer);
            if (index < 0)
                return false;
            _storage.RemoveAt(index);
            return true;
        }

        #endregion

        #region IEnumerable<T> Membres

        public IEnumerator<T> GetEnumerator() {
            return _storage.GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}