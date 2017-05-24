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
    public static class DataTableCollectionExtensions
    {
        public static EnumerableRowCollection<DataRow> Where(this DataTable table, Func<DataRow, bool> predicate)
        {
            return table.AsEnumerable().Where(predicate);
        }

        public static EnumerableRowCollection<U> Select<U>(this DataTable table, Func<DataRow, U> selector)
        {
            return table.AsEnumerable().Select(selector);
        }
        public static IEnumerable<V> SelectMany<U, V>(this DataTable table, Func<DataRow, IEnumerable<U>> selector, Func<DataRow, U, V> resultSelector)
        {
            return table.AsEnumerable().SelectMany(selector, resultSelector);
        }

        public static IEnumerable<V> Join<U, K, V>(this DataTable table, IEnumerable<U> inner, Func<DataRow, K> outerKeySelector, Func<U, K> innerKeySelector, Func<DataRow, U, V> resultSelector)
        {
            return table.AsEnumerable().Join(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static IEnumerable<V> GroupJoin<U, K, V>(this DataTable table, IEnumerable<U> inner, Func<DataRow, K> outerKeySelector, Func<U, K> innerKeySelector, Func<DataRow, IEnumerable<U>, V> resultSelector)
        {
            return table.AsEnumerable().GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static OrderedEnumerableRowCollection<DataRow> OrderBy<K>(this DataTable table, Func<DataRow, K> keySelector)
        {
            return table.AsEnumerable().OrderBy(keySelector);
        }

        public static OrderedEnumerableRowCollection<DataRow> OrderByDescending<K>(this DataTable table, Func<DataRow, K> keySelector)
        {
            return table.AsEnumerable().OrderByDescending(keySelector);
        }

        public static IEnumerable<IGrouping<K, DataRow>> GroupBy<K>(this DataTable table, Func<DataRow, K> keySelector)
        {
            return table.AsEnumerable().GroupBy(keySelector);
        }

        public static IEnumerable<IGrouping<K, E>> GroupBy<K, E>(this DataTable table, Func<DataRow, K> keySelector, Func<DataRow, E> elementSelector)
        {
            return table.AsEnumerable().GroupBy(keySelector, elementSelector);
        }

        public static IEnumerable<DataRow> Where(this DataTable table, Func<DataRow, int, bool> predicate)
        {
            return table.AsEnumerable().Where(predicate);
        }

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
    }
}
