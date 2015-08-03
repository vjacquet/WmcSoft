
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
        public static IEnumerable<DataRow> Where(this DataRowCollection collection, Func<DataRow, bool> predicate) {
            return collection.Cast<DataRow>().Where(predicate);
        }

        public static IEnumerable<DataRow> Where(this DataRowCollection collection, Func<DataRow, int, bool> predicate) {
            return collection.Cast<DataRow>().Where(predicate);
        }
    }
}
