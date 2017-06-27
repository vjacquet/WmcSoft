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
    /// Compares two <see cref="IEnumerable{T}"/> in lexicographical order.
    /// </summary>
    /// <typeparam name="T">The type of elements of the <see cref="IEnumerable{T}"/> to compare.</typeparam>
    [Serializable]
    public sealed class LexicographicalComparer<T> : IComparer<IEnumerable<T>>
    {
        #region Fields

        private readonly IComparer<T> _comparer;

        #endregion

        #region Lifecycle

        public LexicographicalComparer(IComparer<T> comparer = null)
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        #endregion

        #region IComparer Members

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
        public int Compare(IEnumerable<T> x, IEnumerable<T> y)
        {
            if (ReferenceEquals(x, y))
                return 0;
            if (x == null)
                return 1;
            if (y == null)
                return -1;

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
        }

        #endregion
    }
}