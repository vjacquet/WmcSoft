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

        public void Clear() {
            _storage.Clear();
            _count = 0;
        }

        public bool ContainsKey(TKey key) {
            return _storage.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            foreach (var kv in _storage)
                foreach (var value in kv.Value)
                    yield return new KeyValuePair<TKey, TValue>(kv.Key, value);
        }

        public bool Remove(TKey key) {
            List<TValue> list;
            if (_storage.TryGetValue(key, out list)) {
                _count -= list.Count;
                return true;
            }
            return false;
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

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerable<TValue> GetValues(TKey key) {
            List<TValue> list;
            if (_storage.TryGetValue(key, out list)) {
                return list.AsReadOnly();
            }
            return Enumerable.Empty<TValue>();
        }
    }
}