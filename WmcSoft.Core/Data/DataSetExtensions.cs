
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Data
{
    public static class DataSetExtensions
    {
        #region Cast

        public static IEnumerable<R> Cast<R>(this DataView view) where R : DataRow {
            foreach (DataRowView r in view) {
                yield return (R)r.Row;
            }
        }

        #endregion

        #region Compute

        public static T Compute<T>(this DataTable dataTable, string expression, string filter) {
            var value = dataTable.Compute(expression, filter);
            return DataConvert.ChangeTypeOrDefault<T>(value);
        }

        #endregion

        #region CreateUniqueName

        public static string CreateUniquePrefixedName(this IEnumerable<string> names, string prefix) {
            var set = new HashSet<string>(names.Where(n => n.StartsWith(prefix)));
            var i = 1;
            var name = prefix + i;
            while (set.Contains(name)) {
                i++;
                name = prefix + i;
            }
            return name;
        }

        public static string CreateUniqueName(this IEnumerable<string> names, string format) {
            if (format.EndsWith("{0}"))
                return names.CreateUniquePrefixedName(format.Left(-3));

            var set = new HashSet<string>(names);
            var i = 1;
            var name = String.Format(format, i);
            while (set.Contains(name)) {
                i++;
                name = String.Format(format, i);
            }
            return name;
        }

        public static string CreateUniqueName(this DataTableCollection collection, string format) {
            return collection.OfType<DataTable>().Select(t => t.TableName).CreateUniqueName(format);
        }

        public static string CreateUniqueName(this DataColumnCollection collection, string format) {
            return collection.OfType<DataColumn>().Select(t => t.ColumnName).CreateUniqueName(format);
        }

        #endregion

        #region RemoveAll

        public static void RemoveAll(this DataRowCollection collection, IEnumerable<DataRow> rows) {
            foreach (var row in rows)
                collection.Remove(row);
        }

        #endregion

        #region ToDictionary

        public static IDictionary<string, object> ToDictionary(this DataRow row, IDictionary<string, object> additionalData) {
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

        public static IDictionary<string, object> ToDictionary(this DataRow row) {
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

        class TransientDataColumn : DataColumn
        {
            public TransientDataColumn(string columnName, Type dataType, string expr)
                : base(columnName, dataType, expr) {
            }

            protected override void Dispose(bool disposing) {
                if (this.Table != null) {
                    this.Table.Columns.Remove(this);
                }
                base.Dispose(disposing);
            }
        }

        public static DataColumn Transient(this DataColumnCollection collection, string columnName, Type type, string expression) {
            var t = new TransientDataColumn(columnName, type, expression);
            collection.Add(t);
            return t;
        }

        public static DataColumn Transient(this DataColumnCollection collection, Type type, string expression) {
            var found = collection.OfType<TransientDataColumn>().SingleOrDefault(c => c.Expression == expression);
            if (found != null)
                return found;

            var columnName = collection.CreateUniqueName("$expr{0}$");
            return collection.Transient(columnName, type, expression);
        }

        #endregion

        #region Where

        public static IEnumerable<DataRow> Where(this DataRowCollection collection, Func<DataRow, bool> predicate) {
            return collection.Cast<DataRow>().Where(predicate);
        }

        public static IEnumerable<DataRow> Where(this DataRowCollection collection, Func<DataRow, int, bool> predicate) {
            return collection.Cast<DataRow>().Where(predicate);
        }

        #endregion
    }
}
