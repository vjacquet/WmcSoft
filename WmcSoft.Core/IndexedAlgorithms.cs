#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft
{
    public static class IndexedAlgorithms
    {
        #region Helpers

        static internal void UnguardedCopy<T>(IList<T> source, int sourceIndex, IList<T> destination, int destinationIndex, int length)
        {
            var end = sourceIndex + length;
            for (int i = sourceIndex, j = destinationIndex; i < end; i++, j++) {
                destination[j] = source[i];
            }
        }

        static internal void UnguardedCopyBackwards<T>(IList<T> source, int sourceIndex, IList<T> destination, int destinationIndex, int length)
        {
            var end = sourceIndex + length;
            for (int i = sourceIndex + length - 1, j = destinationIndex + length - 1; i >= sourceIndex; i--, j--) {
                destination[j] = source[i];
            }
        }

        static internal void UnguardedReverseCopy<T>(IList<T> source, int sourceIndex, IList<T> destination, int destinationIndex, int length)
        {
            var end = sourceIndex + length;
            for (int i = sourceIndex + length - 1, j = destinationIndex; i >= sourceIndex; i--, j++) {
                destination[j] = source[i];
            }
        }

        #endregion

        #region CopyTo methods

        /// <summary>
        /// Copies a range of elements from an array starting at the specified source
        /// index and pastes them to another array starting at the specified destination
        /// index. The length and the indexes are specified as 32-bit integers.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="source">The array that contains the data to copy.</param>
        /// <param name="array">The array that receives the data.</param>
        /// <param name="arrayIndex">The index in the <paramref name="array"/> at which storing begins.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="arrayIndex"/> is less than zero.</exception>
        public static void CopyTo<T>(this T[] source, T[] array, int arrayIndex)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Array.Copy(source, 0, array, arrayIndex, source.Length);
        }

        /// <summary>
        /// Copies a range of elements from a list starting at the specified source
        /// index and pastes them to another list at the specified destination
        /// index. The length and the indexes are specified as 32-bit integers.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list that contains the data to copy.</param>
        /// <param name="list">The list that receives the data.</param>
        /// <param name="listIndex">The index in the <paramref name="list"/> at which storing begins.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="list"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="listIndex"/> is less than zero.</exception>
        public static void CopyTo<T>(this IList<T> source, IList<T> list, int listIndex)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (list == null) throw new ArgumentNullException(nameof(list));
            if (list.Count < (listIndex + source.Count)) throw new ArgumentException(nameof(list));

            UnguardedCopy(source, 0, list, listIndex, source.Count);
        }

        /// <summary>
        /// Copies a range of elements from an array starting at the specified source
        /// index and pastes them to another array starting at the specified destination
        /// index. The length and the indexes are specified as 32-bit integers.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="source">The array that contains the data to copy.</param>
        /// <param name="array">The array that receives the data.</param>
        /// <param name="arrayIndex">The index in the <paramref name="array"/> at which storing begins.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="arrayIndex"/> is less than zero.-or-<paramref name="length"/> is less than zero.</exception>
        public static void CopyTo<T>(this T[] source, T[] array, int arrayIndex, int length)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Array.Copy(source, 0, array, arrayIndex, length);
        }

        /// <summary>
        /// Copies a range of elements from a list starting at the specified source
        /// index and pastes them to another list at the specified destination
        /// index. The length and the indexes are specified as 32-bit integers.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list that contains the data to copy.</param>
        /// <param name="list">The list that receives the data.</param>
        /// <param name="listIndex">The index in the <paramref name="list"/> at which storing begins.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="list"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="listIndex"/> is less than zero.-or-<paramref name="length"/>is less than zero./// </exception>
        public static void CopyTo<T>(this IList<T> source, IList<T> list, int listIndex, int length)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (list == null) throw new ArgumentNullException(nameof(list));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (list.Count < (listIndex + length)) throw new ArgumentException(nameof(list));
            if (source.Count < length) throw new ArgumentException(nameof(source));

            UnguardedCopy(source, 0, list, listIndex, length);
        }

        /// <summary>
        /// Copies a range of elements from an array starting at the specified source
        /// index and pastes them to another array starting at the specified destination
        /// index. The length and the indexes are specified as 32-bit integers.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="source">The array that contains the data to copy.</param>
        /// <param name="sourceIndex">he index in the <paramref name="source"/> at which copying begins.</param>
        /// <param name="array">The array that receives the data.</param>
        /// <param name="arrayIndex">The index in the <paramref name="array"/> at which storing begins.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="sourceIndex"/> is less than zero.-or-<paramref name="arrayIndex"/> is less than zero.-or-<paramref name="length"/>is less than zero.</exception>
        public static void CopyTo<T>(this T[] source, int sourceIndex, T[] array, int arrayIndex, int length)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            Array.Copy(source, sourceIndex, array, arrayIndex, length);
        }

        /// <summary>
        /// Copies a range of elements from a list starting at the specified source
        /// index and pastes them to another list starting at the specified destination
        /// index. The length and the indexes are specified as 32-bit integers.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="source">The array that contains the data to copy.</param>
        /// <param name="sourceIndex">he index in the <paramref name="source"/> at which copying begins.</param>
        /// <param name="list">The array that receives the data.</param>
        /// <param name="listIndex">The index in the <paramref name="list"/> at which storing begins.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="list"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="sourceIndex"/> is less than zero.-or-<paramref name="listIndex"/> is less than zero.-or-<paramref name="length"/>is less than zero.</exception>
        public static void CopyTo<T>(this IList<T> source, int sourceIndex, IList<T> list, int listIndex, int length)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (list == null) throw new ArgumentNullException(nameof(list));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (list.Count < (listIndex + length)) throw new ArgumentException(nameof(list));
            if (source.Count < length) throw new ArgumentException(nameof(source));

            UnguardedCopy(source, sourceIndex, list, listIndex, length);
        }

        #endregion

        #region CopyBackwardsTo methods

        /// <summary>
        /// Copies a range of elements from an array starting at the specified source
        /// index and pastes them to another array starting at the specified destination
        /// index. The length and the indexes are specified as 32-bit integers.
        /// The elements are copied in reverse order (the last element is copied first), but their relative order is preserved.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="source">The array that contains the data to copy.</param>
        /// <param name="sourceIndex">he index in the <paramref name="source"/> at which copying begins.</param>
        /// <param name="list">The array that receives the data.</param>
        /// <param name="listIndex">The index in the <paramref name="list"/> at which storing begins.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="list"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="sourceIndex"/> is less than zero.-or-<paramref name="listIndex"/> is less than zero.-or-<paramref name="length"/>is less than zero.</exception>
        public static void CopyBackwardsTo<T>(this IList<T> source, int sourceIndex, IList<T> list, int listIndex, int length)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (list.Count < (listIndex + length)) throw new ArgumentException(nameof(list));
            if (source.Count < (sourceIndex + length)) throw new ArgumentException(nameof(source));

            UnguardedCopyBackwards(source, sourceIndex, list, listIndex, length);
        }

        /// <summary>
        /// Copies a range of elements from a list starting at the specified source
        /// index and pastes them to another list at the specified destination
        /// index. The length and the indexes are specified as 32-bit integers.
        /// The elements are copied in reverse order (the last element is copied first), but their relative order is preserved.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list that contains the data to copy.</param>
        /// <param name="list">The list that receives the data.</param>
        /// <param name="listIndex">The index in the <paramref name="list"/> at which storing begins.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="list"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="listIndex"/> is less than zero.-or-<paramref name="length"/>is less than zero./// </exception>
        public static void CopyBackwardsTo<T>(this IList<T> source, IList<T> list, int listIndex, int length)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (list.Count < (listIndex + length)) throw new ArgumentException(nameof(list));
            if (source.Count < length) throw new ArgumentException(nameof(source));

            UnguardedCopyBackwards(source, 0, list, listIndex, length);
        }

        /// <summary>
        /// Copies a range of elements from a list starting at the specified source
        /// index and pastes them to another list at the specified destination
        /// index. The length and the indexes are specified as 32-bit integers.
        /// The elements are copied in reverse order (the last element is copied first), but their relative order is preserved.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list that contains the data to copy.</param>
        /// <param name="list">The list that receives the data.</param>
        /// <param name="listIndex">The index in the <paramref name="list"/> at which storing begins.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="list"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="listIndex"/> is less than zero.</exception>
        public static void CopyBackwardsTo<T>(this IList<T> source, IList<T> list, int listIndex)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (list.Count < listIndex) throw new ArgumentException(nameof(list));

            UnguardedCopyBackwards(source, 0, list, 0, source.Count);
        }

        #endregion

        #region ReverseCopyTo

        /// <summary>
        /// Copies a range of elements from an array starting at the specified source
        /// index and pastes them to another array starting at the specified destination
        /// index. The length and the indexes are specified as 32-bit integers.
        /// The elements are copied in reverse order into the destination list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="source">The array that contains the data to copy.</param>
        /// <param name="sourceIndex">he index in the <paramref name="source"/> at which copying begins.</param>
        /// <param name="list">The array that receives the data.</param>
        /// <param name="listIndex">The index in the <paramref name="list"/> at which storing begins.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="list"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="sourceIndex"/> is less than zero.-or-<paramref name="listIndex"/> is less than zero.-or-<paramref name="length"/>is less than zero.</exception>
        public static void ReverseCopyTo<T>(this IList<T> source, int sourceIndex, IList<T> list, int listIndex, int length)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (list.Count < (listIndex + length)) throw new ArgumentException(nameof(list));
            if (source.Count < (sourceIndex + length)) throw new ArgumentException(nameof(source));

            UnguardedReverseCopy(source, sourceIndex, list, listIndex, length);
        }

        /// <summary>
        /// Copies a range of elements from a list starting at the specified source
        /// index and pastes them to another list at the specified destination
        /// index. The length and the indexes are specified as 32-bit integers.
        /// The elements are copied in reverse order into the destination <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list that contains the data to copy.</param>
        /// <param name="list">The list that receives the data.</param>
        /// <param name="listIndex">The index in the <paramref name="list"/> at which storing begins.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="list"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="listIndex"/> is less than zero.-or-<paramref name="length"/>is less than zero./// </exception>
        public static void ReverseCopyTo<T>(this IList<T> source, IList<T> list, int listIndex, int length)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (list.Count < (listIndex + length)) throw new ArgumentException(nameof(list));
            if (source.Count < length) throw new ArgumentException(nameof(source));

            UnguardedReverseCopy(source, 0, list, listIndex, length);
        }

        /// <summary>
        /// Copies a range of elements from a list starting at the specified source
        /// index and pastes them to another list at the specified destination
        /// index. The length and the indexes are specified as 32-bit integers.
        /// The elements are copied in reverse order into the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list that contains the data to copy.</param>
        /// <param name="list">The list that receives the data.</param>
        /// <param name="listIndex">The index in the <paramref name="list"/> at which storing begins.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="list"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="listIndex"/> is less than zero.</exception>
        public static void ReverseCopyTo<T>(this IList<T> source, IList<T> list, int listIndex)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (list.Count < listIndex) throw new ArgumentException(nameof(list));

            UnguardedReverseCopy(source, 0, list, 0, source.Count);
        }

        #endregion

        #region Exchange

        /// <summary>
        /// Replaces the value at the index with a new <paramref name="value"/> and returns the old value. 
        /// </summary>
        /// <typeparam name="TList">The type of list.</typeparam>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="source">The source list</param>
        /// <param name="value">The new value.</param>
        /// <param name="index">The index.</param>
        /// <returns>The old value at the specified index.</returns>
        public static T Exchange<TList, T>(this TList source, T value, int index)
            where TList : IList<T>
        {
            var temp = source[index];
            source[index] = value;
            return temp;
        }

        /// <summary>
        /// Replaces the value at the first index, pushes its old value to the second index and then returns
        /// its old value.
        /// </summary>
        /// <typeparam name="TList">The type of list.</typeparam>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="source">The source list</param>
        /// <param name="value">The new value.</param>
        /// <param name="index1">The first index.</param>
        /// <param name="index2">The second index.</param>
        /// <returns>The old value at the second index.</returns>
        public static T Exchange<TList, T>(this TList source, T value, int index1, int index2)
            where TList : IList<T>
        {
            var temp = source[index2];
            source[index2] = source[index1];
            source[index1] = value;
            return temp;
        }

        #endregion

        #region Fill

        public static void UnguardedFill<TList, T>(this TList source, T value, int first, int last)
          where TList : IList<T>
        {
            while (first != last) {
                source[first++] = value;
            }
        }

        public static void UnguardedFill<TList, T>(this TList source, T value)
            where TList : IList<T>
        {
            UnguardedFill(source, value, 0, source.Count);
        }

        public static void Fill<TList, T>(this TList source, T value)
            where TList : IList<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            UnguardedFill(source, value);
        }

        #endregion

        #region FindXXX

        public static int UnguardedFindIf<T>(IList<T> list, int first, int last, Predicate<T> relation)
        {
            while (first != last) {
                if (relation(list[first]))
                    return first;
                ++first;
            }
            return last;
        }

        public static int UnguardedFindIfNot<T>(IList<T> list, int first, int last, Predicate<T> relation)
        {
            while (first != last) {
                if (!relation(list[first]))
                    return first;
                ++first;
            }
            return last;
        }

        public static int UnguardedAdjacentFindNotEmpty<T>(IList<T> list, int first, int last, Relation<T> relation)
        {
            var next = first + 1;
            while (next != last) {
                if (relation(list[first], list[next]))
                    return first;
                ++next;
                ++first;
            }
            return last;
        }

        public static int UnguardedAdjacentFind<T>(IList<T> list, int first, int last, Relation<T> relation)
        {
            if (first == last)
                return last;
            return UnguardedAdjacentFindNotEmpty(list, first, last, relation);
        }

        #endregion

        #region InsertionSort

        public static void UnguardedInsertionSort<T>(IList<T> source, int index, int length, IComparer<T> comparer)
        {
            var endIndex = index + length;
            for (int i = index + 1; i < endIndex; i++) {
                var value = source[i];
                int j = i;
                while (j > index && comparer.Compare(value, source[j - 1]) < 0) {
                    source[j] = source[j - 1];
                    --j;
                }
                source[j] = value;
            }
        }

        public static void InsertionSort<T>(this IList<T> source, int index, int length, IComparer<T> comparer = null)
        {
            UnguardedInsertionSort(source, index, length, comparer ?? Comparer<T>.Default);
        }

        public static void InsertionSort<T>(this IList<T> source, IComparer<T> comparer = null)
        {
            UnguardedInsertionSort(source, 0, source.Count, comparer ?? Comparer<T>.Default);
        }

        public static void UnguardedInsertionSort<T>(IList<T> source, int index, int length, Relation<T> relation)
        {
            var endIndex = index + length;
            for (int i = index + 1; i < endIndex; i++) {
                var value = source[i];
                int j = i;
                while (j > index && relation(value, source[j - 1])) {
                    source[j] = source[j - 1];
                    --j;
                }
                source[j] = value;
            }
        }

        public static void InsertionSort<T>(this IList<T> source, int index, int length, Relation<T> relation)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (relation == null) throw new ArgumentNullException(nameof(relation));

            UnguardedInsertionSort(source, index, length, relation);
        }

        public static void InsertionSort<T>(this IList<T> source, Relation<T> relation)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (relation == null) throw new ArgumentNullException(nameof(relation));

            UnguardedInsertionSort(source, 0, source.Count, relation);
        }

        #endregion

        #region Iota

        public static void Iota(this IList<short> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var length = checked((short)source.Count);
            for (short i = 0; i < length; i++) {
                source[i] = i;
            }
        }

        public static void Iota(this IList<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var length = source.Count;
            for (var i = 0; i < length; i++) {
                source[i] = i;
            }
        }

        public static void Iota(this IList<long> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var length = source.Count;
            for (var i = 0; i < length; i++) {
                source[i] = i;
            }
        }

        public static void Iota(this IList<short> source, short startValue, short step = 1)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var length = checked((short)source.Count);
            for (short i = 0; i < length; i++) {
                source[i] = startValue;
                startValue += step;
            }
        }

        public static void Iota(this IList<int> source, int startValue, int step = 1)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var length = source.Count;
            for (var i = 0; i < length; i++) {
                source[i] = startValue;
                startValue += step;
            }
        }

        public static void Iota(this IList<long> source, long startValue, long step = 1)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var length = source.Count;
            for (var i = 0; i < length; i++) {
                source[i] = startValue;
                startValue += step;
            }
        }

        #endregion

        #region KeyIndexCountingSort

        static void UnguardedKeyIndexCountingSort<T, TCounter>(IList<T> source, Func<T, int> keySelector, TCounter counters)
            where TCounter : IList<int>
        {
            var length = source.Count;
            var aux = new T[length];
            source.CopyTo(aux, 0);

            // compute frequencies
            for (int i = 0; i < length; i++)
                counters[keySelector(aux[i]) + 1]++;

            // transforms count to indices
            var maxKey = counters.Count - 1;
            for (int i = 2; i < maxKey; i++)
                counters[i] += counters[i - 1];

            // distributes the records back to the source
            for (int i = 0; i < length; i++)
                source[counters[keySelector(aux[i])]++] = aux[i];
        }

        /// <remarks>The sort is stable.</remarks>
        public static void KeyIndexCountingSort<T>(this IList<T> source, Func<T, int> keySelector, int maxKey)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (maxKey <= 0 || maxKey > ushort.MaxValue) throw new ArgumentOutOfRangeException(nameof(maxKey));

            var counters = new int[maxKey + 1];
            UnguardedKeyIndexCountingSort(source, keySelector, counters);
        }

        #endregion

        #region Rotate

        public static int UnguardedRotate<T>(IList<T> source, int n, int startIndex, int length)
        {
            if (n == 0)
                return startIndex;

            var temp = new T[n];
            if (n < 0) {
                // rotate left
                n = -n;
                UnguardedCopy(source, startIndex, temp, 0, n);
                UnguardedCopy(source, startIndex + n, source, startIndex, length - n);
                startIndex += length - n;
                UnguardedCopy(temp, 0, source, startIndex, n);
            } else {
                // rotate right
                UnguardedCopy(source, startIndex + length - n, temp, 0, n);
                UnguardedCopyBackwards(source, startIndex, source, startIndex + n, length - n);
                UnguardedCopy(temp, 0, source, startIndex, n);
                startIndex += n;
            }
            return startIndex;
        }

        /// <summary>
        /// Performs a left rotation on the <paramref name="source"/>, moving the <paramref name="n"/> first elements of the specified range 
        /// at the end.
        /// </summary>
        /// <typeparam name="T">The type of the elemets</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="n">The number of elements to move</param>
        /// <param name="startIndex">The start index of the range</param>
        /// <param name="length">The length of the range</param>
        /// <returns>The new position of the <paramref name="startIndex"/>.</returns>
        public static int Rotate<T>(this IList<T> source, int n, int startIndex, int length)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (source.Count < (startIndex + length)) throw new ArgumentException(nameof(source));
            if (n > length || -n > length) throw new ArgumentException(nameof(n));

            return UnguardedRotate(source, n, startIndex, length);
        }

        /// <summary>
        /// Performs a left rotation on the <paramref name="source"/>, moving the <paramref name="n"/> first elements at the end.
        /// </summary>
        /// <typeparam name="T">The type of the elemets</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="n">The number of elements to move</param>
        /// <returns>The new position of the first element</returns>
        public static int Rotate<T>(this IList<T> source, int n)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var length = source.Count;
            if (n > length || -n > length) throw new ArgumentException(nameof(n));

            return UnguardedRotate(source, n, 0, length);
        }

        #endregion

        #region StablePartition

        #endregion

        #region SwapItems methods

        /// <summary>
        /// Swaps two items.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="i">The item at the <paramref name="i"/> index.</param>
        /// <param name="j">The item at the <paramref name="j"/> index.</param>
        /// <remarks>This function does not guard against null list or out of bound indices.</remarks>
        public static void SwapItems<T>(this T[] list, int i, int j)
        {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        #endregion
    }
}
