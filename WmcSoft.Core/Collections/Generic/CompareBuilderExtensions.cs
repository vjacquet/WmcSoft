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
    /// <summary>
    /// Defines extension methods to build comparers.
    /// This is a static class. 
    /// </summary>
    public static class CompareBuilderExtensions
    {
        public static IComparer<T> By<T, U>(this ComparerBuilder<T> comparer, Func<T, U> selector) {
            return new SelectComparer<T, U>(selector);
        }

        public static IComparer<T> ThenBy<T, U>(this IComparer<T> comparer, Func<T, U> selector) {
            var then = new SelectComparer<T, U>(selector);
            return new CascadingComparer<T>(Enumerate(comparer), then);
        }

        public static IComparer<T> ByDescending<T, U>(this ComparerBuilder<T> comparer, Func<T, U> selector) {
            return new ReverseComparer<T>(new SelectComparer<T, U>(selector));
        }

        public static IComparer<T> ThenByDescending<T, U>(this IComparer<T> comparer, Func<T, U> selector) {
            var then = new ReverseComparer<T>(new SelectComparer<T, U>(selector));
            return new CascadingComparer<T>(Enumerate(comparer), then);
        }

        #region Helpers

        static IEnumerable<IComparer<T>> Enumerate<T>(IComparer<T> comparer) {
            var enumerable = comparer as IEnumerable<IComparer<T>>;
            return enumerable ?? EnumerableExtensions.AsEnumerable(comparer);
        }

        #endregion
    }

}
