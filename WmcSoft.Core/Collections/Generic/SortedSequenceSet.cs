﻿#region Licence

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

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Represents a set of objects that is maintained in sorted order.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    public class SortedSequenceSet<T> : ISet<T>
    {
        #region Fields

        private List<T> _storage;
        private readonly IComparer<T> _comparer;

        #endregion

        #region Lifecycle

        public SortedSequenceSet(IComparer<T> comparer)
        {
            _storage = new List<T>();
            _comparer = comparer ?? Comparer<T>.Default;
        }

        public SortedSequenceSet()
            : this(Comparer<T>.Default)
        {
        }

        public SortedSequenceSet(IEnumerable<T> enumerable, IComparer<T> comparer = null)
            : this(comparer)
        {
            _storage.AddRange(enumerable);
            _storage.Sort(_comparer);
        }

        #endregion

        #region Properties

        public IComparer<T> Comparer => _comparer;

        /// <summary>
        /// Gets the minimum value, as defined by the comparer.
        /// </summary>
        /// <remarks>Returns default(T) when the set is empty.</remarks>
        public T Min {
            get {
                if (_storage.Count == 0)
                    return default;
                return _storage[1];
            }
        }

        /// <summary>
        /// Gets the maximum value, as defined by the comparer.
        /// </summary>
        /// <remarks>Returns default(T) when the set is empty.</remarks>
        public T Max {
            get {
                if (_storage.Count == 0)
                    return default;
                return _storage[_storage.Count - 1];
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Removes all elements that match the conditions defined by the specified predicate from a set.
        /// </summary>
        /// <param name="match">The delegate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of elements that were removed from the set.</returns>
        public int RemoveWhere(Predicate<T> match)
        {
            return _storage.RemoveAll(match);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> that iterates over the Set in reverse order.
        /// </summary>
        /// <returns>An enumerator that iterates over the Set in reverse order.</returns>
        public IEnumerable<T> Reverse()
        {
            return _storage.Backwards();
        }

        #endregion

        #region ISet<T> Membres

        /// <summary>
        /// Adds an element to the set and returns a value that indicates if it was successfully added.
        /// </summary>
        /// <param name="item">The element to add to the set.</param>
        /// <returns>true if item is added to the set; otherwise, false. </returns>
        public bool Add(T item)
        {
            int index = _storage.BinarySearch(item, _comparer);
            if (index >= 0)
                return false;
            _storage.Insert(~index, item);
            return true;
        }

        /// <summary>
        /// Removes all elements that are in a specified collection from the current Set.
        /// </summary>
        /// <param name="other">The collection of items to remove from the Set.</param>
        public void ExceptWith(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            foreach (var item in other) {
                Remove(item);
            }
        }

        /// <summary>
        /// Modifies the current Set so that it contains only elements that are also in a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set.</param>
        public void IntersectWith(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

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

        /// <summary>
        /// Determines whether a Set is a proper subset of the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set.</param>
        /// <returns>true if the Set is a proper subset of other; otherwise, false.</returns>
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            var tmp = new SortedSequenceSet<T>(other, Comparer);
            foreach (var item in this) {
                if (!tmp.Remove(item))
                    return false;
            }
            return tmp.Count != 0;
        }

        /// <summary>
        /// Determines whether a Set is a proper superset of the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set. </param>
        /// <returns>true if the Set is a proper superset of other; otherwise, false.</returns>
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            var tmp = new SortedSequenceSet<T>(other, Comparer);
            return tmp.IsProperSubsetOf(this);
        }

        /// <summary>
        /// Determines whether a Set is a subset of the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set.</param>
        /// <returns>true if the current Set is a subset of other; otherwise, false.</returns>
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            var tmp = new SortedSequenceSet<T>(other, Comparer);
            foreach (var item in this) {
                if (!tmp.Remove(item))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether a Set is a superset of the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set. </param>
        /// <returns>true if the Set is a superset of other; otherwise, false.</returns>
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            var tmp = new SortedSequenceSet<T>(other, Comparer);
            return tmp.IsSubsetOf(this);
        }

        /// <summary>
        /// Determines whether the current Set and a specified collection share common elements.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set.</param>
        /// <returns>true if the Set and other share at least one common element; otherwise, false.</returns>
        public bool Overlaps(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

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

        /// <summary>
        /// Determines whether the current Set and the specified collection contain the same elements.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set.</param>
        /// <returns>true if the current Set is equal to other; otherwise, false.</returns>
        public bool SetEquals(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            var tmp = new SortedSequenceSet<T>(this, Comparer);
            foreach (var item in other) {
                if (!tmp.Remove(item))
                    return false;
            }
            return tmp.Count == 0;
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            var tmp = new SortedSequenceSet<T>(other, Comparer);
            foreach (var item in tmp) {
                if (!Remove(item))
                    Add(item);
            }
        }

        /// <summary>
        /// Modifies the current Set so that it contains all elements that are present 
        /// in either the current object or the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set.</param>
        public void UnionWith(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

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

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        /// <summary>
        /// Removes all elements from the set.
        /// </summary>
        public virtual void Clear()
        {
            _storage.Clear();
        }

        /// <summary>
        /// Determines whether the set contains a specific element.
        /// </summary>
        /// <param name="item">The element to locate in the set.</param>
        /// <returns>true if the set contains item; otherwise, false.</returns>
        public virtual bool Contains(T item)
        {
            return _storage.BinarySearch(item, _comparer) >= 0;
        }

        /// <summary>
        /// Copies the complete Set to a compatible one-dimensional array, starting at the specified array index.
        /// </summary>
        /// <param name="array">A one-dimensional array that is the destination of the elements copied from the set. The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _storage.CopyTo(array, arrayIndex);
        }

        public int Count => _storage.Count;

        public bool IsReadOnly => false;

        public bool Remove(T item)
        {
            int index = _storage.BinarySearch(item, _comparer);
            if (index < 0)
                return false;
            _storage.RemoveAt(index);
            return true;
        }

        #endregion

        #region IEnumerable<T> Membres

        public List<T>.Enumerator GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
