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
using System.Data;
using System.Linq;

namespace WmcSoft.Data
{
    /// <summary>
    /// Extensions method to provide Linq facilities to <see cref="DataTable"/>s.
    /// This is a static class.
    /// </summary>
    public static class DataTableCollectionExtensions
    {
        #region Linq

        /// <summary>
        /// Filters a sequence of values based on a predicate. Each element's index is used in the logic of the predicate function.
        /// </summary>
        /// <param name="source">A <see cref="DataTable"/> to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A <see cref="EnumerableRowCollection{DataRow}"/> that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is <code>null</code>.</exception>
        public static EnumerableRowCollection<DataRow> Where(this DataTable source, Func<DataRow, bool> predicate)
        {
            return source.AsEnumerable().Where(predicate);
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate. Each element's index is used in the logic of the predicate function.
        /// </summary>
        /// <param name="source">A <see cref="DataTable"/> to filter.</param>
        /// <param name="predicate">A function to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
        /// <returns>A <see cref="EnumerableRowCollection{DataRow}"/> that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is <code>null</code>.</exception>
        public static IEnumerable<DataRow> Where(this DataTable table, Func<DataRow, int, bool> predicate)
        {
            return table.AsEnumerable().Where(predicate);
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="U">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">A <see cref="DataTable"/> to to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An <see cref="EnumerableRowCollection{DataRow}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <code>null</code>.</exception>
        public static EnumerableRowCollection<U> Select<U>(this DataTable source, Func<DataRow, U> selector)
        {
            return source.AsEnumerable().Select(selector);
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="U">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">A <see cref="DataTable"/> to to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An <see cref="IEnumerable{U}"/> whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <code>null</code>.</exception>
        public static IEnumerable<U> Select<U>(this DataTable source, Func<DataRow, int, U> selector)
        {
            return source.AsEnumerable().Select(selector);
        }

        public static IEnumerable<V> SelectMany<U, V>(this DataTable source, Func<DataRow, IEnumerable<U>> selector, Func<DataRow, U, V> resultSelector)
        {
            return source.AsEnumerable().SelectMany(selector, resultSelector);
        }

        public static IEnumerable<V> Join<U, K, V>(this DataTable source, IEnumerable<U> inner, Func<DataRow, K> outerKeySelector, Func<U, K> innerKeySelector, Func<DataRow, U, V> resultSelector)
        {
            return source.AsEnumerable().Join(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static IEnumerable<V> GroupJoin<U, K, V>(this DataTable source, IEnumerable<U> inner, Func<DataRow, K> outerKeySelector, Func<U, K> innerKeySelector, Func<DataRow, IEnumerable<U>, V> resultSelector)
        {
            return source.AsEnumerable().GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="K">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">A <see cref="DataTable"/> to sort.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>An <see cref="OrderedEnumerableRowCollection{DataRow}"/><TElement> whose elements are sorted according to a key.</returns>
        public static OrderedEnumerableRowCollection<DataRow> OrderBy<K>(this DataTable source, Func<DataRow, K> keySelector)
        {
            return source.AsEnumerable().OrderBy(keySelector);
        }

        /// <summary>
        /// Sorts the rows of a <see cref="DataTable"/> in descending order according to the specified key. 
        /// </summary>
        /// <typeparam name="K">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">A <see cref="DataTable"/> to sort.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>An <see cref="OrderedEnumerableRowCollection{DataRow}"/><TElement> whose elements are sorted according to a key.</returns>
        public static OrderedEnumerableRowCollection<DataRow> OrderByDescending<K>(this DataTable source, Func<DataRow, K> keySelector)
        {
            return source.AsEnumerable().OrderByDescending(keySelector);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function.
        /// </summary>
        /// <typeparam name="K">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <param name="source">A <see cref="DataTable"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns></returns>
        public static IEnumerable<IGrouping<K, DataRow>> GroupBy<K>(this DataTable source, Func<DataRow, K> keySelector)
        {
            return source.AsEnumerable().GroupBy(keySelector);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function and projects the elements for each group by using a specified function.
        /// </summary>
        /// <typeparam name="K">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
        /// <typeparam name="E">The type of the elements in the <see cref="IGrouping{K, E}"/>.</typeparam>
        /// <param name="source">A <see cref="DataTable"/> whose elements to group.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in the <see cref="IGrouping{K, E}"/>.</param>
        /// <returns>An <see cref="IEnumerable{IGrouping{K, E}}"/> where each <see cref="IGrouping{K, E}"/> object contains a collection of objects of type TElement and a key.</returns>
        public static IEnumerable<IGrouping<K, E>> GroupBy<K, E>(this DataTable source, Func<DataRow, K> keySelector, Func<DataRow, E> elementSelector)
        {
            return source.AsEnumerable().GroupBy(keySelector, elementSelector);
        }

        #endregion

        #region AddColumn

        public static DataColumn AddColumn<T>(this DataTable table, string columnName)
        {
            return table.Columns.Add(columnName, typeof(T));
        }

        public static DataColumn AddColumn<T>(this DataTable table, string columnName, T defaultValue)
        {
            var column = table.Columns.Add(columnName, typeof(T));
            column.DefaultValue = defaultValue;
            return column;
        }

        #endregion
    }
}
