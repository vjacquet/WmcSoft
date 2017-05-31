#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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
using WmcSoft.Collections.Generic;

namespace WmcSoft.Collections.Specialized
{
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class Ring<T> : ICollection<T>, IReadOnlyCollection<T>
    {
        #region Enumerator 

        public struct Enumerator : IEnumerator<T>
        {
            private readonly Ring<T> _ring;
            private readonly int _version;
            private int _index;
            private T _current;

            internal Enumerator(Ring<T> ring)
            {
                _ring = ring;
                _version = ring._version;
                _index = -1;
                _current = default(T);
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_version != _ring._version)
                    throw new InvalidOperationException();
                if (_index == _ring._count) {
                    return false;
                }
                _index++;
                if (_index == _ring._count) {
                    _current = default(T);
                    return false;
                }
                _current = _ring._array[(_ring._head + _index) % _ring._array.Length];
                return true;
            }

            public T Current { get { return _current; } }

            object IEnumerator.Current {
                get {
                    if (_index < 0 | _index >= _ring.Count)
                        throw new InvalidOperationException();
                    return Current;
                }
            }

            void IEnumerator.Reset()
            {
                if (_version != _ring._version)
                    throw new InvalidOperationException();
                _index = -1;
                _current = default(T);
            }
        }

        #endregion

        private T[] _array;
        private int _head;
        private int _tail;
        private int _count;
        private int _version;

        public Ring(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));

            _array = new T[capacity];
        }

        public int Capacity {
            get {
                return _array.Length;
            }
            set {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                if (value == _array.Length)
                    return;

                var array = new T[value];
                _count = Math.Min(_count, value);
                for (int i = 0; i < _count; i++) {
                    array[i] = _array[_head];
                    _head = (_head + 1) % _array.Length;
                }

                _array = array;
                _head = 0;
                _tail = _count - 1;
                _version++;
            }
        }

        public bool TryEnqueue(T item)
        {
            if (_count < Capacity) {
                _array[_tail] = item;
                _tail = (_tail + 1) % _array.Length;
                _count++;
                _version++;
                return true;
            } else {
                return false;
            }
        }

        public T Enqueue(T item)
        {
            if (TryEnqueue(item))
                return default(T);

            var overwritten = _array[_tail];
            _array[_tail] = item;
            _tail = (_tail + 1) % _array.Length;
            _head = (_head + 1) % _array.Length;
            _version++;
            return overwritten;
        }

        public T Dequeue()
        {
            if (_count == 0) throw new InvalidOperationException();

            var result = _array[_head];
            _array[_head] = default(T);
            _head = (_head + 1) % _array.Length;
            _count--;
            _version++;
            return result;
        }

        public T Peek()
        {
            if (_count == 0) throw new InvalidOperationException();

            return _array[_head];
        }

        public int Count {
            get { return _count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public void Add(T item)
        {
            Enqueue(item);
        }

        bool RemoveForwards(T item, int startIndex, int count)
        {
            var found = Array.IndexOf(_array, item, startIndex, count);
            if (found >= 0) {
                while (found != startIndex) {
                    _array[found] = _array[found - 1];
                    found--;
                }
                _array[found] = default(T);
                _count--;
                _head++;
                return true;
            }
            return false;
        }

        bool RemoveBackwards(T item, int startIndex, int count)
        {
            var found = Array.IndexOf(_array, item, startIndex, count);
            if (found >= 0) {
                var endIndex = startIndex + count - 1;
                while (found != endIndex) {
                    _array[found] = _array[found + 1];
                    found++;
                }
                _array[found] = default(T);
                _count--;
                _tail--;
                return true;
            }
            return false;
        }

        public bool Remove(T item)
        {
            if (_head < _tail) {
                return RemoveBackwards(item, _head, _count);
            } else {
                return RemoveForwards(item, _head, _array.Length - _head)
                    || RemoveBackwards(item, 0, _tail);
            }
        }

        public void Clear()
        {
            if (_count == 0) {
            } else if (_head < _tail) {
                Array.Clear(_array, _head, _count);
            } else {
                Array.Clear(_array, _head, _array.Length - _head);
                Array.Clear(_array, 0, _tail);
            }
            _head = 0;
            _tail = 0;
            _count = 0;
            _version++;
        }

        public bool Contains(T item)
        {
            if (_count == 0) {
                return false;
            } else if (_head < _tail) {
                return Array.IndexOf(_array, item, _head, _count) >= 0;
            } else {
                return Array.IndexOf(_array, item, _head, _array.Length - _head) >= 0
                    || Array.IndexOf(_array, item, 0, _tail) >= 0;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (_count == 0) {
            } else if (_head < _tail) {
                Array.Copy(_array, _head, array, arrayIndex, _count);
            } else {
                var segment = _array.Length - _head;
                Array.Copy(_array, _head, array, arrayIndex, segment);
                Array.Copy(_array, 0, array, arrayIndex + segment, _tail);
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}