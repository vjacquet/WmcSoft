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
using System.Collections.ObjectModel;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Implements a <see cref="IDictionary{TKey, TValue}"/> for which the keys and values are stored in 
    /// two separate arrays.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <remarks>The dictionary assumes it has the sole ownership of the arrays.</remarks>
    public class ZipReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        [Serializable]
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly TKey[] _keys;
            private readonly TValue[] _values;
            private int _index;
            private KeyValuePair<TKey, TValue> _current;

            public Enumerator(TKey[] keys, TValue[] values)
            {
                _keys = keys;
                _values = values;
                _index = 0;
                _current = default(KeyValuePair<TKey, TValue>);
            }

            public KeyValuePair<TKey, TValue> Current { get { return _current; } }

            object IEnumerator.Current {
                get {
                    if (_index == 0 | _index >= _keys.Length)
                        throw new InvalidOperationException();
                    return Current;
                }
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_index < _keys.Length) {
                    _current = new KeyValuePair<TKey, TValue>(_keys[_index], _values[_index]);
                    _index++;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _index = -1;
            }
        }

        private readonly TKey[] _keys;
        private readonly TValue[] _values;

        public ZipReadOnlyDictionary(TKey[] keys, TValue[] values)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (keys.Length != values.Length) throw new ArgumentException();

            _keys = keys;
            _values = values;
        }

        #region IReadOnlyDictionary<TKey, TValue> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(_keys, _values);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <returns>
        /// The number of elements in the collection. 
        /// </returns>
        public int Count {
            get { return _keys.Length; }
        }

        /// <summary>
        /// Determines whether the read-only dictionary contains an element that has the specified key.
        /// </summary>
        /// <returns>
        /// true if the read-only dictionary contains an element that has the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool ContainsKey(TKey key)
        {
            return Array.IndexOf(_keys, key) >= 0;
        }

        /// <summary>
        /// Gets the value that is associated with the specified key.
        /// </summary>
        /// <returns>
        /// true if the object that implements the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2"/> interface contains an element that has the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            var found = Array.IndexOf(_keys, key);
            if (found >= 0) {
                value = _values[found];
                return true;
            }
            value = default(TValue);
            return false;
        }

        /// <summary>
        /// Gets the element that has the specified key in the read-only dictionary.
        /// </summary>
        /// <returns>
        /// The element that has the specified key in the read-only dictionary.
        /// </returns>
        /// <param name="key">The key to locate.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found. </exception>
        public TValue this[TKey key] {
            get {
                var found = Array.IndexOf(_keys, key);
                if (found >= 0)
                    return _values[found];
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Gets an enumerable collection that contains the keys in the read-only dictionary. 
        /// </summary>
        /// <returns>
        /// An enumerable collection that contains the keys in the read-only dictionary.
        /// </returns>
        public ReadOnlyCollection<TKey> Keys {
            get { return Array.AsReadOnly(_keys); }
        }

        /// <summary>
        /// Gets an enumerable collection that contains the values in the read-only dictionary.
        /// </summary>
        /// <returns>
        /// An enumerable collection that contains the values in the read-only dictionary.
        /// </returns>
        public ReadOnlyCollection<TValue> Values {
            get { return Array.AsReadOnly(_values); }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys { get { return Keys; } }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values { get { return Values; } }

        #endregion
    }
}
