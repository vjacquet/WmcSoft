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
    /// Represents a generec read only collection for which the elements are sorted
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyOrderedCollection<T> : IReadOnlyCollection<T>
    {
        /// <summary>
        /// The comparer used to sort the keys.
        /// </summary>
        IComparer<T> Comparer { get; }

        /// <summary>
        /// The smalled element.
        /// </summary>
        /// <exception cref="InvalidOperationException">No such element exist in the list.</exception>
        T Min { get; }

        /// <summary>
        /// The largest element.
        /// </summary>
        /// <exception cref="InvalidOperationException">No such element exist in the list.</exception>
        T Max { get; }

        /// <summary>
        /// Largest element less than or equal to <paramref name="value"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">No such element exist in the list.</exception>
        T Floor(T value);

        /// <summary>
        /// Smallest element greater than or equal to <paramref name="value"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">No such element exist in the list.</exception>
        T Ceiling(T value);

        /// <summary>
        /// Number of elements less than <paramref name="value"/>.
        /// </summary>
        int Rank(T value);

        /// <summary>
        /// Key of rank <paramref name="k"/>.
        /// </summary>
        T Select(int k);

        /// <summary>
        /// The number of elements between the <paramref name="lo"/> and <paramref name="hi"/> values.
        /// </summary>
        /// <param name="lo">The lowest value.</param>
        /// <param name="hi">The highest value</param>
        /// <returns>The number of values.</returns>
        /// <remarks><paramref name="lo"/> is included and <paramref name="hi"/> is excluded.</remarks>
        int CountBetween(T lo, T hi);

        /// <summary>
        /// The values between the <paramref name="lo"/> and <paramref name="hi"/> values.
        /// </summary>
        /// <param name="lo">The lowest value.</param>
        /// <param name="hi">The highest value</param>
        /// <returns>The values.</returns>
        /// <remarks><paramref name="lo"/> is included and <paramref name="hi"/> is excluded.</remarks>
        IReadOnlyCollection<T> EnumerateBetween(T lo, T hi);
    }
}
