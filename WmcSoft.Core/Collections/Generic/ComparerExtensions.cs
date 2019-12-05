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
    public static class ComparerExtensions
    {
        #region Bind

        public static Func<T, int> BindFirst<T>(this IComparer<T> comparer, T first)
        {
            return x => comparer.Compare(first, x);
        }

        public static Func<T, int> BindSecond<T>(this IComparer<T> comparer, T second)
        {
            return x => comparer.Compare(x, second);
        }

        public static Predicate<T> Bind<T>(this IEqualityComparer<T> comparer, T first)
        {
            return x => comparer.Equals(first, x);
        }

        #endregion

        #region EqualsAny

        public static bool EqualsAny<T>(this IEqualityComparer<T> comparer, T reference, T value1, T value2)
        {
            return comparer.Equals(reference, value1) || comparer.Equals(reference, value2);
        }

        public static bool EqualsAny<T>(this IEqualityComparer<T> comparer, T reference, params T[] values)
        {
            for (int i = 0; i < values.Length; i++) {
                if (comparer.Equals(reference, values[i]))
                    return true;
            }
            return false;
        }

        #endregion

        #region EqualsAll

        public static bool EqualsAll<T>(this IEqualityComparer<T> comparer, T reference, T value1, T value2)
        {
            return comparer.Equals(reference, value1) && comparer.Equals(reference, value2);
        }

        public static bool EqualsAll<T>(this IEqualityComparer<T> comparer, T reference, params T[] values)
        {
            for (int i = 0; i < values.Length; i++) {
                if (!comparer.Equals(reference, values[i]))
                    return false;
            }
            return true;
        }

        #endregion

        #region Lexicographical

        /// <summary>
        /// Creates an <see cref="IComparer{T}"/> to compare two <see cref="IEnumerable{T}"/> in lexicographical order.
        /// </summary>
        /// <typeparam name="T">The type of elements of the <see cref="IEnumerable{T}"/> to compare.</typeparam>
        /// <param name="comparer">The comparer to decorate.</param>
        /// <returns>The decorated comparer.</returns>
        public static LexicographicalComparer<T> Lexicographical<T>(this IComparer<T> comparer)
        {
            return new LexicographicalComparer<T>(comparer);
        }

        #endregion

        #region Reverse

        /// <summary>
        /// Decorates a <see cref="IComparer{T}"/> to exposes a method that compares two objects in descending order.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="comparer">The comparer to decorate.</param>
        /// <returns>The decorated comparer.</returns>
        public static ReverseComparer<T> Reverse<T>(this IComparer<T> comparer)
        {
            return new ReverseComparer<T>(comparer);
        }

        #endregion
    }
}
