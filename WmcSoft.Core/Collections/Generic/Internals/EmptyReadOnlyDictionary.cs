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
using System.Linq;

namespace WmcSoft.Collections.Generic.Internals
{
    sealed class EmptyReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, TValue>
    {
        static readonly EmptyReadOnlyList<TKey> EmptyKeys = new EmptyReadOnlyList<TKey>();
        static readonly EmptyReadOnlyList<TValue> EmptyValues = new EmptyReadOnlyList<TValue>();

        public TValue this[TKey key] {
            get { throw new KeyNotFoundException(); }
            set { throw new NotSupportedException(); }
        }

        public int Count {
            get { return 0; }
        }

        public bool IsReadOnly {
            get { return true; }
        }

        public ICollection<TKey> Keys {
            get { return EmptyKeys; }
        }

        public ICollection<TValue> Values {
            get { return EmptyValues; }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys {
            get { return Keys; }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values {
            get { return Values; }
        }

        public void Add(KeyValuePair<TKey, TValue> item) {
            throw new NotSupportedException();
        }

        public void Add(TKey key, TValue value) {
            throw new NotSupportedException();
        }

        public void Clear() {
            throw new NotSupportedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            return false;
        }

        public bool ContainsKey(TKey key) {
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) {
            throw new NotSupportedException();
        }

        public bool Remove(TKey key) {
            throw new NotSupportedException();
        }

        public bool TryGetValue(TKey key, out TValue value) {
            value = default(TValue);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
