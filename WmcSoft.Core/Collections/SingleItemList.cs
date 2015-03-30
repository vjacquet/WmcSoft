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

namespace WmcSoft.Collections
{
    /// <summary>
    /// Implementation of a list that contains only one item.
    /// </summary>
    public class SingleItemList : IList
    {
        #region Fields

        object _item;

        #endregion

        #region Lifecycle

        public SingleItemList(object value) {
            _item = value;
        }

        #endregion

        #region IList Membres

        public int Add(object value) {
            throw new NotSupportedException();
        }

        public void Clear() {
            throw new NotSupportedException();
        }

        public bool Contains(object value) {
            return _item == null
                ? value == null
                : _item.Equals(value);
        }

        public int IndexOf(object value) {
            return Contains(value) ? 0 : -1;
        }

        public void Insert(int index, object value) {
            throw new NotSupportedException();
        }

        public bool IsFixedSize {
            get { return true; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public void Remove(object value) {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index) {
            throw new NotSupportedException();
        }

        public object this[int index] {
            get {
                if (index != 0)
                    throw new ArgumentOutOfRangeException("index");
                return _item;
            }
            set {
                if (index != 0)
                    throw new ArgumentOutOfRangeException("index");
                _item = value;
            }
        }

        #endregion

        #region ICollection Membres

        public void CopyTo(Array array, int index) {
            array.SetValue(_item, index);
        }

        public int Count {
            get { return 1; }
        }

        public bool IsSynchronized {
            get { return false; }
        }

        public object SyncRoot {
            get { return this; }
        }

        #endregion

        #region IEnumerable Membres

        public IEnumerator GetEnumerator() {
            yield return _item;
        }

        #endregion
    }
}
