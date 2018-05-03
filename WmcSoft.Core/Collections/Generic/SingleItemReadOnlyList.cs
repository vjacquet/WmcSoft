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

namespace WmcSoft.Collections.Generic
{
    public class SingleItemReadOnlyList<T> : IReadOnlyList<T>, IList<T>
    {
        #region Fields

        private T _item;

        #endregion

        #region Lifecycle

        public SingleItemReadOnlyList(T item)
        {
            _item = item;
        }

        #endregion

        #region IList<T> Membres

        public T this[int index] {
            get {
                if (index != 0) throw new ArgumentOutOfRangeException(nameof(index));

                return _item;
            }
            set {
                throw new NotSupportedException();
            }
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICollection<T> Membres

        public int Count => 1;

        public bool IsReadOnly => true;

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return EqualityComparer<T>.Default.Equals(_item, item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            array[arrayIndex] = _item;
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<T> Membres

        public IEnumerator<T> GetEnumerator()
        {
            yield return _item;
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
