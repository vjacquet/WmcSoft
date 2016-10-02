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
using WmcSoft.Collections.Generic.Internals;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Implements a generic collection of key/value pairs using a list.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public class ListDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Private fields

        private const int DefaultCapacity = 4;

        private readonly List<KeyValuePair<TKey, TValue>> _storage;
        private readonly IEqualityComparer<TKey> _comparer;

        #endregion

        #region Lifecycle

        public ListDictionary() : this(DefaultCapacity, null) {
        }

        public ListDictionary(int capacity) : this(capacity, null) {
        }

        public ListDictionary(IEqualityComparer<TKey> comparer) : this(DefaultCapacity, comparer) {
        }

        public ListDictionary(int capacity, IEqualityComparer<TKey> comparer) {
            if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));

            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            _storage = new List<KeyValuePair<TKey, TValue>>(capacity);
        }

        #endregion

        #region Properties

        public IEqualityComparer<TKey> Comparer { get { return _comparer; } }

        #endregion

        #region IDictionary<TKey, TValue> members

        public TValue this[TKey key] {
            get {
                var found = IndexOfKey(key);
                if (found < 0)
                    throw new KeyNotFoundException();
                return _storage[found].Value;
            }
            set {
                var found = IndexOfKey(key);
                if (found < 0)
                    _storage.Add(new KeyValuePair<TKey, TValue>(key, value));
                else
                    _storage[found] = new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        public int Count {
            get { return _storage.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public ICollection<TKey> Keys {
            get { return new CollectionAdapter<TKey>(_storage.Count, _storage.Select(p => p.Key), _comparer); }
        }

        public ICollection<TValue> Values {
            get { return new CollectionAdapter<TValue>(_storage.Count, _storage.Select(p => p.Value)); }
        }

        public void Add(KeyValuePair<TKey, TValue> item) {
            if (ContainsKey(item.Key))
                throw new ArgumentException();
            _storage.Add(item);
        }

        public void Add(TKey key, TValue value) {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public void Clear() {
            _storage.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            var found = IndexOfKey(item.Key);
            return (found >= 0 && EqualityComparer<TValue>.Default.Equals(item.Value, _storage[found].Value));
        }

        public bool ContainsKey(TKey key) {
            var found = IndexOfKey(key);
            return (found >= 0);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            _storage.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return _storage.GetEnumerator();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) {
            var found = IndexOfKey(item.Key);
            if (found >= 0 && EqualityComparer<TValue>.Default.Equals(item.Value, _storage[found].Value)) {
                _storage.RemoveAt(found);
                return true;
            }
            return false;
        }

        public bool Remove(TKey key) {
            var found = IndexOfKey(key);
            if (found < 0)
                return false;
            _storage.RemoveAt(found);
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value) {
            var found = IndexOfKey(key);
            if (found < 0) {
                value = default(TValue);
                return false;
            }
            value = _storage[found].Value;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _storage.GetEnumerator();
        }

        #endregion

        #region Helpers

        int IndexOfKey(TKey key) {
            if (key == null) throw new ArgumentNullException("key");
            return _storage.FindIndex(p => _comparer.Equals(key, p.Key));
        }

        #endregion
    }
}
