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
using System.Linq;

namespace WmcSoft.Collections.Specialized
{
    public class UnboundedSparseArray<T> : IList<T>, IReadOnlyList<T>
    {
        private readonly SortedDictionary<int, T> _indexes;
        private readonly T _defaultValue;

        public UnboundedSparseArray(T defaultValue = default(T))
        {
            _indexes = new SortedDictionary<int, T>();
            _defaultValue = defaultValue;
        }

        public T this[int index] {
            get {
                if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

                if (_indexes.TryGetValue(index, out T value))
                    return value;
                return _defaultValue;
            }
            set {
                if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
                _indexes[index] = value;
            }
        }

        public int Count { get { return MaxIndex + 1; } }

        int MinIndex { get { return _indexes.Keys.First(); } }

        int MaxIndex {
            get {
                using (var enumerator = _indexes.Keys.GetEnumerator()) {
                    if (enumerator.MoveNext()) {
                        var countdown = _indexes.Count;
                        while (--countdown != 0)
                            enumerator.MoveNext();
                        return enumerator.Current;
                    }
                    return 0;
                }
            }
        }

        public Pair<int> Extent {
            get {
                var lower = -1;
                var upper = -1;
                var length = _indexes.Count;
                switch (length) {
                case 0:
                    break;
                case 1:
                    lower = upper = MinIndex;
                    break;
                default:
                    using (var enumerator = _indexes.Keys.GetEnumerator()) {
                        enumerator.MoveNext();
                        lower = enumerator.Current;
                        while (--length != 0)
                            enumerator.MoveNext();
                        upper = enumerator.Current;
                    }
                    break;
                }
                return new Pair<int>(lower, upper);
            }
        }

        public bool IsReadOnly { get { return false; } }

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var count = Count;
            for (var i = 0; i < count; i++) {
                array[arrayIndex + i] = _defaultValue;
            }
            foreach (var entry in _indexes) {
                array[arrayIndex + entry.Key] = entry.Value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _indexes.Values.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            if (comparer.Equals(item, _defaultValue)) {
                var i = 0;
                foreach (var entry in _indexes) {
                    if (entry.Key != i || comparer.Equals(item, entry.Value))
                        return i;
                    i++;
                }
            } else {
                foreach (var entry in _indexes) {
                    if (comparer.Equals(item, entry.Value)) {
                        return entry.Key;
                    }
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
