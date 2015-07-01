using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Data.SqlClient;

namespace WmcSoft.Data
{
    public static class DbDataReaderExtensions
    {
        #region GetObjectFromXml

        public static T GetObjectFromXml<T>(this DbDataReader reader, int i)
            where T : new() {
            var sqlClient = reader as SqlDataReader;
            if (sqlClient != null)
                return SqlDataReaderExtensions.GetObjectFromXml<T>(sqlClient, i);
            return DataRecordExtensions.GetObjectFromXml<T>(reader, i);
        }

        public static T GetObjectFromXml<T>(this DbDataReader reader, int i, IDataContractSurrogate surrogate)
            where T : new() {
            var sqlClient = reader as SqlDataReader;
            if (sqlClient != null)
                return SqlDataReaderExtensions.GetObjectFromXml<T>(sqlClient, i, surrogate);
            return DataRecordExtensions.GetObjectFromXml<T>(reader, i, surrogate);
        }

        #endregion

        #region ReadXxx

        public static bool ReadNext<T>(this IDataReader reader, Func<IDataRecord, T> materializer, out T entity) {
            if (reader.Read()) {
                entity = materializer(reader);
                return true;
            }

            entity = default(T);
            return false;
        }

        public static IEnumerable<T> ReadAll<T>(this IDataReader reader, Func<IDataRecord, T> materializer) {
            T entity;
            while (reader.ReadNext(materializer, out entity))
                yield return entity;
        }

        #endregion
    }
}
