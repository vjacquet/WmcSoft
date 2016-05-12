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
using System.Collections.Generic;
using System.Linq;

namespace WmcSoft.Collections.Generic.Internals
{
    sealed class ReadOnlyCollectionFacade<T> : IReadOnlyCollection<T>, ICollection<T>
    {
        #region fields

        readonly ICollection<T> _collection;

        #endregion

        #region Lifecycle

        public ReadOnlyCollectionFacade(ICollection<T> collection) {
            _collection = collection;
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item) {
            throw new NotSupportedException();
        }

        public void Clear() {
            throw new NotSupportedException();
        }

        public bool Contains(T item) {
            var comparer = EqualityComparer<T>.Default;
            return _collection.Any(x => comparer.Equals(x, item));
        }

        public void CopyTo(T[] array, int arrayIndex) {
            foreach (var item in _collection)
                array[arrayIndex++] = item;
        }

        public bool IsReadOnly {
            get { return true; }
        }

        public bool Remove(T item) {
            throw new NotSupportedException();
        }

        #endregion

        #region IReadOnlyCollection<T> Membres

        public int Count {
            get { return _collection.Count; }
        }

        #endregion

        #region IEnumerable<T> Membres

        public IEnumerator<T> GetEnumerator() {
            return _collection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
