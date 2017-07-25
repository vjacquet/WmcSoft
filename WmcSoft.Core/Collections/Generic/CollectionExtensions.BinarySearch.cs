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
using System.Runtime.CompilerServices;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Provides a set of static methods to extend collection related classes or interfaces.
    /// </summary>
    public static partial class CollectionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int MulDiv(int number, int numerator, int denominator)
        {
            return (int)(((long)number * numerator) / denominator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetMidpoint(int lo, int hi)
        {
            Debug.Assert(hi - lo >= 0);
            return lo + (hi - lo) / 2; // cannot overflow when (hi + lo) could
        }

        private static int UnguardedBinarySearch<T>(this IReadOnlyList<T> list, int lo, int hi, T value, IComparer<T> comparer)
        {
            while (lo <= hi) {
                int mid = GetMidpoint(lo, hi);
                int cmp = comparer.Compare(list[mid], value);
                if (cmp < 0) lo = mid + 1;
                else if (cmp > 0) hi = mid - 1;
                else return mid;
            }
            return ~lo;
        }

        private static int UnguardedBinaryRank<T>(this IReadOnlyList<T> list, int lo, int hi, T value, IComparer<T> comparer)
        {
            while (lo <= hi) {
                int mid = GetMidpoint(lo, hi);
                int cmp = comparer.Compare(list[mid], value);
                if (cmp < 0) lo = mid + 1;
                else if (cmp > 0) hi = mid - 1;
                else return mid;
            }
            return lo;
        }

        private static int UnguardedLowerBound<T>(this IReadOnlyList<T> list, int lo, int hi, T value, IComparer<T> comparer)
        {
            while (lo <= hi) {
                int mid = GetMidpoint(lo, hi);
                int cmp = comparer.Compare(list[mid], value);
                if (cmp < 0) lo = mid + 1;
                else if (cmp > 0) hi = mid - 1;
                else return mid;
            }
            return -1;
        }

        /// <summary>
        /// Returns the last element for which the finder returns a non-positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static int LowerBound<T>(this IReadOnlyList<T> source, T value, IComparer<T> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return UnguardedLowerBound(source, 0, source.Count, value, comparer);
        }

        /// <summary>
        /// Returns the last element for which the finder returns a non-positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The index of element or <code>-1</code> when not found.</returns>
        public static int LowerBound<T>(this IReadOnlyList<T> source, int index, int count, T value, IComparer<T> comparer = null)
        {
            Guard(source, index, count);

            return UnguardedLowerBound(source, index, count - 1, value, comparer ?? Comparer<T>.Default);
        }

        /// <summary>
        /// Returns the last element for which the finder returns a non-positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static int UpperBound<T>(this IReadOnlyList<T> source, T value, IComparer<T> comparer = null)
        {
            return UpperBound(source, 0, source.Count, value, comparer);
        }

        /// <summary>
        /// Returns the last element for which the finder returns a non-positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The index of element or <code>-1</code> when not found.</returns>
        public static int UpperBound<T>(this IReadOnlyList<T> source, int index, int count, T value, IComparer<T> comparer = null)
        {
            Guard(source, index, count);

            var found = UnguardedBinarySearch(source, index, count - 1, value, comparer ?? Comparer<T>.Default);
            if (found < 0) {
                return ~found - 1;
            }
            return found;
        }


        /// <summary>
        /// Computes the number of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for which the <paramref name="comparer"/> returns a negative value.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="value">The value the get the rank of</param>
        /// <param name="comparer">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The number of elements for which the comparer returns a negative value.</returns>
        public static int BinaryRank<T>(this IReadOnlyList<T> source, int index, int count, T value, IComparer<T> comparer = null)
        {
            Guard(source, index, count);

            return UnguardedBinaryRank(source, index, index + count - 1, value, comparer ?? Comparer<T>.Default) - index;
        }

        /// <summary>
        /// Computes the number of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for which the <paramref name="comparer"/> returns a negative value.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="value">The value the get the rank of</param>
        /// <param name="comparer">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The number of elements for which the comparer returns a negative value.</returns>
        public static int BinaryRank<T>(this IReadOnlyList<T> source, T value, IComparer<T> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return UnguardedBinaryRank(source, 0, source.Count - 1, value, comparer ?? Comparer<T>.Default);
        }
    }
}