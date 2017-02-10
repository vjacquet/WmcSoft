#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// Performs the specified action on each element of the sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of the source.</typeparam>
        /// <param name="source">The elements to apply the predicate to.</param>
        /// <param name="action">A function to apply on each element.</param>
        /// <remarks>The function is defined even if the <paramref name="source"/> is null.</remarks>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action) {
            if (source == null)
                return;
            foreach (var item in source)
                action(item);
        }

        /// <summary>
        /// Performs the specified action on each element of the sequence, before yield it.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of the source.</typeparam>
        /// <param name="source">The elements to apply the predicate to.</param>
        /// <param name="action">A function to apply on each element.</param>
        /// <remarks>The action is applied only on the enumerated items.</remarks>
        /// <remarks>The function is defined even if the <paramref name="source"/> is null.</remarks>
        public static IEnumerable<TSource> OnEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action) {
            if (source == null)
                yield break;
            foreach (var item in source) {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// Determines whether each element matches the condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of the source.</typeparam>
        /// <param name="source">The elements to apply the predicate to.</param>
        /// <param name="predicate">The delegate that defines the conditions to check against the elements.</param>
        /// <returns>true if every element in the sequence matches the conditions defined by the specified predicate; 
        /// otherwise, false. If there are no elements, the return value is true.</returns>
        /// <remarks>The predicate is applied on each element of the sequence.</remarks>
        /// <remarks>The function is defined even if the <paramref name="source"/> is null.</remarks>
        public static bool TrueForEach<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate) {
            if (source == null)
                return true;

            bool b = true;
            foreach (var item in source)
                b &= predicate(item);
            return b;
        }

        /// <summary>
        /// Determines whether some element matches the condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of the source.</typeparam>
        /// <param name="source">The elements to apply the predicate to.</param>
        /// <param name="predicate">The delegate that defines the conditions to check against the elements.</param>
        /// <returns>true if some element in the sequence matches the conditions defined by the specified predicate; 
        /// otherwise, false. If there are no elements, the return value is true.</returns>
        /// <remarks>The predicate is applied on each element of the sequence.</remarks>
        /// <remarks>The function is defined even if the <paramref name="source"/> is null.</remarks>
        public static bool TrueForSome<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate) {
            if (source == null)
                return false;

            bool b = false;
            foreach (var item in source)
                b |= predicate(item);
            return b;
        }

    }
}