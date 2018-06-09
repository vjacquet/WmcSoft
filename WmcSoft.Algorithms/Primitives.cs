#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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


namespace WmcSoft.Algorithms
{
    public static class Primitives
    {
        /// <summary>
        /// Assigns a new value to an element and returns the old one.
        /// </summary>
        /// <typeparam name="T">The type of element to assign.</typeparam>
        /// <param name="obj">The element.</param>
        /// <param name="value">The new value.</param>
        /// <returns>The old value.</returns>
        public static T Exchange<T>(ref T obj, T value)
        {
            var t = obj;
            obj = value;
            return t;
        }

        /// <summary>
        /// Returns the value of an element and clears it.
        /// </summary>
        /// <typeparam name="T">The type of element to assign.</typeparam>
        /// <param name="obj">The element.</param>
        /// <returns>The value.</returns>
        public static T Move<T>(ref T obj)
        {
            var moved = obj;
            obj = default(T);
            return moved;
        }

        /// <summary>
        /// Swaps two elements.
        /// </summary>
        /// <typeparam name="T">The type of element to swap.</typeparam>
        /// <param name="x">The first parameter.</param>
        /// <param name="y">The second parameter.</param>
        public static void Swap<T>(ref T x, ref T y)
        {
            var t = x;
            x = y;
            y = t;
        }

        /// <summary>
        /// Sorts two elements.
        /// <typeparam name="T">The type of element to sort.</typeparam>
        /// <param name="x">The first parameter.</param>
        /// <param name="y">The second parameter.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements, or <c>null</c> to use the default comparer <see cref="Comparer{T}.Default"/>.</param>
        public static void Sort<T>(ref T x, ref T y, IComparer<T> comparer = null)
        {
            comparer = comparer ?? Comparer<T>.Default;
            if (comparer.Compare(x, y) > 0) {
                Swap(ref x, ref y);
            }
        }
    }
}
