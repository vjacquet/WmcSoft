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

        public Bag() {
            _storage = ContiguousStorage<T>.Empty;
        }

        public Bag(int capacity) {
            if (capacity < 0) throw new ArgumentOutOfRangeException("capacity");

            _storage = (capacity != 0)
                ? new T[capacity]
                : ContiguousStorage<T>.Empty;
        }

        public Bag(IEnumerable<T> collection) {
            _storage = collection.ToArray();
            _count = _storage.Length;
        }

        public int Count {
            get { return _count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public void Add(T item) {
            if (_count == _storage.Length)
                ContiguousStorage<T>.Reserve(ref _storage, 1);
            _storage[_count++] = item;
            _version++;
        }

        public void Clear() {
            _count = 0;
        }

        public bool Contains(T item) {
            return Array.IndexOf(_storage, item, 0, _count) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _storage.CopyTo(array, arrayIndex, _count);
        }

        public bool Remove(T item) {
            var index = Array.IndexOf(_storage, item, 0, _count);
            if (index < 0)
                return false;
            _version++;
            --_count;
            _storage[index] = _storage[_count];
            _storage[_count] = default(T);
            return true;
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