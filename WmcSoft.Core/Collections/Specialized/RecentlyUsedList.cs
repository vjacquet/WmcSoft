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

namespace WmcSoft.Collections.Specialized
{
    public class RecentlyUsedList<T> : ICollection<T>, IReadOnlyList<T>
    {
        private readonly List<T> _items = new List<T>();

        public T this[int index] {
            get { return _items[_items.Count - index - 1]; }
        }

        public int Count {
            get { return _items.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public void Add(T item) {
            _items.Remove(item);
            _items.Add(item);
        }

        public bool Remove(T item) {
            return _items.Remove(item);
        }

        public void Clear() {
            _items.Clear();
        }

        public bool Contains(T item) {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _items.CopyBackwardsTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator() {
            for (int i = _items.Count - 1; i >= 0; i--) {
                yield return _items[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}