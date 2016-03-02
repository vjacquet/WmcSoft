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

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Implements a comparer that calls comparer in sequence as long as they compare equals. 
    /// </summary>
    /// <typeparam name="T">The type of the source's items to compare</typeparam>
    [Serializable]
    public sealed class CascadingComparer<T> : IComparer<T>
        , IEnumerable<IComparer<T>>
    {
        readonly IComparer<T>[] _comparers;

        public CascadingComparer(params IComparer<T>[] comparers) {
            _comparers = comparers;
        }

        #region IComparer<T> Membres

        public int Compare(T x, T y) {
            int result = 0;
            for (int i = 0; i < _comparers.Length; i++) {
                result = _comparers[i].Compare(x, y);
                if (result != 0)
                    break;
            }
            return result;
        }

        #endregion

        #region IEnumerable<IComparer<T> members

        IEnumerator<IComparer<T>> IEnumerable<IComparer<T>>.GetEnumerator() {
            return ((IEnumerable<IComparer<T>>)_comparers).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<IComparer<T>>)_comparers).GetEnumerator();
        }

        #endregion
    }
}
