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

using System.Collections;
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Represents a generic collection of key/value pair over a readonly underlying dictionary;
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <remarks>The underlying dictionary is copied only when the first write operation occurs.</remarks>
    public class CopyOnWriteDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _source;
        private IDictionary<TKey, TValue> _inner;
        private readonly IEqualityComparer<TKey> _comparer;

        public CopyOnWriteDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) {
            _inner = _source = dictionary;
            _comparer = comparer;
        }

        public CopyOnWriteDictionary(IDictionary<TKey, TValue> dictionary)
            : this(dictionary, EqualityComparer<TKey>.Default) {
        }

        public CopyOnWriteDictionary(Dictionary<TKey, TValue> dictionary)
            : this(dictionary, dictionary.Comparer) {
        }

        private IDictionary<TKey, TValue> Readable {
            get { return _inner; }
        }

        private IDictionary<TKey, TValue> Writtable {
            get {
                if (ReferenceEquals(_inner, _source))
                    _inner = new Dictionary<TKey, TValue>(_source, _comparer);
                return _inner;
            }
        }

        public ICollection<TKey> Keys {
            get { return Readable.Keys; }
        }

        public ICollection<TValue> Values {
            get { return Readable.Values; }
        }

        public int Count {
            get { return Readable.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public TValue this[TKey key] {
            get { return Readable[key]; }
            set { Writtable[key] = value; }
        }

        public bool ContainsKey(TKey key) {
            return Readable.ContainsKey(key);
        }

        public void Add(TKey key, TValue value) {
            Writtable.Add(key, value);
        }

        public bool Remove(TKey key) {
            return Writtable.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value) {
            return Readable.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item) {
            Writtable.Add(item);
        }

        public void Clear() {
            Writtable.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            return Readable.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            Readable.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) {
            return Writtable.Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return Readable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Readable.GetEnumerator();
        }
    }
}