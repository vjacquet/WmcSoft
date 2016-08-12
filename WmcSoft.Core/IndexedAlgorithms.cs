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

        static internal void UnguardedCopy<T>(IList<T> source, int sourceIndex, IList<T> destination, int destinationIndex, int length) {
            var end = sourceIndex + length;
            for (int i = sourceIndex, j = destinationIndex; i < end; i++, j++) {
                destination[j] = source[i];
            }
        }

        static internal void UnguardedCopyBackwards<T>(IList<T> source, int sourceIndex, IList<T> destination, int destinationIndex, int length) {
            var end = sourceIndex + length;
            for (int i = sourceIndex + length - 1, j = destinationIndex + length - 1; i >= sourceIndex; i--, j--) {
                destination[j] = source[i];
            }
        }

        #endregion

        #region CopyTo methods

        public static void CopyTo<T>(this T[] source, T[] array, int arrayIndex, int length) {
            if (source == null) throw new ArgumentNullException("source");
            Array.Copy(source, 0, array, arrayIndex, length);
        }

        public static void CopyTo<T>(this IList<T> source, IList<T> array, int arrayIndex, int length) {
            if (source == null) throw new ArgumentNullException("source");

            if (array == null) throw new ArgumentNullException("array");
            if (length < 0) throw new ArgumentOutOfRangeException("length");
            if (array.Count < (arrayIndex + length)) throw new ArgumentException("array");
            if (source.Count < length) throw new ArgumentException("source");

            for (int i = 0; i < length; i++) {
                array[arrayIndex++] = source[i];
            }
        }

        public static void CopyTo<T>(this T[] source, int sourceIndex, T[] array, int arrayIndex, int length) {
            if (source == null) throw new ArgumentNullException("source");
            Array.Copy(source, sourceIndex, array, arrayIndex, length);
        }

        public static void CopyTo<T>(this IList<T> source, int sourceIndex, IList<T> array, int arrayIndex, int length) {
            if (source == null) throw new ArgumentNullException("source");

            if (array == null) throw new ArgumentNullException("array");
            if (length < 0) throw new ArgumentOutOfRangeException("length");
            if (array.Count < (arrayIndex + length)) throw new ArgumentException("array");
            if (source.Count < length) throw new ArgumentException("source");
            length += sourceIndex;

            for (int i = sourceIndex; i < length; i++) {
                array[arrayIndex++] = source[i];
            }
        }

        #endregion

        #region CopyBackwardsTo methods

        public static void CopyBackwardsTo<T>(this IList<T> source, int sourceIndex, IList<T> array, int arrayIndex, int length) {
            if (array == null) throw new ArgumentNullException("array");
            if (length < 0) throw new ArgumentOutOfRangeException("length");
            if (array.Count < (arrayIndex + length)) throw new ArgumentException("array");
            if (source.Count < (sourceIndex + length)) throw new ArgumentException("source");

            for (int i = sourceIndex + length - 1; i >= sourceIndex; i--) {
                array[arrayIndex++] = source[i];
            }
        }

        public static void CopyBackwardsTo<T>(this IList<T> source, IList<T> array, int arrayIndex, int length) {
            if (array == null) throw new ArgumentNullException("array");
            if (length < 0) throw new ArgumentOutOfRangeException("length");
            if (array.Count < (arrayIndex + length)) throw new ArgumentException("array");
            if (source.Count < length) throw new ArgumentException("source");

            for (int i = length - 1; i >= 0; i--) {
                array[arrayIndex++] = source[i];
            }
        }

        public static void CopyBackwardsTo<T>(this IList<T> source, IList<T> array, int arrayIndex) {
            if (source == null) throw new ArgumentNullException("source");
            if (array == null) throw new ArgumentNullException("array");
            if (array.Count < arrayIndex) throw new ArgumentException("array");

            for (int i = source.Count - 1; i >= 0; i--) {
                array[arrayIndex++] = source[i];
            }
        }

        #endregion

        #region Exchange

        /// <summary>
        /// Replaces the value at the index with a new <paramref name="value"/> and returns the old value. 
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="source">The source list</param>
        /// <param name="value">The new value.</param>
        /// <param name="index">The index.</param>
        /// <returns>The old value at the specified index.</returns>
        public static T Exchange<T>(this IList<T> source, T value, int index) {
            var temp = source[index];
            source[index] = value;
            return temp;
        }

        /// <summary>
        /// Replaces the value at the first index, pushes its old value to the second index and then returns
        /// its old value.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="source">The source list</param>
        /// <param name="value">The new value.</param>
        /// <param name="index1">The first index.</param>
        /// <param name="index2">The second index.</param>
        /// <returns>The old value at the second index.</returns>
        public static T Exchange<T>(this IList<T> source, int index1, int index2, T value) {
            var temp = source[index2];
            source[index2] = source[index1];
            source[index1] = value;
            return temp;
        }

        #endregion

        #region Rotate

        public static int UnguardedRotate<T>(this IList<T> source, int n, int startIndex, int length) {
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
        public static int Rotate<T>(this IList<T> source, int n, int startIndex, int length) {
            if (source == null) throw new ArgumentNullException("source");
            if (length < 0) throw new ArgumentOutOfRangeException("length");
            if (source.Count < (startIndex + length)) throw new ArgumentException("source");
            if (n > length || -n > length) throw new ArgumentException("n");

            return UnguardedRotate(source, n, startIndex, length);
        }

        /// <summary>
        /// Performs a left rotation on the <paramref name="source"/>, moving the <paramref name="n"/> first elements at the end.
        /// </summary>
        /// <typeparam name="T">The type of the elemets</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="n">The number of elements to move</param>
        /// <returns>The new position of the first element</returns>
        public static int Rotate<T>(this IList<T> source, int n) {
            if (source == null) throw new ArgumentNullException("source");
            var length = source.Count;
            if (n > length || -n > length) throw new ArgumentException("n");

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
        public static void SwapItems<T>(this T[] list, int i, int j) {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        #endregion
    }
}