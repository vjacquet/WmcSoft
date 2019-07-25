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
using System.Diagnostics;
using System.IO;
using System.Threading;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Collections.Specialized
{
    /// <summary>
    /// Represents a list of items with a moving gap to speed insertions and deletions in the "middle".
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    [Serializable]
    [DebuggerDisplay("Count = {Count,nq}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public sealed class GapList<T> : IList<T>, IReadOnlyList<T>, ICollection
    {
        // *************........................*******
        // ^            ^                       ^      ^
        // 0            |                       |     capacity
        //        gapStartIndex           gapEndIndex

        #region Enumerator

        [Serializable]
        public struct Enumerator : IEnumerator<T>
        {
            private readonly GapList<T> list;
            private readonly T[] storage;
            private readonly int version;
            private int index;
            private int count;
            private T current;

            internal Enumerator(GapList<T> list)
            {
                Debug.Assert(list != null);

                this.list = list;
                storage = list.storage;
                index = 0;
                count = list.gapStartIndex;
                version = list.version;
                current = default;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (version == list.version && index < count) {
                    current = storage[index++];
                    return true;
                } else if (version == list.version && index < list.gapEndIndex && list.gapEndIndex != storage.Length) {
                    index = list.gapEndIndex;
                    count = storage.Length;
                    current = storage[index++];
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                if (version != list.version)
                    throw new InvalidOperationException();
                index = 0;
                current = default;
                return false;
            }

            public T Current => current;

            object IEnumerator.Current {
                get {
                    if (index == 0 | index > storage.Length)
                        throw new InvalidOperationException();
                    return Current;
                }
            }

            void IEnumerator.Reset()
            {
                if (version != list.version)
                    throw new InvalidOperationException();
                index = -1;
                count = list.gapStartIndex;
                current = default(T);
            }
        }

        #endregion

        private static readonly T[] Empty = Array.Empty<T>();
        private const int DefaultCapacity = 4;

        private T[] storage;

        private int gapStartIndex;
        private int gapEndIndex;
        private int version;

        public GapList()
        {
            storage = Empty;
        }

        /// <summary>
        /// Constructs a <see cref="GapList{T}"/> with a given initial capacity. The list is
        /// initially empty, but will have room for the given number of elements
        /// before any reallocations are required.
        /// </summary>
        /// <param name="capacity">The required capacity.</param>
        public GapList(int capacity)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            storage = capacity == 0 ? Empty : new T[capacity];
            gapEndIndex = capacity;
        }

        /// <summary>
        /// Constructs a <see cref="GapList{T}"/>, copying the contents of the given collection. The
        /// size and capacity of the new list will both be equal to the size of the
        /// given <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">The items to add to the list.</param>
        public GapList(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentOutOfRangeException(nameof(collection));

            if (collection is ICollection<T> c) {
                var count = c.Count;
                if (count == 0) {
                    storage = Empty;
                } else {
                    storage = new T[count];
                    c.CopyTo(storage, 0);
                    gapStartIndex = count;
                    gapEndIndex = count;
                }
            } else {
                storage = Empty;
                using (var enumerator = collection.GetEnumerator()) {
                    while (enumerator.MoveNext()) {
                        Add(enumerator.Current);
                    }
                }
            }
        }

        private int Slide(int index)
        {
            if (index < gapStartIndex)
                return index;
            return index - gapStartIndex + gapEndIndex;
        }

        void Reserve(int grow)
        {
            var gap = gapEndIndex - gapStartIndex;
            if (gap < grow) {
                Capacity = Math.Max(2 * storage.Length, DefaultCapacity);
            }
        }

        public int Position {
            get { return gapStartIndex; }
            set { Seek(value, SeekOrigin.Begin); }
        }

        public int Seek(int offset, SeekOrigin origin)
        {
            var count = Count;
            switch (origin) {
            case SeekOrigin.Begin:
                break;
            case SeekOrigin.End:
                offset = count - offset;
                break;
            case SeekOrigin.Current:
            default:
                offset = gapStartIndex + offset;
                break;
            }
            if (offset < 0)
                offset = 0;
            else if (offset > Count)
                offset = count;
            Seek(offset);
            return gapStartIndex;
        }

        void Seek(int position)
        {
            version++; // always increment the version
            if (position < gapStartIndex) {
                // *****-----.....*****
                //      ^
                // *****.....-----*****
                var n = gapStartIndex - position;
                gapEndIndex = storage.UnguardedRotate(-n, position, gapEndIndex - position);
                gapStartIndex = position;
            } else if (position > gapStartIndex) {
                // ***********.....******
                //                    ^
                // **************.....***
                var n = position - gapStartIndex;
                var gap = gapEndIndex - gapStartIndex;
                gapEndIndex = storage.UnguardedRotate(n, gapStartIndex, n + gap) + gap;
                gapStartIndex = position;
            }
        }

        /// <summary>
        ///   Gets and sets the capacity of this list.  The capacity is the size of
        ///   the internal array used to hold items.  When set, the internal 
        ///   array of the list is reallocated to the given capacity.
        /// </summary>
        public int Capacity {
            get {
                return storage.Length;
            }
            set {
                var count = Count;
                if (value < count) throw new ArgumentOutOfRangeException(nameof(value));

                if (value != storage.Length) {
                    if (value > 0) {
                        var storage = new T[value];
                        if (count > 0) {
                            Array.Copy(this.storage, 0, storage, 0, gapStartIndex);
                            var n = this.storage.Length - gapEndIndex;
                            Array.Copy(this.storage, gapEndIndex, storage, storage.Length - n, n);
                            gapEndIndex = storage.Length - n;
                        } else {
                            gapEndIndex = storage.Length;
                        }
                        this.storage = storage;
                    } else {
                        storage = Empty;
                        gapEndIndex = 0;
                    }
                }
            }
        }

        public T this[int index] {
            get { return storage[Slide(index)]; }
            set { storage[Slide(index)] = value; }
        }

        public int Count => gapStartIndex + storage.Length - gapEndIndex;

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

        public bool IsReadOnly => false;

        /// <summary>
        /// Adds an item to the <see cref="GapList{T}"/>.
        /// </summary>
        /// <param name="item">The item to add to the <see cref="GapList{T}"/>.</param>
        public void Add(T item)
        {
            Seek(Count);
            Insert(item);
        }

        /// <summary>
        /// Removes all elements from the <see cref="GapList{T}"/>.
        /// </summary> 
        /// <remarks>This method is an O(n) when n is <see cref="Count"/>.</remarks>
        public void Clear()
        {
            version++;
            storage.UnguardedFill(default, gapStartIndex = 0, gapEndIndex = storage.Length); // no loitering
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="GapList{T}"/>.
        /// </summary>
        /// <param name="item">The item to locate in the <see cref="GapList{T}"/>. The value can be <c>null</c> for reference types.</param>
        /// <returns><c>true</c> if <paramref name="item"/> is found in the <see cref="GapList{T}"/>; otherwise <c>false</c>.</returns>
        /// <remarks>The same comparer as <see cref="Array.IndexOf"/> to locate the item.</remarks>
        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Copies the <see cref="GapList{T}"/> to a compatible one-dimensional array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="GapList{T}"/>. 
        /// The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(storage, 0, array, arrayIndex, gapStartIndex);
            Array.Copy(storage, gapEndIndex, array, arrayIndex + gapStartIndex, storage.Length - gapEndIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            Array.Copy(storage, 0, array, index, gapStartIndex);
            Array.Copy(storage, gapEndIndex, array, index + gapStartIndex, storage.Length - gapEndIndex);
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of a value.
        /// </summary>
        /// <param name="item">The object to locate.</param>
        /// <returns>The zero-based index of the first occurrence of <paramref name="item"/>, if found; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            var found = Array.IndexOf(storage, item, 0, gapStartIndex);
            if (found >= 0)
                return found;
            found = Array.IndexOf(storage, item, gapEndIndex, storage.Length - gapEndIndex);
            if (found >= 0)
                return found - (gapEndIndex - gapStartIndex);
            return -1;
        }

        /// <summary>
        /// Inserts an item at the current position into the <see cref="GapList{T}"/>.
        /// </summary>
        /// <param name="item">The item to add to the <see cref="GapList{T}"/>.</param>
        public void Insert(T item)
        {
            Reserve(1);
            version++;
            storage[gapStartIndex++] = item;
        }

        /// <summary>
        /// Inserts an element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted..</param>
        /// <param name="item">The object to insert.</param>
        public void Insert(int index, T item)
        {
            Seek(index);
            Insert(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the <see cref="GapList{T}"/>.
        /// </summary>
        /// <param name="item">The item to remove from the <see cref="GapList{T}"/>. The value can be <c>null</c> for reference types.</param>
        /// <returns><c>true</c> if <paramref name="item"/> is successfully removed; otherwise <c>false</c>.</returns>
        /// <remarks>The same comparer as <see cref="Array.IndexOf"/> to locate the item.</remarks>
        public bool Remove(T item)
        {
            var found = IndexOf(item);
            if (found < 0)
                return false;
            RemoveAt(found);
            return true;
        }

        /// <summary>
        /// Removes the element at the given index. The size of the list is decreased by one.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            Seek(index + 1); // seek increment the version
            gapStartIndex--;
            storage[gapStartIndex] = default; // no loitering
        }

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
    }
}
