using System;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace WmcSoft.Data.SqlClient
{
    /// <summary>
    /// Defines the extension methods to the <see cref="SqlDataReader"/> class.
    /// This is a static class. 
    /// </summary>
    public static class SqlDataReaderExtensions
    {
        public static T GetObjectFromXml<T>(this SqlDataReader reader, int i)
            where T : class {
            return GetObjectFromXml<T>(reader, i, new DataContractSerializer(typeof(T)));
        }

        public static T GetObjectFromXml<T>(this SqlDataReader reader, int i, IDataContractSurrogate surrogate)
            where T : class {
            return GetObjectFromXml<T>(reader, i, new DataContractSerializer(typeof(T), Type.EmptyTypes, Int16.MaxValue, false, true, surrogate));
        }

        public static T GetObjectFromXml<T>(this SqlDataReader reader, int i, XmlObjectSerializer serializer)
            where T : class {
            if (reader.IsDBNull(i))
                return null;
            using (var xml = reader.GetSqlXml(i).CreateReader()) {
                return (T)serializer.ReadObject(xml);
            }
        }
    }
}
