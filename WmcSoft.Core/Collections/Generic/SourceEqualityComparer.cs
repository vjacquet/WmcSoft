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
    /// <summary>
    /// Implements an equality comparer which, given the index of elements in a source, compare the corresponding items.
    /// </summary>
    /// <typeparam name="T">The type of the source's items to compare</typeparam>
    public class SourceEqualityComparer<T> : IEqualityComparer<int>
    {
        readonly IReadOnlyList<T> _list;
        readonly IEqualityComparer<T> _comparer;

        public SourceEqualityComparer(IReadOnlyList<T> list, IEqualityComparer<T> comparer = null) {
            if (list == null) throw new ArgumentNullException(nameof(list));

            _list = list;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        #region IEqualityComparer<int> Membres

        public bool Equals(int x, int y) {
            return _comparer.Equals(_list[x], _list[y]);
        }

        public int GetHashCode(int obj) {
            return _comparer.GetHashCode(_list[obj]);
        }

        #endregion
    }
}