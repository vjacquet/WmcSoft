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

namespace WmcSoft.Collections.Generic
{
    public class Index<TKey, TValue> : IIndex<TKey, TValue>
    {
        private readonly Dictionary<TKey, List<TValue>> _storage;
        private int _count;

        public Index() {
            _storage = new Dictionary<TKey, List<TValue>>();
        }

        public Index(IEqualityComparer<TKey> comparer) {
            _storage = new Dictionary<TKey, List<TValue>>(comparer);
        }

        public Index(int capacity, IEqualityComparer<TKey> comparer) {
            _storage = new Dictionary<TKey, List<TValue>>(capacity, comparer);
        }

        public IReadOnlyList<TValue> this[TKey key] {
            get { return _storage[key]; }
        }

        public int Count {
            get { return _count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public IEnumerable<TKey> Keys {
            get { return _storage.Keys; }
        }

        public IEnumerable<TValue> GetValues(TKey key) {
            List<TValue> list;
            if (_storage.TryGetValue(key, out list)) {
                return list.AsReadOnly();
            }
            return Enumerable.Empty<TValue>();
        }

        public bool Add(TKey key, TValue value) {
            List<TValue> list;
            if (!_storage.TryGetValue(key, out list)) {
                list = new List<TValue>();
                _storage.Add(key, list);
            }
            list.Add(value);
            ++_count;
            return true;
        }

        public bool Add(KeyValuePair<TKey, TValue> item) {
            return Add(item.Key, item.Value);
        }
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) {
            Add(item.Key, item.Value);
        }

        public void Clear() {
            _storage.Clear();
            _count = 0;
        }

        public bool ContainsKey(TKey key) {
            return _storage.ContainsKey(key);
        }

        public bool Contains(TKey key, TValue value) {
            List<TValue> list;
            if (_storage.TryGetValue(key, out list)) {
                return list.Contains(value);
            }
            return false;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            return Contains(item.Key, item.Value);
        }

        public int Remove(TKey key) {
            List<TValue> list;
            if (_storage.TryGetValue(key, out list)) {
                _storage.Remove(key);

                var removed = list.Count;
                _count -= removed;
                list.Clear();
                return removed;
            }
            return 0;
        }

        public bool Remove(TKey key, TValue value) {
            List<TValue> list;
            if (_storage.TryGetValue(key, out list) && list.Remove(value)) {
                if (list.Count == 0) {
                    _storage.Remove(key);
                }
                --_count;
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) {
            return Remove(item.Key, item.Value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            foreach (var kv in _storage)
                foreach (var value in kv.Value)
                    yield return new KeyValuePair<TKey, TValue>(kv.Key, value);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Copies the elements of the <see cref="Index{TKey, TValue}"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="Index{TKey, TValue}"/>. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Rank != 1) throw new ArgumentException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if ((arrayIndex + Count) > array.Length) throw new IndexOutOfRangeException();

            foreach (var kv in _storage) {
                foreach (var value in kv.Value) {
                    array[arrayIndex++] = new KeyValuePair<TKey, TValue>(kv.Key, value);
                }
            }
        }
    }
}