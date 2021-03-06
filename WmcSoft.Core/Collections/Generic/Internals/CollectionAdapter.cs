﻿#region Licence

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

namespace WmcSoft.Collections.Generic.Internals
{
    sealed class CollectionAdapter<T> : IReadOnlyCollection<T>, ICollection<T>
    {
        #region Fields

        private readonly int _count;
        private readonly IEnumerable<T> _enumerable;
        private readonly IEqualityComparer<T> _comparer;

        #endregion

        #region Lifecycle

        public CollectionAdapter(int count, IEnumerable<T> enumerable, IEqualityComparer<T> comparer = null)
        {
            _enumerable = enumerable;
            _count = count;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        #endregion

        #region IReadOnlyCollection<T> Members

        public int Count {
            get { return _count; }
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        #endregion

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            return _enumerable.Any(x => _comparer.Equals(x, item));
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in _enumerable)
                array[arrayIndex++] = item;
        }

        bool ICollection<T>.IsReadOnly {
            get { return true; }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
