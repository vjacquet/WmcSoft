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
    public sealed class EqualityComparerAdapter<T> : IEqualityComparer<T>
    {
        #region Fields

        private readonly IComparer<T> _comparer;

        #endregion

        #region Lifecycle

        public EqualityComparerAdapter(IComparer<T> comparer) {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            _comparer = comparer;
        }

        #endregion

        #region IEqualityComparer<T> Members

        public bool Equals(T x, T y) {
            return _comparer.Compare(x, y) == 0;
        }

        public int GetHashCode(T obj) {
            if (obj == null)
                return 0;
            return obj.GetHashCode();
        }

        #endregion
    }
}
