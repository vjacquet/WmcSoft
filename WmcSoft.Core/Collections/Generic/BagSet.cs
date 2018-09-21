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
using System.Diagnostics;
using System.Linq;
using WmcSoft.Collections.Generic.Internals;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Represents a set in which the items are not ordered.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the bag.</typeparam>
    [DebuggerDisplay("Count = {Count,nq}")]
    [Serializable]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class BagSet<T> : ISet<T>
    {
        #region Enumerator

        [Serializable]
        public struct Enumerator : IEnumerator<T>
        {
            private readonly BagSet<T> bag;
            private readonly T[] storage;
            private readonly int version;
            private int index;
            private T current;

            internal Enumerator(BagSet<T> bag) : this(bag, bag.storage)
            {
            }

            internal Enumerator(BagSet<T> bag, T[] storage)
            {
                Debug.Assert(bag != null && storage != null && bag.Count <= storage.Length);

                this.bag = bag;
                this.storage = storage;
                index = this.bag.Count; // enumerates downward.
                version = bag.version;
                current = default;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (version == bag.version && index > 0) {
                    current = storage[--index];
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                if (version != bag.version)
                    throw new InvalidOperationException();
                index = -1;
                current = default;
                return false;
            }

            public T Current => current;

            object IEnumerator.Current {
                get {
                    if (index < 0 | index >= bag.Count)
                        throw new InvalidOperationException();
                    return current;
                }
            }

            void IEnumerator.Reset()
            {
                if (version != bag.version)
                    throw new InvalidOperationException();
                index = 0;
                current = default;
            }
        }

        #endregion

        private T[] storage;
        private int count;
        private int version;

        public BagSet()
            : this(EqualityComparer<T>.Default)
        {
        }

        public BagSet(IEqualityComparer<T> comparer)
        {
            storage = ContiguousStorage<T>.Empty;
            Comparer = comparer;
        }

        public BagSet(IEnumerable<T> enumerable, IEqualityComparer<T> comparer)
            : this(comparer)
        {
            storage = enumerable.Distinct(comparer).ToArray();
            count = storage.Length;
        }

        public BagSet(IEnumerable<T> enumerable)
            : this(enumerable, EqualityComparer<T>.Default)
        {
        }

        public IEqualityComparer<T> Comparer { get; }

        /// <summary>
        /// Removes all elements that match the conditions defined by the specified predicate from a set.
        /// </summary>
        /// <param name="match">The delegate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of elements that were removed from the set.</returns>
        public int RemoveWhere(Predicate<T> match)
        {
            int removed = 0;
            int i = count;

            version++;

            // quick remove at the end
            while (i > 0 && match(storage[--i])) {
                storage[--count] = default;
                removed++;
            }

            // move and remove
            while (i-- > 0) {
                if (match(storage[i])) {
                    removed++;
                    storage[i] = storage[--count];
                    storage[count] = default;
                }
            }
            return removed;
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> that iterates over the Set in reverse order.
        /// </summary>
        /// <returns>An enumerator that iterates over the Set in reverse order.</returns>
        public IEnumerable<T> Reverse()
        {
            return storage.Backwards();
        }

        #region ISet<T> Membres

        int FindIndex(T item)
        {
            return Array.FindIndex(storage, 0, count, x => Comparer.Equals(x, item));
        }

        /// <summary>
        /// Adds an element to the set and returns a value that indicates if it was successfully added.
        /// </summary>
        /// <param name="item">The element to add to the set.</param>
        /// <returns>true if item is added to the set; otherwise, false. </returns>
        public bool Add(T item)
        {
            if (FindIndex(item) >= 0) {
                version++;
                return false;
            }
            if (count == storage.Length)
                ContiguousStorage<T>.Reserve(ref storage, 1);
            storage[count++] = item;
            version++;
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

            version++;

            // handle empty cases
            if (storage.Length == 0)
                return;

            var tmp = new BagSet<T>(Comparer);
            foreach (var item in other) {
                if (Contains(item))
                    tmp.Add(item);
            }
            storage = tmp.storage;
            count = tmp.count;
        }

        /// <summary>
        /// Determines whether a Set is a proper subset of the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current Set.</param>
        /// <returns>true if the Set is a proper subset of other; otherwise, false.</returns>
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            var tmp = new HashSet<T>(other, Comparer);
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

            var tmp = new HashSet<T>(other, Comparer);
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

            var tmp = new HashSet<T>(other, Comparer);
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

            var tmp = new HashSet<T>(other, Comparer);
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
            if (storage.Length == 0)
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

            var tmp = new HashSet<T>(this, Comparer);
            foreach (var item in other) {
                if (!tmp.Remove(item))
                    return false;
            }
            return tmp.Count == 0;
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            var tmp = new HashSet<T>(other, Comparer);
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
            version++;
            for (int i = 0; i < count; i++) {
                storage[i] = default;
            }
            count = 0;
        }

        /// <summary>
        /// Determines whether the set contains a specific element.
        /// </summary>
        /// <param name="item">The element to locate in the set.</param>
        /// <returns>true if the set contains item; otherwise, false.</returns>
        public virtual bool Contains(T item)
        {
            return FindIndex(item) >= 0;
        }

        /// <summary>
        /// Copies the complete Set to a compatible one-dimensional array, starting at the specified array index.
        /// </summary>
        /// <param name="array">A one-dimensional array that is the destination of the elements copied from the set. The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            storage.CopyTo(array, arrayIndex, count);
        }

        public int Count => count;

        public bool IsReadOnly => false;

        public bool Remove(T item)
        {
            var index = FindIndex(item);
            version++;
            if (index < 0)
                return false;
            storage[index] = storage[--count];
            storage[count] = default;
            return true;
        }

        #endregion

        #region IEnumerable<T> Membres

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
