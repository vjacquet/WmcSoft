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
using WmcSoft.Collections.Generic;
using System.Collections;

namespace WmcSoft
{
    /// <summary>
    /// Defines the extension methods to the array class. This is a static class.
    /// </summary>
    public static class ArrayExtensions
    {
        #region AsEnumerable

        public static IEnumerable<T> AsEnumerable<T>(this Array array)
        {
            return array.Cast<T>();
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
        public static TOutput[] ConvertAll<TInput, TOutput>(this TInput[] array, Converter<TInput, TOutput> converter)
        {
            return Array.ConvertAll(array, converter);
        }

        #endregion

        #region Flatten

        public static T[] Flatten<T>(this T[][] jagged)
        {
            var count = 0;
            var length = jagged.Length;
            for (int i = 0; i < length; i++) {
                var row = jagged[i];
                if (row != null)
                    count += row.Length;
            }

            var index = 0;
            var result = new T[count];
            for (int i = 0; i < length; i++) {
                var row = jagged[i];
                if (row != null) {
                    Array.Copy(row, 0, result, index, row.Length);
                    index += row.Length;
                }
            }
            return result;
        }

        #endregion

        #region GetEnumerator

        public struct RangeEnumerator<T> : IEnumerator<T>
        {
            private readonly T[] _storage;
            private readonly int _begin;
            private readonly int _end;
            private int _index;
            private T _current;

            public RangeEnumerator(T[] array, int startIndex, int length)
            {
                if (array == null) throw new ArgumentNullException(nameof(array));
                if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
                if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
                if (array.Length < (startIndex + length)) throw new ArgumentException(nameof(array));

                _storage = array;
                _begin = startIndex;
                _end = startIndex + length;
                _index = _begin;
                _current = default(T);
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_index < _end) {
                    _current = _storage[_index];
                    ++_index;
                    return true;
                }
                _current = default(T);
                return false;
            }

            public void Reset()
            {
                _index = _begin;
                _current = default(T);
            }

            public T Current {
                get { return _current; }
            }

            object IEnumerator.Current {
                get { return Current; }
            }
        }

        public static RangeEnumerator<T> GetEnumerator<T>(this T[] array, int startIndex, int length)
        {
            return new RangeEnumerator<T>(array, startIndex, length);
        }

        #endregion

        #region Multidimensional

        public static bool StructuralEqual<T>(this T[] x, T[] y, IEqualityComparer<T> comparer = null)
        {
            if (x == null)
                return y == null;

            if (x.Length != y.Length)
                return false;

            return x.SequenceEqual(y, comparer ?? EqualityComparer<T>.Default);
        }

