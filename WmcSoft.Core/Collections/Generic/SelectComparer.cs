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
    /// Implements a comparer that applies a selector before comparing the returned value.
    /// </summary>
    /// <typeparam name="TSource">The type of the source's items to compare.</typeparam>
    /// <typeparam name="TReturn">The type of the value returned by the selector.</typeparam>
    public class SelectComparer<TSource, TReturn> : IComparer<TSource>
    {
        private readonly Func<TSource, TReturn> _selector;
        private readonly IComparer<TReturn> _comparer;

        public SelectComparer(Func<TSource, TReturn> selector, IComparer<TReturn> comparer = null) {
            _selector = selector;
            _comparer = comparer ?? Comparer<TReturn>.Default;
        }

        #region IComparer<TSource> Membres

        int IComparer<TSource>.Compare(TSource x, TSource y) {
            return _comparer.Compare(_selector(x), _selector(y));
        }

        #endregion
    }
}
