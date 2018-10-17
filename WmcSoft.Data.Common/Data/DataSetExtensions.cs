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
using System.Diagnostics;
using System.Linq;

namespace WmcSoft.Data
{
    public static class DataSetExtensions
    {
        #region Cast

        public static IEnumerable<R> Cast<R>(this DataView view)
            where R : DataRow
        {
            Debug.Assert(view != null);

            foreach (DataRowView r in view) {
                yield return (R)r.Row;
            }
        }

        public static IEnumerable<R> Cast<R>(this DataView view, DataRowVersion rowVersion)
            where R : DataRow
        {
            Debug.Assert(view != null);

            foreach (DataRowView r in view) {
                if (r.RowVersion == rowVersion)
                    yield return (R)r.Row;
            }
        }

        #endregion

        #region Compute

        public static T Compute<T>(this DataTable dataTable, string expression, string filter)
        {
            Debug.Assert(dataTable != null);

            var value = dataTable.Compute(expression, filter);
            return DataConvert.ChangeType<T>(value);
        }

        public static T ComputeOrDefault<T>(this DataTable dataTable, string expression, string filter)
        {
            Debug.Assert(dataTable != null);

            var value = dataTable.Compute(expression, filter);
            return DataConvert.ChangeTypeOrDefault<T>(value);
        }

        #endregion

        #region CreateUniqueName

        public static string CreateUniquePrefixedName(this IEnumerable<string> names, string prefix)
        {
            Debug.Assert(names != null);

            var set = new HashSet<string>(names.Where(n => n.StartsWith(prefix)));
            var i = 1;
            var name = prefix + i;
            while (set.Contains(name)) {
                i++;
                name = prefix + i;
            }
            return name;
        }

        public static string CreateUniqueName(this IEnumerable<string> names, string format)
        {
            Debug.Assert(names != null);

            if (format.EndsWith("{0}"))
                return names.CreateUniquePrefixedName(format.Substring(0, format.Length - 3));

            var set = new HashSet<string>(names);
            var i = 1;
            var name = string.Format(format, i);
            while (set.Contains(name)) {
                i++;
                name = string.Format(format, i);
            }
            return name;
        }

        public static string CreateUniqueName(this DataTableCollection collection, string format)
        {
            Debug.Assert(collection != null);
            return collection.OfType<DataTable>().Select(t => t.TableName).CreateUniqueName(format);
        }

        public static string CreateUniqueName(this DataColumnCollection collection, string format)
        {
            Debug.Assert(collection != null);
            return collection.OfType<DataColumn>().Select(t => t.ColumnName).CreateUniqueName(format);
        }

        #endregion

        #region RemoveAll

        public static void RemoveAll(this DataRowCollection collection, IEnumerable<DataRow> rows)
        {
            Debug.Assert(collection != null);

            if (rows != null) {
                var bin = rows.ToList(); // copy to prevent modifying the underlying collection if produced by Linq
                foreach (var row in bin)
                    collection.Remove(row);
            }
        }

        #endregion

        #region ToDictionary

        public static IDictionary<string, object> ToDictionary(this DataRow row, IDictionary<string, object> additionalData)
        {
            Debug.Assert(row != null);

            if (additionalData == null)
                return row.ToDictionary();

            var dictionary = new Dictionary<string, object>(additionalData);
            var columns = row.Table.Columns;
            foreach (DataColumn dc in columns) {
                if (row.IsNull(dc))
                    dictionary.Remove(dc.ColumnName);
                dictionary[dc.ColumnName] = row[dc];
            }
            return dictionary;
        }

        public static IDictionary<string, object> ToDictionary(this DataRow row)
        {
            Debug.Assert(row != null);

            var columns = row.Table.Columns;
            var dictionary = new Dictionary<string, object>(columns.Count);
            foreach (DataColumn dc in columns) {
                if (row.IsNull(dc))
                    continue;
                dictionary.Add(dc.ColumnName, row[dc]);
            }
            return dictionary;
        }

        #endregion

        #region Transient

        /// <summary>
        /// Represents a data column that will be removed from its table when disposed.
        /// </summary>
        class TransientDataColumn : DataColumn
        {
            public TransientDataColumn(string columnName, Type dataType, string expr)
                : base(columnName, dataType, expr)
            {
            }

            protected override void Dispose(bool disposing)
            {
                if (Table != null) {
                    Table.Columns.Remove(this);
                }
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Add to a data columns collection a column that will be removed from it on dispose.
        /// </summary>
        /// <param name="collection">The data columns collection.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="type">The data type of the column value.</param>
        /// <param name="expression">The expression for the column.</param>
        /// <returns>The column that was added to the collection.</returns>
        public static DataColumn Transient(this DataColumnCollection collection, string columnName, Type type, string expression)
        {
            Debug.Assert(collection != null);

            var t = new TransientDataColumn(columnName, type, expression);
            collection.Add(t);
            return t;
        }


        /// <summary>
        /// Add to a data columns collection an anonymous column that will be removed from it on dispose.
        /// </summary>
        /// <param name="collection">The data columns collection.</param>
        /// <param name="type">The data type of the column value.</param>
        /// <param name="expression">The expression for the column.</param>
        /// <returns>The column that was added to the collection.</returns>
        /// <remarks>If transient column with the same expression exists in the collection, it is returned.</remarks>
        public static DataColumn Transient(this DataColumnCollection collection, Type type, string expression)
        {
            Debug.Assert(collection != null);

            var found = collection.OfType<TransientDataColumn>().SingleOrDefault(c => c.Expression == expression);
            if (found != null)
                return found;
            var columnName = collection.CreateUniqueName("$expr{0}$");
            return collection.Transient(columnName, type, expression);
        }

        #endregion
    }
}