        public static bool StructuralEqual<T>(this T[,] x, T[,] y, IEqualityComparer<T> comparer = null)
        {
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

        public static IEnumerable<T> GetColumn<T>(this T[,] array, int column)
        {
            var rows = array.GetLength(0);
            for (var i = 0; i < rows; i++) {
                yield return array[i, column];
            }
        }

        public static IEnumerable<T> GetRow<T>(this T[,] array, int row)
        {
            var columns = array.GetLength(1);
            for (var j = 0; j < columns; j++) {
                yield return array[row, j];
            }
        }

        public static IEnumerable<T[]> GetColumns<T>(this T[,] array)
        {
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

        public static IEnumerable<T[]> GetRows<T>(this T[,] array)
        {
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

        /// <summary>
        /// Creates a new array for which the row and column indices are switched.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns>A new transposed array</returns>
        public static T[,] Transpose<T>(this T[,] array)
        {
            var rows = array.GetLength(1);
            var columns = array.GetLength(0);
            var result = new T[rows, columns];
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    result[i, j] = array[j, i];
                }
            }
            return result;
        }

        /// <summary>
        /// Creates a multi-dimensional array from a jagged-array.
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="array">The jagged array.</param>
        /// <returns>The multi-dimensional array.</returns>
        /// <remarks>The number of columns is the maximum of items in each array. The missing cells are filled with <c>default(T)</c>.</remarks>
        public static T[,] ToMultiDimensional<T>(this T[][] array)
        {
            var rows = array.Length;
            var columns = array.Max(i => i.Length);
            var result = new T[rows, columns];
            for (int i = 0; i < rows; i++) {
                var row = array[i];
                for (int j = 0; j < row.Length; j++) {
                    result[i, j] = row[j];
                }
            }
            return result;
        }

        /// <summary>
        /// Enumerate the length of each dimension of the <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>The length of each dimension.</returns>
        public static IEnumerable<int> EnumerateDimensions(this Array array)
        {
            var rank = array.Rank;
            for (int i = 0; i < rank; i++) {
                yield return array.GetLength(i);
            }
        }

        /// <summary>
        /// Get the length of each dimension of the <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>The length of each dimension.</returns>
        public static int[] GetDimensions(this Array array)
        {
            var rank = array.Rank;
            var dimensions = new int[rank];
            for (int i = 0; i < rank; i++) {
                dimensions[i] = array.GetLength(i);
            }
            return dimensions;
        }

        #endregion

        #region Path

        /// <summary>
        /// Enumerates the items of the <paramref name="source"/> at the specified <paramref name="indices"/>.
        /// </summary>
        /// <typeparam name="T">The type of items</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="indices">The indices of the items in the array</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of the items at the specified <paramref name="indices"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when an indice is not in the array.</exception>
        public static IEnumerable<T> Path<T>(this T[] source, params int[] indices)
        {
            var length = indices.Length;
            for (int i = 0; i < length; i++) {
                yield return source[indices[i]];
            }
        }

        #endregion

        #region Replace / ReplaceIf

        public static void UnguardedReplace<T>(this T[] array, int startIndex, int length, T oldValue, T newValue, IEqualityComparer<T> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            var endIndex = startIndex + length;
            for (int i = startIndex; i < endIndex; i++) {
                if (comparer.Equals(array[i], oldValue))
                    array[i] = newValue;
            }
        }

        /// <summary>
        /// Replaces all occurences of <paramref name="oldValue"/> within a specified range with <paramref name="newValue"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="startIndex">The zero-based index at which the range starts.</param>
        /// <param name="length">The number of elements in the range.</param>
        /// <param name="oldValue">The value to be replaced.</param>
        /// <param name="newValue">The new value.</param>
        public static void Replace<T>(this T[] array, int startIndex, int length, T oldValue, T newValue, IEqualityComparer<T> comparer = null)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (array.Length < (startIndex + length)) throw new ArgumentException(nameof(array));

            UnguardedReplace(array, startIndex, length, oldValue, newValue, comparer);
        }

        /// <summary>
        /// Replaces all occurences of <paramref name="oldValue"/> with <paramref name="newValue"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="oldValue">The value to be replaced.</param>
        /// <param name="newValue">The new value.</param>
        public static void Replace<T>(this T[] array, T oldValue, T newValue, IEqualityComparer<T> comparer = null)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            UnguardedReplace(array, 0, array.Length, oldValue, newValue, comparer);
        }

        public static void UnguardedReplaceIf<T>(this T[] array, int startIndex, int length, Predicate<T> predicate, T newValue)
        {
            var endIndex = startIndex + length;
            for (int i = startIndex; i < endIndex; i++) {
                if (predicate(array[i]))
                    array[i] = newValue;
            }
        }

