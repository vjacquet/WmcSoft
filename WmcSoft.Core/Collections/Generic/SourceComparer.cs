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

using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Implements a comparer which, given the index of elements in a source, compare the corresponding items.
    /// </summary>
    /// <typeparam name="T">The type of the source's items to compare</typeparam>
    public class SourceComparer<T> : IComparer<int>
    {
        private readonly IReadOnlyList<T> _list;
        private readonly IComparer<T> _comparer;

        public SourceComparer(IReadOnlyList<T> list, IComparer<T> comparer = null) {
            _list = list;
            _comparer = comparer ?? Comparer<T>.Default;
        }

        #region IComparer<int> Membres

        int IComparer<int>.Compare(int x, int y) {
            return _comparer.Compare(_list[x], _list[y]);
        }

        #endregion
    }
}
