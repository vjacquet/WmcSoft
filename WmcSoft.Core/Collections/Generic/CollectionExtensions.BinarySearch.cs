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

        private static int UnguardedBinaryRank<T>(this IReadOnlyList<T> list, int startIndex, int n, T value, IComparer<T> comparer)
        {
            return UnguardedLowerBound(list, startIndex, n, value, comparer) - startIndex;
        }

#if CPP
template<class ForwardIterator, class T>
ForwardIterator lower_bound(ForwardIterator first, ForwardIterator last, const T& val)
{
    ForwardIterator it;
    iterator_traits<ForwardIterator>::difference_type count, step;
    count = distance(first, last);
    while (count>0)
    {
        it = first; step=count/2; advance(it, step);
        if (* it<val) {                 // or: if (comp(*it,val)), for version (2)
            first=++it;
            count-=step+1;
        }
        else count=step;
    }
    return first
}
#endif

        private static int UnguardedLowerBound<T>(this IReadOnlyList<T> list, int first, int n, T value, IComparer<T> comparer)
        {
            int last = first + n;
            while (first < last) {
                int mid = GetMidpoint(first, last);
                int cmp = comparer.Compare(list[mid], value);
                if (cmp < 0) first = mid + 1;
                else last = mid;
            }
            return first;
        }

#if CPP
template<class ForwardIterator, class T>
  ForwardIterator upper_bound(ForwardIterator first, ForwardIterator last, const T& val)
{
    ForwardIterator it;
    iterator_traits<ForwardIterator>::difference_type count, step;
    count = std::distance(first, last);
    while (count > 0) {
        it = first; step = count / 2; std::advance(it, step);
        if (!(val < *it))                 // or: if (!comp(val,*it)), for version (2)
          { first = ++it; count -= step + 1; } else count = step;
    }
    return first;
}
#endif

        private static int UnguardedUpperBound<T>(this IReadOnlyList<T> list, int first, int n, T value, IComparer<T> comparer)
        {
            int last = first + n;
            while (first < last) {
                int mid = GetMidpoint(first, last);
                int cmp = comparer.Compare(value, list[mid]);
                if (cmp >= 0) first = mid + 1;
                else last = mid;
            }
            return first;
        }

        private static Tuple<int,int> UnguardedEqualRange<T>(this IReadOnlyList<T> list, int first, int n, T value, IComparer<T> comparer)
        {
            var lo = UnguardedLowerBound(list, first, n, value, comparer);
            var hi = UnguardedUpperBound(list, lo, n - (lo - first), value, comparer);
            return Tuple.Create(lo, hi);
        }

        /// <summary>
        /// Returns the index of the first element for which the comparer returns a non-negative value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="value">The value the get the rank of</param>
        /// <param name="comparer">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The index of the element, or the count of element in <paramref name="source"/> if no element matches.</returns>
        public static int LowerBound<T>(this IReadOnlyList<T> source, T value, IComparer<T> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return UnguardedLowerBound(source, 0, source.Count, value, comparer ?? Comparer<T>.Default);
        }

        /// <summary>
        /// Returns the index of the first element for which the comparer returns a non-negative value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="value">The value the get the rank of</param>
        /// <param name="comparer">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The index of element or (<paramref name="index"/> + <paramref name="count"/>) if no element matches.</returns>
        public static int LowerBound<T>(this IReadOnlyList<T> source, int index, int count, T value, IComparer<T> comparer = null)
        {
            Guard(source, index, count);

            return UnguardedLowerBound(source, index, count, value, comparer ?? Comparer<T>.Default);
        }

        /// <summary>
        /// Returns the index of the first element for which the comparer returns a strictly positive value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="value">The value the get the rank of</param>
        /// <param name="comparer">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The index of the element, or the count of element in <paramref name="source"/> if no element matches.</returns>
        public static int UpperBound<T>(this IReadOnlyList<T> source, T value, IComparer<T> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return UnguardedUpperBound(source, 0, source.Count, value, comparer ?? Comparer<T>.Default);
        }

        /// <summary>
        /// Returns the index of the first element for which the comparer returns a non negative value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="value">The value the get the rank of</param>
        /// <param name="comparer">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The index of the element, or the count of element in <paramref name="source"/> if no element matches.</returns>
        public static int UpperBound<T>(this IReadOnlyList<T> source, int index, int count, T value, IComparer<T> comparer = null)
        {
            Guard(source, index, count);

            return UnguardedUpperBound(source, index, count, value, comparer ?? Comparer<T>.Default);
        }

        /// <summary>
        /// Returns the bounds of the subrange that includes all elements equivalent to the value value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="value">The value the get the rank of</param>
        /// <param name="comparer">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The bounds of the subrange.</returns>
        public static Tuple<int,int> EqualRange<T>(this IReadOnlyList<T> source, T value, IComparer<T> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return UnguardedEqualRange(source, 0, source.Count, value, comparer ?? Comparer<T>.Default);
        }

        /// <summary>
        /// Returns the bounds of the subrange that includes all elements equivalent to the value value.
        /// </summary>
        /// <typeparam name="T">The type of items in the list</typeparam>
        /// <param name="source">The sorted list</param>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="value">The value the get the rank of</param>
        /// <param name="comparer">Function returning 0 wen the element equal to the searched item, < 0 when it is smaller and > 0 when it is greater.</param>
        /// <returns>The bounds of the subrange.</returns>
        public static Tuple<int, int> EqualRange<T>(this IReadOnlyList<T> source, int index, int count, T value, IComparer<T> comparer = null)
        {
            Guard(source, index, count);

            return UnguardedEqualRange(source, index, count, value, comparer ?? Comparer<T>.Default);
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

            return UnguardedBinaryRank(source, index, count, value, comparer ?? Comparer<T>.Default);
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

            return UnguardedBinaryRank(source, 0, source.Count, value, comparer ?? Comparer<T>.Default);
        }
    }
}