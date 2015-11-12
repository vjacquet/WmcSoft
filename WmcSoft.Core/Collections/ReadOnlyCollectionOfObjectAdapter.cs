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

using System;
using System.Collections;
using System.Collections.Generic;

namespace WmcSoft.Collections
{
    /// <summary>
    /// Adapts an <see cref="ICollection"/> as a <see cref="IReadOnlyCollection{object}"/>.
    /// </summary>
    public sealed class ReadOnlyCollectionOfObjectAdapter : ICollection<object>, IReadOnlyCollection<object>
    {
        #region fields

        readonly ICollection _collection;

        #endregion

        #region Lifecycle

        public ReadOnlyCollectionOfObjectAdapter(ICollection collection) {
            _collection = collection;
        }

        #endregion

        #region ICollection<object> Members

        public void Add(object item) {
            throw new NotSupportedException();
        }

        public void Clear() {
            throw new NotSupportedException();
        }

        public bool Contains(object item) {
            if (item == null) {
                foreach (var e in _collection)
                    if (e == null)
                        return true;
            } else {
                foreach (var e in _collection)
                    if (item.Equals(e))
                        return true;
            }
            return false;
        }

        public void CopyTo(object[] array, int arrayIndex) {
            foreach (var item in _collection)
                array[arrayIndex++] = item;
        }

        public bool IsReadOnly {
            get { return true; }
        }

        public bool Remove(object item) {
            throw new NotSupportedException();
        }

        #endregion

        #region IReadOnlyCollection<object> Members

        public int Count {
            get { return _collection.Count; }
        }

        #endregion

        #region IEnumerable<object> Members

        public IEnumerator<object> GetEnumerator() {
            foreach (var item in _collection)
                yield return item;
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return _collection.GetEnumerator();
        }

        #endregion
    }
}
