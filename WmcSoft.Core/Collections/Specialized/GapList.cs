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
using WmcSoft.Collections.Generic;

namespace WmcSoft.Collections.Specialized
{
    /// <summary>
    /// Represents a list of items with a moving gap to speed insertions and deletions in the "middle".
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public sealed class GapList<T> : IList<T>
    {
        // *************........................*******
        // ^            ^                       ^      ^
        // 0            |                       |     capacity
        //       _gapStartIndex          _gapEndIndex

        #region Enumerator

        [Serializable]
        public struct Enumerator : IEnumerator<T>
        {
            private readonly GapList<T> _list;
            private readonly T[] _storage;
            private readonly int _version;
            private int _index;
            private int _count;
            private T _current;

            internal Enumerator(GapList<T> list) {
                Debug.Assert(list != null);

                _list = list;
                _storage = list._storage;
                _index = 0;
                _count = list._gapStartIndex;
                _version = list._version;
                _current = default(T);
            }

            public void Dispose() {
            }

            public bool MoveNext() {
                if (_version == _list._version && _index < _count) {
                    _current = _storage[_index++];
                    return true;
                } else if (_version == _list._version && _index < _list._gapEndIndex && _list._gapEndIndex != _storage.Length) {
                    _index = _list._gapEndIndex;
                    _count = _storage.Length;
                    _current = _storage[_index++];
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare() {
                if (_version != _list._version)
                    throw new InvalidOperationException();
                _index = 0;
                _current = default(T);
                return false;
            }

            public T Current { get { return _current; } }

            object IEnumerator.Current {
                get {
                    if (_index == 0 | _index >= _storage.Length)
                        throw new InvalidOperationException();
                    return Current;
                }
            }

            void IEnumerator.Reset() {
                if (_version != _list._version)
                    throw new InvalidOperationException();
                _index = -1;
                _count = _list._gapStartIndex;
                _current = default(T);
            }
        }

        #endregion

        private static readonly T[] Empty = new T[0];
        private const int DefaultCapacity = 4;

        private T[] _storage;

        private int _gapStartIndex;
        private int _gapEndIndex;
        private int _version;

        public GapList() {
            _storage = Empty;
        }

        public GapList(int capacity) {
            if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            _storage = capacity == 0 ? Empty : new T[capacity];
            _gapEndIndex = capacity;
        }

        public GapList(IEnumerable<T> collection) {
            if (collection == null) throw new ArgumentOutOfRangeException(nameof(collection));

            var c = collection as ICollection<T>;
            if (c != null) {
                var count = c.Count;
                if (count == 0) {
                    _storage = Empty;
                } else {
                    _storage = new T[count];
                    c.CopyTo(_storage, 0);
                    _gapStartIndex = count;
                    _gapEndIndex = count;
                }
            } else {
                _storage = Empty;
                using (IEnumerator<T> enumerator = collection.GetEnumerator()) {
                    while (enumerator.MoveNext()) {
                        Add(enumerator.Current);
                    }
                }
            }
        }

        private int Slide(int index) {
            if (index < _gapStartIndex)
                return index;
            return index - _gapStartIndex + _gapEndIndex;
        }

        void Reserve(int grow) {
            var gap = _gapEndIndex - _gapStartIndex;
            if (gap < grow) {
                Capacity = Math.Max(Count + gap, _storage.Length == 0 ? DefaultCapacity : _storage.Length * 2);
            }
        }

        public int Position {
            get { return _gapStartIndex; }
            set { Seek(value, SeekOrigin.Begin); }
        }

        public int Seek(int offset, SeekOrigin origin) {
            var count = Count;
            switch (origin) {
            case SeekOrigin.Begin:
                break;
            case SeekOrigin.Current:
                offset = _gapStartIndex + offset;
                break;
            case SeekOrigin.End:
                offset = count - offset;
                break;
            default:
                goto case SeekOrigin.Current;
            }
            if (offset < 0)
                offset = 0;
            else if (offset > Count)
                offset = count;
            Seek(offset);
            return _gapStartIndex;
        }

        void Seek(int position) {
            if (position < _gapStartIndex) {
                // *****-----.....*****
                //      ^
                // *****.....-----*****
                var n = _gapStartIndex - position;
                _gapEndIndex = _storage.Rotate(-n, position, _gapEndIndex - position);
                _gapStartIndex = position;

                _version++;
            } else if (position > _gapStartIndex) {
                // ***********.....******
                //                    ^
                // **************.....***
                var n = position - _gapStartIndex;
                var gap = _gapEndIndex - _gapStartIndex;
                _gapEndIndex = _storage.Rotate(n, _gapStartIndex, n + gap) + gap;
                _gapStartIndex = position;

                _version++;
            }
        }

        public int Capacity {
            get {
                return _storage.Length;
            }
            set {
                var count = Count;
                if (value < count) throw new ArgumentOutOfRangeException(nameof(value));

                if (value != _storage.Length) {
                    if (value > 0) {
                        T[] storage = new T[value];
                        if (count > 0) {
                            Array.Copy(_storage, 0, storage, 0, _gapStartIndex);
                            var n = _storage.Length - _gapEndIndex;
                            Array.Copy(_storage, _gapEndIndex, storage, storage.Length - n, n);
                            _gapEndIndex = storage.Length - n;
                        }
                        _storage = storage;
                    } else {
                        _storage = Empty;
                        _gapEndIndex = 0;
                    }
                }
            }
        }

        public T this[int index] {
            get { return _storage[Slide(index)]; }
            set { _storage[Slide(index)] = value; }
        }

        public int Count {
            get { return _gapStartIndex + _storage.Length - _gapEndIndex; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public void Add(T item) {
            Reserve(1);
            _storage[_gapStartIndex++] = item;
            _version++;
        }

        public void Clear() {
            _gapStartIndex = 0;
            _gapEndIndex = _storage.Length;
            _version++;
        }

        public bool Contains(T item) {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex) {
            Array.Copy(_storage, 0, array, arrayIndex, _gapStartIndex);
            Array.Copy(_storage, _gapEndIndex, array, arrayIndex + _gapStartIndex, _storage.Length - _gapEndIndex);
        }

        public Enumerator GetEnumerator() {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return GetEnumerator();
        }

        public int IndexOf(T item) {
            var found = Array.IndexOf(_storage, item, 0, _gapStartIndex);
            if (found >= 0)
                return found;
            var gap = _gapEndIndex - _gapStartIndex;
            return Array.IndexOf(_storage, item, _gapEndIndex, _storage.Length - _gapEndIndex) - gap;
        }

        public void Insert(int index, T item) {
            Seek(index);
            Add(item);
        }

        public bool Remove(T item) {
            var found = IndexOf(item);
            if (found < 0)
                return false;
            RemoveAt(found);
            return true;
        }

        public void RemoveAt(int index) {
            Seek(index + 1); // seek increment the version
            _gapStartIndex--;
            _storage[_gapStartIndex] = default; // no loitering
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
