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

using System;
using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    [Serializable]
    public sealed class LexicographicalComparer<T> : IComparer<IEnumerable<T>>
    {
        #region Fields

        readonly IComparer<T> _comparer;

        #endregion

        #region Lifecycle

        public LexicographicalComparer(IComparer<T> comparer) {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            _comparer = comparer;
        }

        #endregion

        #region IComparer Members

        public int Compare(IEnumerable<T> x, IEnumerable<T> y) {
            using (var enumerator1 = x.GetEnumerator())
            using (var enumerator2 = y.GetEnumerator()) {
                var hasValue1 = enumerator1.MoveNext();
                var hasValue2 = enumerator2.MoveNext();

                while (hasValue1 & hasValue2) {
                    int comparison = _comparer.Compare(enumerator1.Current, enumerator2.Current);
                    if (comparison != 0)
                        return comparison;
                    hasValue1 = enumerator1.MoveNext();
                    hasValue2 = enumerator2.MoveNext();
                }
                if (hasValue1)
                    return 1;
                if (hasValue2)
                    return -1;
                return 0;
            }

            #endregion
        }
    }
}
