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
        private static int UnguardedBinarySearch<T>(this IReadOnlyList<T> list, int lo, int hi, Func<T, int> finder)
        {
            while (lo <= hi) {
                int mid = GetMidpoint(lo, hi);
                int cmp = finder(list[mid]);
                if (cmp < 0) lo = mid + 1;
                else if (cmp > 0) hi = mid - 1;
                else return mid;
            }
            return ~lo;
        }

        private static int UnguardedBinaryRank<T>(this IReadOnlyList<T> list, int lo, int hi, Func<T, int> finder)
        {
            while (lo <= hi) {
                int mid = GetMidpoint(lo, hi);
                int cmp = finder(list[mid]);
                if (cmp < 0) lo = mid + 1;
                else if (cmp > 0) hi = mid - 1;
                else return mid;
            }
            return lo;
        }

        /// <summary>
        /// Searches a range of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for an element using the specified function and returns the zero-based index of the element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The zero-based index of item in the sorted <seealso cref="IReadOnlyList{T}"/>, if item is found; 
        /// otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, 
        /// if there is no larger element, the bitwise complement of Count.</returns>
        public static int BinarySearch<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder)
        {
            Guard(source, index, count);

            try {
                return UnguardedBinarySearch(source, index, index + count - 1, finder);
            } catch (Exception e) {
                var message = string.Format(Properties.Resources.InvalidOperationExceptionRethrow, nameof(finder), e.GetType().FullName);
                throw new InvalidOperationException(message, e);
            }
        }

        /// <summary>
        /// Searches a range of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for an element using the specified function and returns the zero-based index of the element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The zero-based index of item in the sorted <seealso cref="IReadOnlyList{T}"/>, if item is found; 
        /// otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than item or, 
        /// if there is no larger element, the bitwise complement of Count.</returns>
        public static int BinarySearch<T>(this IReadOnlyList<T> source, Func<T, int> finder)
        {
            return BinarySearch(source, 0, source.Count, finder);
        }

        /// <summary>
        /// Searches a range of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for an element using the specified function and returns the element.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T BinaryFind<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder, T defaultValue = default(T))
        {
            var found = BinarySearch(source, index, count, finder);
            if (found >= 0)
                return source[found];
            return defaultValue;
        }

        /// <summary>
        /// Searches a range of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for an element using the specified function and returns the element.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T BinaryFind<T>(this IReadOnlyList<T> source, Func<T, int> finder, T defaultValue = default(T))
        {
            return BinaryFind(source, 0, source.Count, finder, defaultValue);
        }

        /// <summary>
        /// Returns the last element for which the finder returns a non-positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The index of element or <code>-1</code> when not found.</returns>
        public static int LowerBound<T>(this IReadOnlyList<T> source, Func<T, int> finder)
        {
            return LowerBound(source, 0, source.Count, finder);
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
        public static int LowerBound<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder)
        {
            var found = BinarySearch(source, index, count, finder);
            if (found < 0) {
                return ~found;
            }
            return found;
        }

        /// <summary>
        /// Returns the last element for which the finder returns a non-positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T Floor<T>(this IReadOnlyList<T> source, Func<T, int> finder, T defaultValue = default(T))
        {
            return Floor(source, 0, source.Count, finder, defaultValue);
        }

        /// <summary>
        /// Returns the last element for which the finder returns a non-positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T Floor<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder, T defaultValue = default(T))
        {
            var found = BinarySearch(source, index, count, finder);
            if (found < 0) {
                var lo = ~found;
                if (lo > index)
                    return source[lo - 1];
                return defaultValue;
            }
            return source[found];
        }

        /// <summary>
        /// Returns the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The index of element or <code>-1</code> when not found.</returns>
        public static int UpperBound<T>(this IReadOnlyList<T> source, Func<T, int> finder)
        {
            return UpperBound(source, 0, source.Count, finder);
        }

        /// <summary>
        /// Returns the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The index of element or <code>-1</code> when not found.</returns>
        public static int UpperBound<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder)
        {
            var found = BinarySearch(source, index, count, finder);
            if (found < 0) {
                var lo = ~found;
                if (lo < index + count)
                    return lo;
                return index + count;
            }
            return found;
        }

        /// <summary>
        /// Returns the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T Ceilling<T>(this IReadOnlyList<T> source, Func<T, int> finder, T defaultValue = default(T))
        {
            return Ceilling(source, 0, source.Count, finder, defaultValue);
        }

        /// <summary>
        /// Returns the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static T Ceilling<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder, T defaultValue = default(T))
        {
            var found = BinarySearch(source, index, count, finder);
            if (found < 0) {
                var lo = ~found;
                if (lo < index + count)
                    return source[lo];
                return defaultValue;
            }
            return source[found];
        }

        /// <summary>
        /// Returns the last element for which the finder returns a negative value and the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static Tuple<T, T> Bounds<T>(this IReadOnlyList<T> source, Func<T, int> finder, T defaultValue = default(T))
        {
            return Bounds(source, 0, source.Count, finder, defaultValue);
        }

        /// <summary>
        /// Returns the last element for which the finder returns a negative value and the first element for which the finder returns a positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The element or the <paramref name="defaultValue"/> when not found</returns>
        public static Tuple<T, T> Bounds<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder, T defaultValue = default(T))
        {
            if (count == 0)
                return Tuple.Create(defaultValue, defaultValue);
            var found = BinarySearch(source, index, count, finder);
            if (found < 0) {
                var lo = ~found;
                if (lo <= index)
                    return Tuple.Create(defaultValue, source[lo]);
                if (lo < index + count)
                    return Tuple.Create(source[lo - 1], source[lo]);
                return Tuple.Create(source[lo - 1], defaultValue);
            }
            var equivalent = source[found];
            return Tuple.Create(equivalent, equivalent);
        }

        /// <summary>
        /// Computes the number of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for which the <paramref name="finder"/> returns a negative value.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The number of elements for which the finder returns a negative value.</returns>
        public static int BinaryRank<T>(this IReadOnlyList<T> source, int index, int count, Func<T, int> finder)
        {
            Guard(source, index, count);

            try {
                return UnguardedBinaryRank(source, index, index + count - 1, finder) - index;
            } catch (Exception e) {
                var message = string.Format(Properties.Resources.InvalidOperationExceptionRethrow, nameof(finder), e.GetType().FullName);
                throw new InvalidOperationException(message, e);
            }
        }

        /// <summary>
        /// Computes the number of elements in the sorted <seealso cref="IReadOnlyList{T}"/> for which the <paramref name="finder"/> returns a negative value.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="finder">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The number of elements for which the finder returns a negative value.</returns>
        public static int BinaryRank<T>(this IReadOnlyList<T> source, Func<T, int> finder)
        {
            return BinaryRank(source, 0, source.Count, finder);
        }
    }
}