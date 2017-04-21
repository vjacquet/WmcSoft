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
using System.Collections.Generic;
using System.Linq;

namespace WmcSoft.Collections.Specialized
{
    /// <summary>
    /// Represents a bidirectional map that preserves the uniqueness of its values as well as that of its keys.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public class Bimap<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _index;
        private readonly Dictionary<TValue, TKey> _reverse;

        private Bimap(Dictionary<TKey, TValue> index, Dictionary<TValue, TKey> reverse)
        {
            _index = index;
            _reverse = reverse;
        }

        public Bimap(IEqualityComparer<TKey> keyComparer = null, IEqualityComparer<TValue> valueComparer = null)
            : this(new Dictionary<TKey, TValue>(keyComparer), new Dictionary<TValue, TKey>(valueComparer))
        {
        }

        public Bimap(int capacity, IEqualityComparer<TKey> keyComparer = null, IEqualityComparer<TValue> valueComparer = null)
            : this(new Dictionary<TKey, TValue>(capacity, keyComparer), new Dictionary<TValue, TKey>(capacity, valueComparer))
        {
        }

        public Bimap<TValue, TKey> Inverse()
        {
            return new Bimap<TValue, TKey>(_reverse, _index);
        }

        #region IDictionary<TKey, TValue> Members

        public void Add(TKey key, TValue value)
        {
            _index.Add(key, value);
            _reverse.Add(value, key);
        }

        public bool ContainsKey(TKey key)
        {
            return _index.ContainsKey(key);
        }

        public ICollection<TKey> Keys {
            get { return _index.Keys; }
        }

        public bool Remove(TKey key)
        {
            if (_index.TryGetValue(key, out TValue value)) {
                _index.Remove(key);
                return _reverse.Remove(value);
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _index.TryGetValue(key, out value);
        }

        public bool TryGetKey(TValue value, out TKey key)
        {
            return _reverse.TryGetValue(value, out key);
        }

        public ICollection<TValue> Values {
            get { return _index.Values; }
        }

        public TValue this[TKey key] {
            get {
                return _index[key];
            }
            set {
                if (_index.TryGetValue(key, out TValue oldValue)) {
                    try {
                        _reverse.Remove(oldValue);
                        _reverse.Add(value, key);
                        _index[key] = value;
                    } catch (Exception) {
                        _reverse[oldValue] = key; // rollback removal
                        throw;
                    }
                } else {
                    _reverse.Add(value, key);
                    _index[key] = value;
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _index.Clear();
            _reverse.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _index.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            var collection = ((ICollection<KeyValuePair<TKey, TValue>>)_index);
            collection.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return _index.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var collection = ((ICollection<KeyValuePair<TKey, TValue>>)_index);
            if (collection.Remove(item)) {
                _reverse.Remove(item.Value);
                return true;
            }
            return false;
        }

        #endregion

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            return _index.GetEnumerator();
        }

        #region IEnumerable<KeyValuePair<T,T>> Members

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// Represents a bidirectional map that preserves the uniqueness of its values as well as that of its keys.
    /// </summary>
    /// <typeparam name="T">The type of key and value.</typeparam>
    public class Bimap<T> : IDictionary<T, T>
    {
        private readonly Dictionary<T, T> _index;
        private readonly Dictionary<T, T> _reverse;

        private Bimap(Dictionary<T, T> index, Dictionary<T, T> reverse)
        {
            _index = index;
            _reverse = reverse;
        }

        public Bimap()
            : this(new Dictionary<T, T>(), new Dictionary<T, T>())
        {
        }

        public Bimap(IEqualityComparer<T> comparer)
            : this(new Dictionary<T, T>(comparer), new Dictionary<T, T>(comparer))
        {
        }

        public Bimap(int capacity, IEqualityComparer<T> comparer = null)
            : this(new Dictionary<T, T>(capacity, comparer), new Dictionary<T, T>(capacity, comparer))
        {
        }

        public Bimap<T> Inverse()
        {
            return new Bimap<T>(_reverse, _index);
        }

        #region IDictionary<T,T> Members

        public void Add(T key, T value)
        {
            _index.Add(key, value);
            _reverse.Add(value, key);
        }

        public bool ContainsKey(T key)
        {
            return _index.ContainsKey(key);
        }

        public ICollection<T> Keys {
            get { return _index.Keys; }
        }

        public bool Remove(T key)
        {
            if (_index.TryGetValue(key, out T value)) {
                _index.Remove(key);
                return _reverse.Remove(value);
            }
            return false;
        }

        public bool TryGetValue(T key, out T value)
        {
            return _index.TryGetValue(key, out value);
        }

        public bool TryGetKey(T value, out T key)
        {
            return _reverse.TryGetValue(value, out key);
        }

        public ICollection<T> Values {
            get { return _index.Values; }
        }

        public T this[T key] {
            get {
                return _index[key];
            }
            set {
                if (_index.TryGetValue(key, out T oldValue)) {
                    try {
                        _reverse.Remove(oldValue);
                        _reverse.Add(value, key);
                        _index[key] = value;
                    } catch (Exception) {
                        _reverse[oldValue] = key; // rollback removal
                        throw;
                    }
                } else {
                    _reverse.Add(value, key);
                    _index[key] = value;
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<T,T>> Members

        public void Add(KeyValuePair<T, T> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _index.Clear();
            _reverse.Clear();
        }

        public bool Contains(KeyValuePair<T, T> item)
        {
            return _index.Contains(item);
        }

        public void CopyTo(KeyValuePair<T, T>[] array, int arrayIndex)
        {
            var collection = ((ICollection<KeyValuePair<T, T>>)_index);
            collection.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return _index.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(KeyValuePair<T, T> item)
        {
            var collection = ((ICollection<KeyValuePair<T, T>>)_index);
            if (collection.Remove(item)) {
                _reverse.Remove(item.Value);
                return true;
            }
            return false;
        }

        #endregion

        public Dictionary<T, T>.Enumerator GetEnumerator()
        {
            return _index.GetEnumerator();
        }

        #region IEnumerable<KeyValuePair<T,T>> Members

        IEnumerator<KeyValuePair<T, T>> IEnumerable<KeyValuePair<T, T>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}