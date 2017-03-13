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
using System.Diagnostics;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Implements a comparer that applies a selector before comparing the returned value.
    /// </summary>
    /// <typeparam name="TSource">The type of the source's items to compare.</typeparam>
    /// <typeparam name="TReturn">The type of the value returned by the selector.</typeparam>
    public struct SelectComparer<TSource, TReturn> : IComparer<TSource>
    {
        private readonly Func<TSource, TReturn> _selector;
        private readonly IComparer<TReturn> _comparer;

        public SelectComparer(SelectorVoucher<TSource, TReturn> vouch, IComparer<TReturn> comparer = null) {
            Debug.Assert(vouch.SupportsNullArgument);

            _selector = vouch;
            _comparer = comparer ?? Comparer<TReturn>.Default;
        }

        public SelectComparer(Func<TSource, TReturn> selector, IComparer<TReturn> comparer = null) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            _selector = (x) => (x != null) ? selector(x) : default(TReturn);
            _comparer = comparer ?? Comparer<TReturn>.Default;
        }

        /// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
        /// <param name="x">The first instance to compare.</param>
        /// <param name="y">The second instance to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of x and y, as shown in the
        /// following table.
        /// <list type="table">
        /// <listheader>
        ///   <description>Value</description>
        ///   <description>Meaning</description>
        /// </listheader>
        /// <item>
        ///   <description>Less than zero</description>
        ///   <description>x is less than y.</description>
        /// </item>
        /// <item>
        ///   <description>Zero</description>
        ///   <description>x equals y.</description>
        /// </item>
        /// <item>
        ///   <description>Greater than zero</description>
        ///   <description>x is greater than y.</description>
        /// </item>
        /// </list>
        /// </returns>
        public int Compare(TSource x, TSource y) {
            return _comparer.Compare(_selector(x), _selector(y));
        }
    }
}