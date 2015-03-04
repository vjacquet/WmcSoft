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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    public static class ArrayExtensions
    {
        #region Sort methods

        /// <summary>
        /// Sorts the elements in an entire Array using the <see cref="IComparable&lt;T&gt;"/> generic interface implementation of each element of the Array.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The one-dimensional, zero-based Array to sort.</param>
        /// <returns>The one-dimensional, zero-based sorted Array</returns>
        public static T[] Sort<T>(this T[] array) {
            Array.Sort(array);
            return array;
        }

        /// <summary>
        /// Sorts the elements in an Array using the specified <see cref="Comparison&lt;T&gt;"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The one-dimensional, zero-based Array to sort.</param>
        /// <param name="comparison">The <see cref="Comparison&lt;T&gt;"/> to use when comparing elements.</param>
        /// <returns>The one-dimensional, zero-based sorted Array</returns>
        public static T[] Sort<T>(this T[] array, Comparison<T> comparison) {
            Array.Sort(array, comparison);
            return array;
        }

        /// <summary>
        /// Sorts the elements in an Array using the specified <see cref="IComparer&lt;T&gt;"/> generic interface.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The one-dimensional, zero-based Array to sort.</param>
        /// <param name="comparer">The <see cref="IComparer&lt;T&gt;"/> generic interface implementation to use when comparing elements, or null to use the IComparable<T> generic interface implementation of each element.</param>
        /// <returns>The one-dimensional, zero-based sorted Array</returns>
        public static T[] Sort<T>(this T[] array, IComparer<T> comparer) {
            Array.Sort(array, comparer);
            return array;
        }

        #endregion
    }
}
