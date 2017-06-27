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
    /// Provides a set of static methods to extend collection related classes or interfaces.
    /// </summary>
    public static partial class CollectionExtensions
    {
        public static int UnguardedMinElement<TSource>(this IList<TSource> source, int index, int length, Comparison<TSource> comparison)
        {
            if (source.Count == 0)
                return -1;
            var min = source[index];
            var p = index;
            length += index;
            for (int i = index + 1; i < length; i++) {
                if (comparison(source[i], min) < 0) {
                    min = source[p = i];
                }
            }
            return p;
        }

        /// <summary>
        /// Compute the index of the min element in the source list.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of source</typeparam>
        /// <param name="source">The source element</param>
        /// <returns>The zero-based index of the min element or -1 if the range is empty.</returns>
        public static int MinElement<TSource>(this IList<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = Comparer<TSource>.Default.Compare;
            return UnguardedMinElement(source, 0, source.Count, comparison);
        }

        /// <summary>
        /// Compute the index of the min element in the source list.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of source</typeparam>
        /// <param name="source">The source element</param>
        /// <param name="comparer">The IComparer<T> implementation to use when comparing elements, or null to use the default comparer Comparer<T>.Default.</param>
        /// <returns>The zero-based index of the min element or -1 if the range is empty.</returns>
        public static int MinElement<TSource>(this IList<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = (comparer ?? Comparer<TSource>.Default).Compare;
            return UnguardedMinElement(source, 0, source.Count, comparison);
        }

        /// <summary>
        /// Compute the index of the min element in the source list.
        /// </summary>
        /// <typeparam name="TSource">The type of the element of source</typeparam>
        /// <param name="source">The source element</param>
        /// <param name="index">The zero-based starting index of the range to get the min element from.</param>
        /// <param name="count">The length of the range to get the min element from.</param>
        /// <param name="comparer">The IComparer<T> implementation to use when comparing elements, or null to use the default comparer Comparer<T>.Default.</param>
        /// <returns>The zero-based index of the min element or -1 if the range is empty.</returns>
        public static int MinElement<TSource>(this IList<TSource> source, int index, int count, IComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = (comparer ?? Comparer<TSource>.Default).Compare;
            return UnguardedMinElement(source, index, count, comparison);
        }

        public static int MinElement<TSource>(this IList<TSource> source, Comparison<TSource> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return UnguardedMinElement(source, 0, source.Count, comparison);
        }

        public static int MinElement<TSource>(this IList<TSource> source, int index, int length, Comparison<TSource> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (index < 0) throw new ArgumentException(nameof(index));
            if (length < 0) throw new ArgumentException(nameof(length));
            if ((index + length) > source.Count) throw new ArgumentException();

            return UnguardedMinElement(source, index, length, comparison);
        }

        public static int UnguardedMaxElement<TSource>(this IList<TSource> source, int index, int length, Comparison<TSource> comparison)
        {
            if (source.Count == 0)
                return -1;
            var max = source[index];
            var p = index;
            length += index;
            for (var i = index + 1; i < length; i++) {
                if (comparison(source[i], max) >= 0) {
                    max = source[p = i];
                }
            }
            return p;
        }

        public static int MaxElement<TSource>(this IList<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = Comparer<TSource>.Default.Compare;
            return UnguardedMaxElement(source, 0, source.Count, comparison);
        }

        public static int MaxElement<TSource>(this IList<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = (comparer ?? Comparer<TSource>.Default).Compare;
            return UnguardedMaxElement(source, 0, source.Count, comparison);
        }

        public static int MaxElement<TSource>(this IList<TSource> source, int index, int length, IComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = (comparer ?? Comparer<TSource>.Default).Compare;
            return UnguardedMaxElement(source, index, length, comparison);
        }

        public static int MaxElement<TSource>(this IList<TSource> source, Comparison<TSource> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return UnguardedMaxElement(source, 0, source.Count, comparison);
        }

        public static int MaxElement<TSource>(this IList<TSource> source, int index, int length, Comparison<TSource> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (index < 0) throw new ArgumentException(nameof(index));
            if (length < 0) throw new ArgumentException(nameof(length));
            if ((index + length) > source.Count) throw new ArgumentException();

            return UnguardedMaxElement(source, index, length, comparison);
        }

        public static Tuple<int, int> MinMaxElement<TSource>(this IList<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = Comparer<TSource>.Default.Compare;
            return MinMaxElement(source, 0, source.Count, comparison);
        }

        public static Tuple<int, int> MinMaxElement<TSource>(this IList<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = (comparer ?? Comparer<TSource>.Default).Compare;
            return MinMaxElement(source, 0, source.Count, comparison);
        }

        public static Tuple<int, int> MinMaxElement<TSource>(this IList<TSource> source, int index, int length, IComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Comparison<TSource> comparison = (comparer ?? Comparer<TSource>.Default).Compare;
            return MinMaxElement(source, index, length, comparison);
        }

        public static Tuple<int, int> MinMaxElement<TSource>(this IList<TSource> source, Comparison<TSource> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return MinMaxElement(source, 0, source.Count, comparison);
        }

        public static Tuple<int, int> MinMaxElement<TSource>(this IList<TSource> source, int index, int length, Comparison<TSource> comparison)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0)
                return Tuple.Create(-1, -1);
            var min = source[index];
            var max = source[index];
            var p = index;
            var q = index;
            for (var i = index + 1; i < length; i++) {
                if (comparison(source[i], min) < 0) {
                    min = source[p = i];
                } else if (comparison(source[i], max) >= 0) {
                    max = source[q = i];
                }
            }
            return Tuple.Create(p, q);
        }
    }
}