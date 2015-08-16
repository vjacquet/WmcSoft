using System;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace WmcSoft.Data.SqlClient
{
    public static class SqlDataReaderExtensions
    {
        public static T GetObjectFromXml<T>(this SqlDataReader reader, int i)
            where T : new() {
            if (reader.IsDBNull(i))
                return new T();
            var serializer = new DataContractSerializer(typeof(T));
            using (var xml = reader.GetSqlXml(i).CreateReader()) {
                return (T)serializer.ReadObject(xml);
            }
        }

        public static T GetObjectFromXml<T>(this SqlDataReader reader, int i, IDataContractSurrogate surrogate)
            where T : new() {
            if (reader.IsDBNull(i))
                return new T();
            var serializer = new DataContractSerializer(typeof(T), new Type[0], Int16.MaxValue, false, true, surrogate);
            using (var xml = reader.GetSqlXml(i).CreateReader()) {
                return (T)serializer.ReadObject(xml);
            }
        }
    }
}
