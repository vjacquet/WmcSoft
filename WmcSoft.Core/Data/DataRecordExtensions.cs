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
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace WmcSoft.Data
{
    public static class DataRecordExtensions
    {
        #region GetEnum methods

        public static T GetEnum<T>(this IDataRecord record, int ordinal) {
            Debug.Assert(typeof(T).IsEnum);

            var value = Enum.Parse(typeof(T), record.GetValue(ordinal).ToString());
            if (Enum.IsDefined(typeof(T), value))
                return (T)value;
            throw new InvalidCastException();
        }

        #endregion

        #region GetNullableXxx

        public static string GetNullableString(this IDataRecord record, int i) {
            if (record.IsDBNull(i))
                return null;
            return record.GetString(i);
        }

        public static bool? GetNullableBoolean(this IDataRecord record, int i) {
            if (record.IsDBNull(i))
                return null;
            return record.GetBoolean(i);
        }

        public static DateTime? GetNullableDateTime(this IDataRecord record, int i) {
            if (record.IsDBNull(i))
                return null;
            return record.GetDateTime(i);
        }

        public static short? GetNullableInt16(this IDataRecord record, int i) {
            if (record.IsDBNull(i))
                return null;
            return record.GetInt16(i);
        }

        public static int? GetNullableInt32(this IDataRecord record, int i) {
            if (record.IsDBNull(i))
                return null;
            return record.GetInt32(i);
        }

        public static long? GetNullableInt64(this IDataRecord record, int i) {
            if (record.IsDBNull(i))
                return null;
            return record.GetInt64(i);
        }

        public static float? GetNullableFloat(this IDataRecord record, int i) {
            if (record.IsDBNull(i))
                return null;
            return record.GetFloat(i);
        }

        public static double? GetNullableDouble(this IDataRecord record, int i) {
            if (record.IsDBNull(i))
                return null;
            return record.GetDouble(i);
        }

        public static double? GetNullableDecimal(this IDataRecord record, int i) {
            if (record.IsDBNull(i))
                return null;
            return (double)record.GetDecimal(i);
        }

        #endregion

        #region GetXxxOrDefault

        public static string GetStringOrDefault(this IDataRecord record, int i, string defaultValue = "") {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetString(i);
        }

        public static bool GetBooleanOrDefault(this IDataRecord record, int i, bool defaultValue = false) {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetBoolean(i);
        }

        public static DateTime? GetDateTimeOrDefault(this IDataRecord record, int i, DateTime defaultValue = new DateTime()) {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetDateTime(i);
        }

        public static short? GetInt16OrDefault(this IDataRecord record, int i, short defaultValue = 0) {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetInt16(i);
        }

        public static int? GetInt32OrDefault(this IDataRecord record, int i, int defaultValue = 0) {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetInt32(i);
        }

        public static long? GetInt64OrDefault(this IDataRecord record, int i, long defaultValue = 0) {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetInt64(i);
        }

        public static float? GetFloatOrDefault(this IDataRecord record, int i, float defaultValue = 0f) {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetFloat(i);
        }

        public static double? GetDoubleOrDefault(this IDataRecord record, int i, double defaultValue = 0d) {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetDouble(i);
        }

        public static decimal? GetDecimalOrDefault(this IDataRecord record, int i, decimal defaultValue = 0m) {
            if (record.IsDBNull(i))
                return defaultValue;
            return (decimal)record.GetDecimal(i);
        }

        #endregion

        #region GetObjectFromXml

        public static T GetObjectFromXml<T>(this IDataRecord record, int i)
            where T : new() {
            var xml = record.GetValue(i).ToString();
            if (String.IsNullOrEmpty(xml))
                return new T();
            var serializer = new DataContractSerializer(typeof(T));
            using (var reader = XmlReader.Create(new StringReader(xml))) {
                return (T)serializer.ReadObject(reader);
            }
        }

        public static T GetObjectFromXml<T>(this IDataRecord record, int i, IDataContractSurrogate surrogate)
            where T : new() {
            var xml = record.GetValue(i).ToString();
            if (String.IsNullOrEmpty(xml))
                return new T();
            var serializer = new DataContractSerializer(typeof(T), new Type[0], Int16.MaxValue, false, true, surrogate);
            using (var reader = XmlReader.Create(new StringReader(xml))) {
                return (T)serializer.ReadObject(reader);
            }
        }
        #endregion
    }
}
