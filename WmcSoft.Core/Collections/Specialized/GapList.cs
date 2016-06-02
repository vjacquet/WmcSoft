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
using System.IO;

namespace WmcSoft.Collections.Specialized
{
    public sealed class GapList<T> : IList<T>
    {
        // *************........................*******
        // ^            ^                       ^      ^
        // 0     _gapStartIndex                 |     capacity
        //                               _gapEndIndex

        private static readonly T[] Empty = new T[0];
        private const int DefaultCapacity = 4;

        private T[] _storage;
        //private int _count;
        //private int _gapIndex;

        private int _gapStartIndex;
        private int _gapEndIndex;

        public GapList() {
            _storage = Empty;
        }

        public GapList(int capacity) {
            if (capacity < 0) throw new ArgumentOutOfRangeException("capacity");
            _storage = capacity == 0 ? Empty : new T[capacity];
            _gapEndIndex = capacity;
        }

        public GapList(IEnumerable<T> collection) {
            if (collection == null) throw new ArgumentOutOfRangeException("collection");

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

        private int Translate(int index) {
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
            get {
                return _gapStartIndex;
            }
            set {
                Seek(value, SeekOrigin.Begin);
            }
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
            } else if (position > _gapStartIndex) {
                // ***********.....******
                //                    ^
                // **************.....***
                var n = position - _gapStartIndex;
                var gap = _gapEndIndex - _gapStartIndex;
                _gapEndIndex = _storage.Rotate(n, _gapStartIndex, n + gap) + gap;
                _gapStartIndex = position;
            }
        }

        public int Capacity {
            get {
                return _storage.Length;
            }
            set {
                var count = Count;
                if (value < count) throw new ArgumentOutOfRangeException("value");

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
            get { return _storage[Translate(index)]; }
            set { _storage[Translate(index)] = value; }
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
        }

        public void Clear() {
            _gapStartIndex = 0;
            _gapEndIndex = _storage.Length;
        }

        public bool Contains(T item) {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex) {
            Array.Copy(_storage, 0, array, arrayIndex, _gapStartIndex);
            Array.Copy(_storage, _gapEndIndex, array, arrayIndex + _gapStartIndex, _storage.Length - _gapEndIndex);
        }

        public IEnumerator<T> GetEnumerator() {
            using (var enumerator = _storage.GetEnumerator(0, _gapStartIndex)) {
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
            }

            using (var enumerator = _storage.GetEnumerator(_gapEndIndex, _storage.Length - _gapEndIndex)) {
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
            }
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
            Seek(index + 1);
            _gapStartIndex--;
#if DEBUG
            _storage[_gapStartIndex] = default(T);
#endif
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
