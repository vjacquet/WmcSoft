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
    public static partial class EnumerableExtensions
    {
        static bool UnguardedIsSorted<T>(IEnumerable<T> enumerable, IComparer<T> comparer) {
            using (var enumerator = enumerable.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var previous = enumerator.Current;
                    while (enumerator.MoveNext()) {
                        if (comparer.Compare(previous, enumerator.Current) > 0)
                            return false;
                        previous = enumerator.Current;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Check if a sequence of elements is sorted.
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="source">The sequence.</param>
        /// <param name="comparer">The comparer</param>
        /// <returns>Returns true if each element in the sequence is less than or equal to its successors; otherwise, false.</returns>
        public static bool IsSorted<T>(this IEnumerable<T> source, IComparer<T> comparer) {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return UnguardedIsSorted(source, comparer ?? Comparer<T>.Default);
        }

        /// <summary>
        /// Check if a sequence of elements is sorted, using the default comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="source">The sequence.</param>
        /// <returns>Returns true if each element in the sequence is less than or equal to its successors; otherwise, false.</returns>
        public static bool IsSorted<T>(this IEnumerable<T> source) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return UnguardedIsSorted(source, Comparer<T>.Default);
        }

        static bool UnguardedIsSorted<T>(IList<T> list, IComparer<T> comparer, int startIndex, int length) {
            var end = startIndex + length;
            for (int i = startIndex + 1; i < end; i++) {
                if (comparer.Compare(list[i - 1], list[i]) > 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check if a sequence of elements is sorted.
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="list">The sequence.</param>
        /// <param name="comparer">The comparer</param>
        /// <param name="startIndex">he start index.</param>
        /// <param name="length">The length.</param>
        /// <returns>Returns true if each element in the sequence is less than or equal to its successors; otherwise, false.</returns>
        public static bool IsSorted<T>(this IList<T> list, IComparer<T> comparer, int startIndex, int length) {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (list.Count < (startIndex + length)) throw new ArgumentException(nameof(list));

            return UnguardedIsSorted(list, comparer ?? Comparer<T>.Default, startIndex, length);
        }

        /// <summary>
        /// Check if a sequence of elements is sorted, using the default comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="list">The sequence.</param>
        /// <param name="startIndex">he start index.</param>
        /// <param name="length">The length.</param>
        /// <returns>Returns true if each element in the sequence is less than or equal to its successors; otherwise, false.</returns>
        public static bool IsSorted<T>(this IList<T> list, int startIndex, int length) {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (list.Count < (startIndex + length)) throw new ArgumentException(nameof(list));

            return UnguardedIsSorted(list, Comparer<T>.Default, startIndex, length);
        }

        /// <summary>
        /// Check if a sequence of elements is sorted.
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="enumerable">The sequence.</param>
        /// <returns>Returns true if each element in the sequence is less than to its successors; otherwise, false.</returns>
        /// <remarks>If true, then the elements are unique in the sequence.</remarks>
        public static bool IsSortedSet<T>(this IEnumerable<T> enumerable, IComparer<T> comparer) {
            using (var enumerator = enumerable.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    var previous = enumerator.Current;
                    while (enumerator.MoveNext()) {
                        if (comparer.Compare(previous, enumerator.Current) >= 0)
                            return false;
                        previous = enumerator.Current;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Check if a sequence of elements is sorted, using the default comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="enumerable">The sequence.</param>
        /// <param name="comparer">The comparer</param>
        /// <returns>Returns true if each element in the sequence is less than to its successors; otherwise, false.</returns>
        /// <remarks>If true, then the elements are unique in the sequence.</remarks>
        public static bool IsSortedSet<T>(this IEnumerable<T> enumerable) {
            return enumerable.IsSortedSet(Comparer<T>.Default);
        }
    }
}