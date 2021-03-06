﻿#region Licence

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
    /// Implements a generic mutable collection of key/value pairs for wich the key can have multiple values.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the mutable index.</typeparam>
    /// <typeparam name="TValue">The type of values in the mutable index.</typeparam>
    public partial class Index<TKey, TValue> : IIndex<TKey, TValue>
    {
        private readonly Dictionary<TKey, ICollection<TValue>> _storage;
        private int _count;

        public Index()
        {
            _storage = new Dictionary<TKey, ICollection<TValue>>();
        }

        public Index(IEqualityComparer<TKey> comparer)
        {
            _storage = new Dictionary<TKey, ICollection<TValue>>(comparer);
        }

        public Index(int capacity, IEqualityComparer<TKey> comparer)
        {
            _storage = new Dictionary<TKey, ICollection<TValue>>(capacity, comparer);
        }

        public IReadOnlyCollection<TValue> this[TKey key] {
            get {
                if (_storage.TryGetValue(key, out ICollection<TValue> list))
                    return list.AsReadOnly();
                return EmptyReadOnlyList<TValue>.Instance;
            }
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

        public IEnumerable<TValue> Values {
            get { return _storage.Values.SelectMany(x => x); }
        }

        public bool Add(TKey key, TValue value)
        {
            if (!_storage.TryGetValue(key, out ICollection<TValue> list)) {
                list = new List<TValue>();
                _storage.Add(key, list);
            }
            list.Add(value);
            ++_count;
            return true;
        }

        public bool Add(KeyValuePair<TKey, TValue> item)
        {
            return Add(item.Key, item.Value);
        }
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _storage.Clear();
            _count = 0;
        }

        public bool ContainsKey(TKey key)
        {
            return _storage.ContainsKey(key);
        }

        public bool Contains(TKey key, TValue value)
        {
            if (_storage.TryGetValue(key, out ICollection<TValue> list)) {
                return list.Contains(value);
            }
            return false;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return Contains(item.Key, item.Value);
        }

        public int Remove(TKey key)
        {
            if (_storage.TryGetValue(key, out ICollection<TValue> list)) {
                _storage.Remove(key);

                var removed = list.Count;
                _count -= removed;
                list.Clear();
                return removed;
            }
            return 0;
        }

        public bool Remove(TKey key, TValue value)
        {
            if (_storage.TryGetValue(key, out ICollection<TValue> list) && list.Remove(value)) {
                if (list.Count == 0) {
                    _storage.Remove(key);
                }
                --_count;
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key, item.Value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var kv in _storage)
                foreach (var value in kv.Value)
                    yield return new KeyValuePair<TKey, TValue>(kv.Key, value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Copies the elements of the <see cref="Index{TKey, TValue}"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="Index{TKey, TValue}"/>. 
        /// The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
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

        public ILookup<TKey, TValue> Lookup {
            get {
                return new LookupAdapter(_storage);
            }
        }

        class LookupAdapter : ILookup<TKey, TValue>
        {
            private readonly IDictionary<TKey, ICollection<TValue>> _index;

            public LookupAdapter(IDictionary<TKey, ICollection<TValue>> index)
            {
                _index = index;
            }

            public IEnumerable<TValue> this[TKey key] => _index[key];

            public int Count => _index.Count;

            public bool Contains(TKey key) => _index.ContainsKey(key);

            public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
            {
                foreach (var kv in _index)
                    yield return new Grouping<TKey, TValue>(kv.Key, kv.Value.AsReadOnly());
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

    }
}
