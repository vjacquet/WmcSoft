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
using System.Linq;
using System.Collections.Generic;

namespace WmcSoft
{
    public static class ArrayExtensions
    {
        #region AsEnumerable

        public static IEnumerable<T> AsEnumerable<T>(this Array array) {
            return array.Cast<T>();
        }

        #endregion

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

        #region ConvertAll

        /// <summary>
        /// Converts an array of one type to an array of another type.
        /// </summary>
        /// <typeparam name="TInput">The type of the elements of the source array.</typeparam>
        /// <typeparam name="TOutput">The type of the elements of the target array.</typeparam>
        /// <param name="array">The one-dimensional, zero-based Array to convert to a target type.</param>
        /// <param name="converter">A Converter<TInput, TOutput> that converts each element from one type to another type.</param>
        /// <returns>An array of the target type containing the converted elements from the source array.</returns>
        public static TOutput[] ConvertAll<TInput, TOutput>(this TInput[] array, Converter<TInput, TOutput> converter) {
            return Array.ConvertAll(array, converter);
        }

        #endregion

        #region Multidimensional

        public static bool StructuralEqual<T>(this T[] x, T[] y, IEqualityComparer<T> comparer = null) {
            if (x == null)
                return y == null;

            if (x.Length != y.Length)
                return false;

            return x.SequenceEqual(y, comparer ?? EqualityComparer<T>.Default);
        }

        public static bool StructuralEqual<T>(this T[,] x, T[,] y, IEqualityComparer<T> comparer = null) {
            if (x == null)
                return y == null;

            var rows = x.GetLength(0);
            var columns = x.GetLength(1);
            if (rows != y.GetLength(0) || columns != y.GetLength(1))
                return false;

            var equalityComparer = comparer ?? EqualityComparer<T>.Default;

            for (var j = 0; j < columns; j++) {
                for (var i = 0; i < rows; i++) {
                    if (!equalityComparer.Equals(x[i, j], y[i, j]))
                        return false;
                }
            }
            return true;
        }

        public static IEnumerable<T> GetColumn<T>(this T[,] array, int column) {
            var rows = array.GetLength(0);
            for (var i = 0; i < rows; i++) {
                yield return array[i, column];
            }
        }

        public static IEnumerable<T> GetRow<T>(this T[,] array, int row) {
            var columns = array.GetLength(1);
            for (var j = 0; j < columns; j++) {
                yield return array[row, j];
            }
        }

        public static IEnumerable<T[]> GetColumns<T>(this T[,] array) {
            var rows = array.GetLength(0);
            var columns = array.GetLength(1);
            for (var j = 0; j < columns; j++) {
                var column = new T[rows];
                for (var i = 0; i < rows; i++) {
                    column[i] = array[i, j];
                }
                yield return column;
            }
        }

        public static IEnumerable<T[]> GetRows<T>(this T[,] array) {
            var rows = array.GetLength(0);
            var columns = array.GetLength(1);
            for (var i = 0; i < rows; i++) {
                var row = new T[columns];
                for (var j = 0; j < columns; j++) {
                    row[j] = array[i, j];
                }
                yield return row;
            }
        }

        public static IEnumerable<int> EnumerateDimensions(this Array array) {
            if (array == null)
                yield break;

            var rank = array.Rank;
            for (int i = 0; i < rank; i++) {
                yield return array.GetLength(i);
            }
        }

        public static int[] GetDimensions(this Array array) {
            var rank = array.Rank;
            var dimensions = new int[rank];
            for (int i = 0; i < rank; i++) {
                dimensions[i] = array.GetLength(i);
            }
            return dimensions;
        }

        #endregion

        #region SwapItems

        public static void SwapItems<T>(this T[] array, int i, int j) {
            var t = array[i];
            array[i] = array[j];
            array[j] = t;
        }

        #endregion
    }
}