        /// <summary>
        /// Replaces elements within the specified range matching the <paramref name="predicate"/> with <paramref name="newValue"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="startIndex">The zero-based index at which the range starts.</param>
        /// <param name="length">The number of elements in the range.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="newValue">The new value.</param>
        public static void ReplaceIf<T>(this T[] array, int startIndex, int length, Predicate<T> predicate, T newValue)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (array.Length < (startIndex + length)) throw new ArgumentException(nameof(array));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            UnguardedReplaceIf(array, startIndex, length, predicate, newValue);
        }

        /// <summary>
        /// Replaces all elements matching the <paramref name="predicate"/> with <paramref name="newValue"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="newValue">The new value.</param>
        public static void ReplaceIf<T>(this T[] array, Predicate<T> predicate, T newValue)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            UnguardedReplaceIf(array, 0, array.Length, predicate, newValue);
        }

        #endregion

        #region Rotate

        public static int UngardedRotate<T>(this T[] source, int n, int startIndex, int length)
        {
            if (n == 0)
                return startIndex;

            if (n < 0) {
                n = -n;
                var temp = new T[n];
                // rotate left
                Array.Copy(source, startIndex, temp, 0, n);
                Array.Copy(source, startIndex + n, source, startIndex, length - n);
                startIndex += length - n;
                Array.Copy(temp, 0, source, startIndex, n);
            } else {
                var temp = new T[n];
                // rotate right
                Array.Copy(source, startIndex + length - n, temp, 0, n);
                Array.Copy(source, startIndex, source, startIndex + n, length - n);
                Array.Copy(temp, 0, source, startIndex, n);
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
        public static int Rotate<T>(this T[] source, int n, int startIndex, int length)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (source.Length < (startIndex + length)) throw new ArgumentException(nameof(source));
            if (n > length || -n > length) throw new ArgumentException(nameof(n));

            return UngardedRotate(source, n, startIndex, length);
        }

        /// <summary>
        /// Performs a left rotation on the <paramref name="source"/>, moving the <paramref name="n"/> first elements of the specified range 
        /// at the end.
        /// </summary>
        /// <typeparam name="T">The type of the elemets</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="n">The number of elements to move</param>
        /// <returns>The new position of the <paramref name="startIndex"/>.</returns>
        public static int Rotate<T>(this T[] source, int n)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var length = source.Length;
            if (n > length || -n > length) throw new ArgumentException(nameof(n));

            return UngardedRotate(source, n, 0, length);
        }

        #endregion

        #region Sort methods

        /// <summary>
        /// Sorts the elements in an entire Array using the <see cref="IComparable{T}"/> generic interface implementation of each element of the Array.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The one-dimensional, zero-based Array to sort.</param>
        /// <returns>The one-dimensional, zero-based sorted Array</returns>
        public static T[] Sort<T>(this T[] array)
        {
            Array.Sort(array);
            return array;
        }

        /// <summary>
        /// Sorts the elements in an Array using the specified <see cref="Comparison{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The one-dimensional, zero-based Array to sort.</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
        /// <returns>The one-dimensional, zero-based sorted Array</returns>
        public static T[] Sort<T>(this T[] array, Comparison<T> comparison)
        {
            Array.Sort(array, comparison);
            return array;
        }

        /// <summary>
        /// Sorts the elements in an Array using the specified <see cref="IComparer{T}"/> generic interface.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The one-dimensional, zero-based Array to sort.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> generic interface implementation to use when comparing elements, or null to use the IComparable<T> generic interface implementation of each element.</param>
        /// <returns>The one-dimensional, zero-based sorted Array</returns>
        public static T[] Sort<T>(this T[] array, IComparer<T> comparer)
        {
            Array.Sort(array, comparer);
            return array;
        }

        /// <summary>
        /// Sorts the range of elements from an array, defined by the start index and the count of elements, using the given comparison function.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="index">The start index</param>
        /// <param name="length">The length of the range to sort</param>
        /// <param name="comparer">The comparison function.</param>
        public static T[] Sort<T>(this T[] array, int index, int length, Comparison<T> comparison)
        {
            Array.Sort(array, index, length, Comparer<T>.Create(comparison));
            return array;
        }

        /// <summary>
        /// Sorts the range of elements from an array, defined by the start index and the count of elements, using the given comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="index">The start index</param>
        /// <param name="length">The length of the range to sort</param>
        /// <param name="comparer">The comparer object.</param>
        public static T[] Sort<T>(this T[] array, int index, int length, IComparer<T> comparer)
        {
            Array.Sort(array, index, length, comparer);
            return array;
        }
        #endregion

        #region SortBackwards methods

        /// <summary>
        /// Sorts all the elements in the array in backwards order.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source array</param>
        public static T[] SortBackwards<T>(this T[] array)
        {
            SortBackwards(array, Comparer<T>.Default);
            return array;
        }

        /// <summary>
        /// Sorts all the elements in the array in backwards order, using the given comparison.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="comparison">The comparison function.</param>
        public static T[] SortBackwards<T>(this T[] array, Comparison<T> comparison)
        {
            array.Sort((x, y) => comparison(y, x));
            return array;
        }

        /// <summary>
        /// Sorts all the elements in the array in backwards order, using the given comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="comparer">The comparer object.</param>
        public static T[] SortBackwards<T>(this T[] array, IComparer<T> comparer)
        {
            Array.Sort(array, new ReverseComparer<T>(comparer));
            return array;
        }

        /// <summary>
        /// Sorts in backwards order the range of elements from an array, defined by the start index and the count of elements, using the given comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="source">The source array</param>
        /// <param name="index">The start index</param>
        /// <param name="length">The length of the range to ort</param>
        /// <param name="comparer">The comparer object.</param>
        public static T[] SortBackwards<T>(this T[] array, int index, int length, IComparer<T> comparer)
        {
            Array.Sort(array, index, length, new ReverseComparer<T>(comparer));
            return array;
        }

        #endregion
    }
}
