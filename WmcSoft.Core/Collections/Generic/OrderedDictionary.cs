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
    public class OrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue>
    {
        static void ThrowNoSuchElement()
        {
            throw new InvalidOperationException("No such entry in the dictionary.");
        }

        private readonly List<TKey> _keys;
        private readonly List<TValue> _values;

        #region Lifecycle

        public OrderedDictionary(IComparer<TKey> comparer = null)
        {
            _keys = new List<TKey>();
            _values = new List<TValue>();
            Comparer = comparer ?? Comparer<TKey>.Default;
        }

        public OrderedDictionary(int capacity, IComparer<TKey> comparer = null)
        {
            _keys = new List<TKey>(capacity);
            _values = new List<TValue>(capacity);
            Comparer = comparer ?? Comparer<TKey>.Default;
        }

        public OrderedDictionary(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer = null)
        {
            _keys = new List<TKey>(dictionary.Count);
            _values = new List<TValue>(dictionary.Count);
            Comparer = comparer ?? Comparer<TKey>.Default;
            foreach (var kv in dictionary)
                Add(kv.Key, kv.Value);
        }

        #endregion

        public IComparer<TKey> Comparer { get; }

        public bool IsReadOnly => false;
        public int Count => _keys.Count;
        public IReadOnlyList<TKey> Keys => _keys;
        public IReadOnlyList<TValue> Values => _values;

        public bool ContainsKey(TKey key)
        {
            return _keys.BinarySearch(key) >= 0;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var found = _keys.BinarySearch(key);
            if (found < 0) {
                value = default;
                return false;
            }
            value = _values[found];
            return true;
        }

        public TValue this[TKey key] {
            get {
                var found = _keys.BinarySearch(key);
                if (found < 0)
                    throw new KeyNotFoundException();
                return _values[found];
            }
            set {

            }
        }

        /// <summary>
        /// The <see cref="KeyValuePair{TKey, TValue}"/> with the smallest key.
        /// </summary>
        public KeyValuePair<TKey, TValue> Min {
            get {
                if (Count == 0)
                    ThrowNoSuchElement();
                return UnguardedSelect(0);
            }
        }

        /// <summary>
        /// The <see cref="KeyValuePair{TKey, TValue}"/> with the largest key.
        /// </summary>
        /// <exception cref="InvalidOperationException">No such element exist in the list.</exception>
        public KeyValuePair<TKey, TValue> Max {
            get {
                if (Count == 0)
                    ThrowNoSuchElement();
                return UnguardedSelect(Count - 1);
            }
        }

        /// <summary>
        /// Largest key less than or equal to <paramref name="key"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">No such element exist in the list.</exception>
        public KeyValuePair<TKey, TValue> Floor(TKey key)
        {
            var found = _keys.BinarySearch(key, Comparer);
            if (found >= 0)
                return UnguardedSelect(found);
            return UnguardedSelect(~found - 1);
        }

        /// <summary>
        /// Smallest key greater than or equal to <paramref name="key"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">No such element exist in the list.</exception>
        public KeyValuePair<TKey, TValue> Ceiling(TKey key)
        {
            var found = _keys.BinarySearch(key, Comparer);
            if (found >= 0)
                return UnguardedSelect(found);
            return UnguardedSelect(~found);
        }

        /// <summary>
        /// Number of keys less than <paramref name="key"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">No such element exist in the list.</exception>
        public int Rank(TKey key)
        {
            return _keys.BinaryRank(key, Comparer);
        }

        /// <summary>
        /// Key of rank <paramref name="k"/>.
        /// </summary>
        public KeyValuePair<TKey, TValue> Select(int k)
        {
            if (k < 0 | k > Count) throw new ArgumentOutOfRangeException(nameof(k));

            return UnguardedSelect(k);
        }

        /// <summary>
        /// The number of values between the <paramref name="lo"/> and <paramref name="hi"/> keys.
        /// </summary>
        /// <param name="lo">The lowest key.</param>
        /// <param name="hi">The highest key</param>
        /// <returns>The number of values.</returns>
        /// <remarks><paramref name="lo"/> is included and <paramref name="hi"/> is excluded.</remarks>
        public int CountBetween(TKey lo, TKey hi)
        {
            return Rank(hi) - Rank(lo);
        }

        /// <summary>
        /// The values between the <paramref name="lo"/> and <paramref name="hi"/> keys.
        /// </summary>
        /// <param name="lo">The lowest key.</param>
        /// <param name="hi">The highest key</param>
        /// <returns>The values.</returns>
        /// <remarks><paramref name="lo"/> is included and <paramref name="hi"/> is excluded.</remarks>
        public IReadOnlyCollection<KeyValuePair<TKey, TValue>> EnumerateBetween(TKey lo, TKey hi)
        {
            var l = Rank(lo);
            var h = Rank(hi);
            var count = h - l;
            var enumerable = EnumerateRange(l, count);
            return new ReadOnlyCollectionAdapter<KeyValuePair<TKey, TValue>>(count, enumerable);
        }

        IEnumerable<KeyValuePair<TKey, TValue>> EnumerateRange(int startIndex, int count)
        {
            while (startIndex != count) {
                yield return UnguardedSelect(startIndex);
                startIndex++;
            }
        }

        /// <summary>
        /// Removes the minimum value.
        /// </summary>
        public void RemoveMin()
        {
            UnguardedRemoveAt(0);
        }

        /// <summary>
        /// Removes the maximum value.
        /// </summary>
        public void RemoveMax()
        {
            UnguardedRemoveAt(Count - 1);
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => _keys;
        ICollection<TValue> IDictionary<TKey, TValue>.Values => _values;

        private void UnguardedRemoveAt(int index)
        {
            _keys.RemoveAt(index);
            _values.RemoveAt(index);
        }

        public void Clear()
        {
            _keys.Clear();
            _values.Clear();
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var found = _keys.BinarySearch(key, Comparer);
            if (found >= 0)
                throw new ArgumentException();
            var index = ~found;
            _keys.Insert(index, key);
            _values.Insert(index, value);
        }

        public bool Remove(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var found = _keys.BinarySearch(key, Comparer);
            if (found < 0)
                return false;
            _keys.RemoveAt(found);
            _values.RemoveAt(found);
            return true;
        }

        private KeyValuePair<TKey, TValue> UnguardedSelect(int k)
        {
            return new KeyValuePair<TKey, TValue>(_keys[k], _values[k]);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            var found = _keys.BinarySearch(item.Key);
            if (found < 0)
                return false;
            return Comparer<TValue>.Default.Compare(_values[found], item.Value) == 0;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            var length = Count;
            for (int i = 0; i < length; i++, arrayIndex++) {
                array[arrayIndex] = UnguardedSelect(i);
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            var found = _keys.BinarySearch(item.Key);
            if (found >= 0 && Comparer<TValue>.Default.Compare(_values[found], item.Value) == 0) {
                _keys.RemoveAt(found);
                _values.RemoveAt(found);
                return true;
            }
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var length = Count;
            for (int i = 0; i < length; i++) {
                yield return UnguardedSelect(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
