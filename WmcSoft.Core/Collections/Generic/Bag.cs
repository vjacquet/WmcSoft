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
using System.Threading;
using WmcSoft.Collections.Generic.Internals;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Represents a bag of items.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the bag.</typeparam>
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class Bag<T> : ICollection<T>, IReadOnlyCollection<T>, ICollection
    {
        #region Enumerator

        [Serializable]
        public struct Enumerator : IEnumerator<T>
        {
            private readonly Bag<T> bag;
            private readonly T[] storage;
            private readonly int version;
            private int index;
            private T current;

            internal Enumerator(Bag<T> bag) : this(bag, bag.storage)
            {
            }

            internal Enumerator(Bag<T> bag, T[] storage)
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Bag{T}"/> class that is empty and 
        /// has the default initial capacity.
        /// </summary>
        public Bag()
        {
            storage = ContiguousStorage<T>.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bag{T}"/> class that is empty and 
        /// has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new bag can initially store.</param>
        public Bag(int capacity)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));

            storage = (capacity != 0) ? new T[capacity] : ContiguousStorage<T>.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bag{T}"/> class that contains elements 
        /// copied from the specified collection and has sufficient capacity to accommodate 
        /// the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new bag.</param>
        public Bag(IEnumerable<T> collection)
        {
            storage = collection.ToArray();
            count = storage.Length;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="Bag{T}"/>.
        /// </summary>
        public int Count => count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Bag{T}"/> is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033", Justification = "Provided only for legacy.")]
        object ICollection.SyncRoot {
            get {
                if (_syncRoot == null)
                    Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null);
                return _syncRoot;
            }
        }
        [NonSerialized]
        private object _syncRoot;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033", Justification = "Provided only for legacy.")]
        bool ICollection.IsSynchronized => false;

        /// <summary>
        /// Adds an item to the <see cref="Bag{T}"/>.
        /// </summary>
        /// <param name="item">The item to add to the <see cref="Bag{T}"/>.</param>
        public void Add(T item)
        {
            version++;
            if (count == storage.Length)
                ContiguousStorage<T>.Reserve(ref storage, 1);
            storage[count++] = item;
        }

        /// <summary>
        /// Removes all elements from the <see cref="Bag{T}"/>.
        /// </summary> 
        /// <remarks>This method is an O(n) when n is <see cref="Count"/>.</remarks>
        public void Clear()
        {
            version++;
            for (int i = 0; i < count; i++) {
                storage[i] = default;
            }
            count = 0;
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="Bag{T}"/>.
        /// </summary>
        /// <param name="item">The item to locate in the <see cref="Bag{T}"/>. The value can be <c>null</c> for reference types.</param>
        /// <returns><c>true</c> if <paramref name="item"/> is found in the <see cref="Bag{T}"/>; otherwise <c>false</c>.</returns>
        /// <remarks>The same comparer as <see cref="Array.IndexOf"/> to locate the item.</remarks>
        public bool Contains(T item)
        {
            return Array.IndexOf(storage, item, 0, count) >= 0;
        }

        /// <summary>
        /// Copies the <see cref="Bag{T}"/> to a compatible one-dimensional array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="Bag{T}"/>. 
        /// The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            storage.CopyTo(array, arrayIndex, count);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            Array.Copy(storage, 0, array, index, count);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the <see cref="Bag{T}"/>.
        /// </summary>
        /// <param name="item">The item to remove from the <see cref="Bag{T}"/>. The value can be <c>null</c> for reference types.</param>
        /// <returns><c>true</c> if <paramref name="item"/> is successfully removed; otherwise <c>false</c>.</returns>
        /// <remarks>The same comparer as <see cref="Array.IndexOf"/> to locate the item.</remarks>
        public bool Remove(T item)
        {
            var index = Array.IndexOf(storage, item, 0, count);
            version++;
            if (index < 0)
                return false;
            storage[index] = storage[--count];
            storage[count] = default;
            return true;
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of elements removed from the <see cref="Bag{T}"/>.</returns>
        public int RemoveAll(Predicate<T> match)
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

        public virtual Enumerator GetEnumerator()
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

        /// <summary>
        /// Removes the item at the specified index and returns it.
        /// </summary>
        /// <param name="index">The index of the item to pop.</param>
        /// <returns>The item at the specified <paramref name="index"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index is not in the range [0, Count).</exception>
        protected T PopAt(int index)
        {
            Debug.Assert(index < count);

            var item = storage.Exchange(default(T), count - 1, index);
            // strong guarantee when the index is not valid
            count--;
            version++;
            return item;
        }

        /// <summary>
        /// Picks a random element.
        /// </summary>
        /// <param name="random">The pseudo-random generator to use.</param>
        /// <returns>A random element taken out of the bag.</returns>
        public T Pick(Random random)
        {
            var last = Count;
            return PopAt(random.Next(last));
        }

        /// <summary>
        /// Rearranges the items randomly in the bag.
        /// </summary>
        /// <param name="random">The pseudo-random generator to use.</param>
        public void Shake(Random random)
        {
            CollectionExtensions.UnguardedShuffle(storage, 0, Count, random);
        }
    }
}
