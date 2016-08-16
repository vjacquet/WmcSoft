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
    /// Represents a bag of items.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the bag.</typeparam>
    public class Bag<T> : ICollection<T>
    {
        #region Enumerator

        public struct Enumerator : IEnumerator<T>
        {
            private Bag<T> _bag;
            private int _version;
            private int _index;
            private T _current;

            internal Enumerator(Bag<T> bag) {
                _bag = bag;
                _index = 0;
                _version = bag._version;
                _current = default(T);
            }

            public T Current { get { return _current; } }

            object IEnumerator.Current { get { return Current; } }

            public void Dispose() {
            }

            public bool MoveNext() {
                CheckVersion();

                if (_index < _bag._count) {
                    _current = _bag._storage[_index];
                    _index++;
                    return true;
                }

                _index = _bag._count + 1;
                _current = default(T);
                return false;
            }

            public void Reset() {
                _index = 0;
                _current = default(T);
            }

            void CheckVersion() {
                if (_version != _bag._version)
                    throw new InvalidOperationException();
            }
        }

        #endregion

        private T[] _storage;
        private int _count;
        private int _version;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bag{T}"/> class that is empty and 
        /// has the default initial capacity.
        /// </summary>
        public Bag() {
            _storage = ContiguousStorage<T>.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bag{T}"/> class that is empty and 
        /// has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new bag can initially store.</param>
        public Bag(int capacity) {
            if (capacity < 0) throw new ArgumentOutOfRangeException("capacity");

            _storage = (capacity != 0)
                ? new T[capacity]
                : ContiguousStorage<T>.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bag{T}"/> class that contains elements 
        /// copied from the specified collection and has sufficient capacity to accommodate 
        /// the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new bag.</param>
        public Bag(IEnumerable<T> collection) {
            _storage = collection.ToArray();
            _count = _storage.Length;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="Bag{T}"/>.
        /// </summary>
        public int Count {
            get { return _count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Bag{T}"/> is read-only.
        /// </summary>
        public bool IsReadOnly {
            get { return false; }
        }

        /// <summary>
        /// Adds an item to the <see cref="Bag{T}"/>.
        /// </summary>
        /// <param name="item">The item to add to the <see cref="Bag{T}"/>.</param>
        public void Add(T item) {
            if (_count == _storage.Length)
                ContiguousStorage<T>.Reserve(ref _storage, 1);
            _storage[_count++] = item;
            _version++;
        }

        /// <summary>
        /// Removes all elements from the <see cref="Bag{T}"/>.
        /// </summary> 
        /// <remarks>This method is an O(n) when n is <see cref="Count"/>.</remarks>
        public void Clear() {
            for (int i = 0; i < _count; i++) {
                _storage[i] = default(T);
            }
            _count = 0;
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="Bag{T}"/>.
        /// </summary>
        /// <param name="item">The item to locate in the <see cref="Bag{T}"/>. The value can be <c>null</c> for reference types.</param>
        /// <returns><c>true</c> it <paramref name="item"/> is found in the <see cref="Bag{T}"/>; otherwise <c>false</c>.</returns>
        /// <remarks>The same comparer as <see cref="Array.IndexOf"/> to locate the item.</remarks>
        public bool Contains(T item) {
            return Array.IndexOf(_storage, item, 0, _count) >= 0;
        }

        /// <summary>
        /// Copies the <see cref="Bag{T}"/> to a compatible one-dimensional array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="Bag{T}"/>. 
        /// The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex) {
            _storage.CopyTo(array, arrayIndex, _count);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the <see cref="Bag{T}"/>.
        /// </summary>
        /// <param name="item">The item to remove from the <see cref="Bag{T}"/>. The value can be <c>null</c> for reference types.</param>
        /// <returns><c>true</c> it <paramref name="item"/> is successfully removed; otherwise <c>false</c>.</returns>
        /// <remarks>The same comparer as <see cref="Array.IndexOf"/> to locate the item.</remarks>
        public bool Remove(T item) {
            var index = Array.IndexOf(_storage, item, 0, _count);
            if (index < 0)
                return false;
            _version++;
            _storage[index] = _storage[--_count];
            _storage[_count] = default(T);
            return true;
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of elements removed from the <see cref="Bag{T}"/>.</returns>
        public int RemoveAll(Predicate<T> match) {
            int removed = 0;
            int i = _count;

            // quick remove at the end
            while (i > 0 && match(_storage[--i])) {
                _storage[--_count] = default(T);
                removed++;
            }

            // move and remove
            while (i-- > 0) {
                if (match(_storage[i])) {
                    removed++;
                    _storage[i] = _storage[--_count];
                    _storage[_count] = default(T);
                }
            }
            if (removed > 0)
                _version++;
            return removed;
        }

        Enumerator GetEnumerator() {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Removes the item at the specified index and returns it.
        /// </summary>
        /// <param name="index">The index of the item to pop.</param>
        /// <returns>The item at the specified <paramref name="index"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index is not in the range [0, Count).</exception>
        protected T PopAt(int index) {
            Debug.Assert(index < _count);

            var item = _storage.Exchange(default(T), _count - 1, index);
            // strong guarantee when the index is not valid
            _count--;
            _version++;
            return item;
        }
    }
}