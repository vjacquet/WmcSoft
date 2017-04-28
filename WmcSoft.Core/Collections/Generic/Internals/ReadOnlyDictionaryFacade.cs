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

using System.Collections.Generic;
using System.Diagnostics;

namespace WmcSoft.Collections.Generic.Internals
{
    sealed class ReadOnlyDictionaryFacade<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        #region fields

        private readonly IDictionary<TKey, TValue> _underlying;

        #endregion

        #region Lifecycle

        public ReadOnlyDictionaryFacade(IDictionary<TKey, TValue> dictionary)
        {
            Debug.Assert(dictionary != null);

            _underlying = dictionary;
        }

        #endregion

        #region IReadOnlyDictionary<TKey,TValue> Members

        public bool ContainsKey(TKey key)
        {
            return _underlying.ContainsKey(key);
        }

        public IEnumerable<TKey> Keys {
            get { return _underlying.Keys; }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _underlying.TryGetValue(key, out value);
        }

        public IEnumerable<TValue> Values {
            get { return _underlying.Values; }
        }

        public TValue this[TKey key] {
            get { return _underlying[key]; }
        }

        #endregion

        #region IReadOnlyCollection<KeyValuePair<TKey,TValue>> Members

        public int Count {
            get { return _underlying.Count; }
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _underlying.GetEnumerator();
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