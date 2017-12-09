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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Represents a sequence of sorted items.
    /// </summary>
    /// <typeparam name="T">The type of items.</typeparam>
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    public class SortedSequence<T> : ICollection<T>
    {
        #region Fields

        private readonly List<T> _storage;
        private readonly IComparer<T> _comparer;

        #endregion

        #region Lifecycle

        public SortedSequence(IComparer<T> comparer)
        {
            _storage = new List<T>();
            _comparer = comparer ?? Comparer<T>.Default;
        }

        public SortedSequence()
            : this(null)
        {
        }

        #endregion

        #region Properties

        public IComparer<T> Comparer => _comparer;

        #endregion

        #region ICollection<T> Membres

        public void Add(T item)
        {
            int index = _storage.BinarySearch(item, _comparer);
            if (index >= 0) {
                do {
                    index++;
                } while (index < _storage.Count && _comparer.Compare(_storage[index], item) == 0);
                _storage.Insert(index, item);
            } else {
                _storage.Insert(~index, item);
            }
        }

        public void Clear()
        {
            _storage.Clear();
        }

        public bool Contains(T item)
        {
            return _storage.BinarySearch(item, _comparer) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _storage.CopyTo(array, arrayIndex);
        }

        public int Count => _storage.Count;

        public bool IsReadOnly => false;

        public bool Remove(T item)
        {
            int index = _storage.BinarySearch(item, _comparer);
            if (index < 0)
                return false;
            _storage.RemoveAt(index);
            return true;
        }

        #endregion

        #region IEnumerable<T> Membres

        public List<T>.Enumerator GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
