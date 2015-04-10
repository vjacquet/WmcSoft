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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Collections.Generic
{
    public class SingleItemList<T> : IList<T>
    {
        #region Fields

        private bool _hasValue;
        private T _item;

        #endregion

        #region Lifecycle

        public SingleItemList() {
        }

        public SingleItemList(T item) {
            _hasValue = true;
            _item = item;
        }

        #endregion

        #region IList<T> Membres

        public int IndexOf(T item) {
            return Contains(item) ? 0 : -1;
        }

        public void Insert(int index, T item) {
            if (_hasValue | index != 0)
                throw new ArgumentOutOfRangeException("index");
            _item = item;
        }

        public void RemoveAt(int index) {
            if (index != 0)
                throw new ArgumentOutOfRangeException("index");
            _hasValue = false;
        }

        public T this[int index] {
            get {
                if (!_hasValue | index != 0)
                    throw new ArgumentOutOfRangeException("index");
                return _item;
            }
            set {
                if (!_hasValue | index != 0)
                    throw new ArgumentOutOfRangeException("index");
                _item = value;
            }
        }

        #endregion

        #region ICollection<T> Membres

        public void Add(T item) {
            if (_hasValue)
                throw new NotSupportedException();
            _item = item;
            _hasValue = true;
        }

        public void Clear() {
            _hasValue = false;
            _item = default(T);
        }

        public bool Contains(T item) {
            return _hasValue && _item.Equals(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            if (_hasValue)
                array[arrayIndex] = _item;
        }

        public int Count {
            get { return _hasValue ? 1 : 0; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(T item) {
            if (Contains(item)) {
                _hasValue = false;
                _item = default(T);
                return true;
            }
            return false;
        }

        #endregion

        #region IEnumerable<T> Membres

        public IEnumerator<T> GetEnumerator() {
            if (_hasValue)
                yield return _item;
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
