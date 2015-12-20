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
    /// Defines the extension methods to the <see cref="List{T}"/> class. This is a static class. 
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Sorts all the elements in the list in backwards order.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source list</param>
        public static void SortBackwards<T>(this List<T> source) {
            SortBackwards(source, Comparer<T>.Default);
        }

        /// <summary>
        /// Sorts all the elements in the list in backwards order, using the given comparison.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source list</param>
        /// <param name="comparison">The comparison function.</param>
        public static void SortBackwards<T>(this List<T> source, Comparison<T> comparison) {
            source.Sort((x, y) => comparison(y, x));
        }

        /// <summary>
        /// Sorts all the elements in the list in backwards order, using the given comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source list</param>
        /// <param name="comparer">The comparer object.</param>
        public static void SortBackwards<T>(this List<T> source, IComparer<T> comparer) {
            source.Sort(new ReverseComparer<T>(comparer));
        }

        /// <summary>
        /// Sorts in backwards order the range of elements from the list, defined by the start index and the count of elements, using the given comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source list</param>
        /// <param name="index">The start index</param>
        /// <param name="count">The count of element to sort</param>
        /// <param name="comparer">The comparer object.</param>
        public static void SortBackwards<T>(this List<T> source, int index, int count, IComparer<T> comparer) {
            source.Sort(index, count, new ReverseComparer<T>(comparer));
        }
    }
}
