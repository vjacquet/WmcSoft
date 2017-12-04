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
    /// <summary>
    /// Defines the extension methods to the <see cref="IDataRecord"/> interface.
    /// This is a static class. 
    /// </summary>
    public static class DataRecordExtensions
    {
        #region GetEnum methods

        /// <summary>
        /// Gets the value of the specified column as an enum of the given type.
        /// </summary>
        /// <typeparam name="TEnum">The tye of the enum</typeparam>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The value of the specified column.</returns>
        public static TEnum GetEnum<TEnum>(this IDataRecord record, int i)
        {
            Debug.Assert(typeof(TEnum).IsEnum);

            var value = Enum.Parse(typeof(TEnum), record.GetValue(i).ToString());
            if (Enum.IsDefined(typeof(TEnum), value))
                return (TEnum)value;
            throw new InvalidCastException();
        }

        #endregion

        #region GetNullableXxx

        /// <summary>
        /// Gets the value of the specified column as a string, or null.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The value of the specified column.</returns>
        public static string GetNullableString(this IDataRecord record, int i)
        {
            if (record.IsDBNull(i))
                return null;
            return record.GetString(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean, or null.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The value of the specified column.</returns>
        public static bool? GetNullableBoolean(this IDataRecord record, int i)
        {
            if (record.IsDBNull(i))
                return null;
            return record.GetBoolean(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a byte, or null.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The value of the specified column.</returns>
        public static byte? GetNullableByte(this IDataRecord record, int i)
        {
            if (record.IsDBNull(i))
                return null;
            return record.GetByte(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a single character, or null.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The value of the specified column.</returns>
        public static char? GetNullableChar(this IDataRecord record, int i)
        {
            if (record.IsDBNull(i))
                return null;
            return record.GetChar(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a DateTime object, or null.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The value of the specified column.</returns>
        public static DateTime? GetNullableDateTime(this IDataRecord record, int i)
        {
            if (record.IsDBNull(i))
                return null;
            return record.GetDateTime(i);
        }

        /// <summary>
        /// Get the value of the specified field as a globally unique identifier (GUID), or null.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The value of the specified column.</returns>
        public static Guid? GetNullableGuid(this IDataRecord record, int i)
        {
            if (record.IsDBNull(i))
                return null;
            return record.GetGuid(i);
        }

        /// <summary>
        /// Get the 16-bits integer value of the specified field, or null.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The value of the specified column.</returns>
        public static short? GetNullableInt16(this IDataRecord record, int i)
        {
            if (record.IsDBNull(i))
                return null;
            return record.GetInt16(i);
        }

        /// <summary>
        /// Get the 32-bits integer value of the specified field, or null.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The value of the specified column.</returns>
        public static int? GetNullableInt32(this IDataRecord record, int i)
        {
            if (record.IsDBNull(i))
                return null;
            return record.GetInt32(i);
        }

        /// <summary>
        /// Get the 64-bits integer value of the specified field, or null.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The value of the specified column.</returns>
        public static long? GetNullableInt64(this IDataRecord record, int i)
        {
            if (record.IsDBNull(i))
                return null;
            return record.GetInt64(i);
        }

        /// <summary>
        /// Get the single-precision floating point value of the specified field, or null.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The value of the specified column.</returns>
        public static float? GetNullableFloat(this IDataRecord record, int i)
        {
            if (record.IsDBNull(i))
                return null;
            return record.GetFloat(i);
        }

        /// <summary>
        /// Get the double-precision floating point value of the specified field, or null.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The value of the specified column.</returns>
        public static double? GetNullableDouble(this IDataRecord record, int i)
        {
            if (record.IsDBNull(i))
                return null;
            return record.GetDouble(i);
        }

        /// <summary>
        /// Get the fixed-position numeric value of the specified field, or null.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The value of the specified column.</returns>
        public static double? GetNullableDecimal(this IDataRecord record, int i)
        {
            if (record.IsDBNull(i))
                return null;
            return (double)record.GetDecimal(i);
        }

        #endregion

        #region GetXxxOrDefault

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value of the specified column.</returns>
        public static string GetStringOrDefault(this IDataRecord record, int i, string defaultValue = "")
        {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetString(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value of the specified column.</returns>
        public static bool GetBooleanOrDefault(this IDataRecord record, int i, bool defaultValue = false)
        {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetBoolean(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a byte, or the default value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value of the specified column.</returns>
        public static byte GetByteOrDefault(this IDataRecord record, int i, byte defaultValue = default(byte))
        {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetByte(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a single character, or the default value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value of the specified column.</returns>
        public static char GetCharOrDefault(this IDataRecord record, int i, char defaultValue = default(char))
        {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetChar(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a DateTime object.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value of the specified column.</returns>
        public static DateTime GetDateTimeOrDefault(this IDataRecord record, int i, DateTime defaultValue = default(DateTime))
        {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetDateTime(i);
        }

        /// <summary>
        /// Get the value of the specified field as a globally unique identifier (GUID), or a default value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value of the specified column.</returns>
        public static Guid GetGuidOrDefault(this IDataRecord record, int i, Guid defaultValue = default(Guid))
        {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetGuid(i);
        }

        /// <summary>
        /// Get the 16-bits integer value of the specified field, or the default value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value of the specified column.</returns>
        public static short GetInt16OrDefault(this IDataRecord record, int i, short defaultValue = 0)
        {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetInt16(i);
        }

        /// <summary>
        /// Get the 32-bits integer value of the specified field, or the default value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value of the specified column.</returns>
        public static int GetInt32OrDefault(this IDataRecord record, int i, int defaultValue = 0)
        {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetInt32(i);
        }

        /// <summary>
        /// Get the 64-bits integer value of the specified field, or the default value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value of the specified column.</returns>
        public static long GetInt64OrDefault(this IDataRecord record, int i, long defaultValue = 0)
        {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetInt64(i);
        }

        /// <summary>
        /// Get the single-precision floating point value of the specified field, or the default value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value of the specified column.</returns>
        public static float GetFloatOrDefault(this IDataRecord record, int i, float defaultValue = 0f)
        {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetFloat(i);
        }

        /// <summary>
        /// Get the double-precision floating point value of the specified field, or the default value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value of the specified column.</returns>
        public static double GetDoubleOrDefault(this IDataRecord record, int i, double defaultValue = 0d)
        {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetDouble(i);
        }

        /// <summary>
        /// Get the fixed-position numeric value of the specified field, or the default value.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value of the specified column.</returns>
        public static decimal GetDecimalOrDefault(this IDataRecord record, int i, decimal defaultValue = 0m)
        {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetDecimal(i);
        }

        #endregion

        #region Materialize

        public static IEnumerable<T> Materialize<T>(this IEnumerable<IDataRecord> source, Func<IDataRecord, T> materializer)
        {
            return source.Select(materializer);
        }

        /// <summary>
        /// Gets the string value of the specified column for the given records.
        /// </summary>
        /// <param name="source">The records;</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The enumeration of the string value of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<string> GetString(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetString(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a string, or null, for all records.
        /// </summary>
        /// <param name="source">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The enumeration of the string value of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<string> GetNullableString(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetNullableString(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a string, or the default value, for all records.
        /// </summary>
        /// <param name="source">The record.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value of the specified column.</returns>
        public static IEnumerable<string> GetStringOrDefault(this IEnumerable<IDataRecord> source, int i = 0, string defaultValue = "")
        {
            return Materialize(source, r => r.GetStringOrDefault(i, defaultValue));
        }

        /// <summary>
        /// Gets the boolean value of the specified column for the given records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<bool> GetBoolean(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetBoolean(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a boolean, or null, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<bool?> GetNullableBoolean(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetNullableBoolean(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a boolean, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        public static IEnumerable<bool> GetBooleanOrDefault(this IEnumerable<IDataRecord> source, int i = 0, bool defaultValue = false)
        {
            return Materialize(source, r => r.GetBooleanOrDefault(i, defaultValue));
        }

        /// <summary>
        /// Gets the byte value of the specified column for the given records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<byte> GetByte(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetByte(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a byte, or null, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<byte?> GetNullableByte(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetNullableByte(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a byte, or the default value, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        public static IEnumerable<byte> GetByteOrDefault(this IEnumerable<IDataRecord> source, int i = 0, byte defaultValue = default(byte))
        {
            return Materialize(source, r => r.GetByteOrDefault(i, defaultValue));
        }

        /// <summary>
        /// Gets the char value of the specified column for the given records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<char> GetChar(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetChar(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a char, or null, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<char?> GetNullableChar(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetNullableChar(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a char, or the default value, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        public static IEnumerable<char> GetCharOrDefault(this IEnumerable<IDataRecord> source, int i = 0, char defaultValue = default(char))
        {
            return Materialize(source, r => r.GetCharOrDefault(i, defaultValue));
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> value of the specified column for the given records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<DateTime> GetDateTime(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetDateTime(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="DateTime"/>, or null, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<DateTime?> GetNullableDateTime(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetNullableDateTime(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="DateTime"/>, or the default value, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        public static IEnumerable<DateTime> GetDateTimeOrDefault(this IEnumerable<IDataRecord> source, int i = 0, DateTime defaultValue = default(DateTime))
        {
            return Materialize(source, r => r.GetDateTimeOrDefault(i, defaultValue));
        }

        /// <summary>
        /// Gets the <see cref="Guid"/> value of the specified column for the given records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<Guid> GetGuid(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetGuid(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Guid"/>, or null, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<Guid?> GetNullableGuid(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetNullableGuid(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Guid"/>, or the default value, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        public static IEnumerable<Guid> GetGuidOrDefault(this IEnumerable<IDataRecord> source, int i = 0, Guid defaultValue = default(Guid))
        {
            return Materialize(source, r => r.GetGuidOrDefault(i, defaultValue));
        }

        /// <summary>
        /// Gets the <see cref="short"/> value of the specified column for the given records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<short> GetInt16(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetInt16(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int16"/>, or null, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<short?> GetNullableInt16(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetNullableInt16(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int16"/>, or the default value, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        public static IEnumerable<short> GetInt16OrDefault(this IEnumerable<IDataRecord> source, int i = 0, short defaultValue = default(short))
        {
            return Materialize(source, r => r.GetInt16OrDefault(i, defaultValue));
        }

        /// <summary>
        /// Gets the <see cref="int"/> value of the specified column for the given records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<int> GetInt32(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetInt32(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="int"/>, or null, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<int?> GetNullableInt32(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetNullableInt32(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int32"/>, or the default value, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        public static IEnumerable<int> GetInt32OrDefault(this IEnumerable<IDataRecord> source, int i = 0, int defaultValue = default(int))
        {
            return Materialize(source, r => r.GetInt32OrDefault(i, defaultValue));
        }

        /// <summary>
        /// Gets the <see cref="long/> value of the specified column for the given records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<long> GetInt64(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetInt64(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="long"/>, or null, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<long?> GetNullableInt64(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetNullableInt64(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="long"/>, or the default value, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        public static IEnumerable<long> GetInt64OrDefault(this IEnumerable<IDataRecord> source, int i = 0, long defaultValue = default(long))
        {
            return Materialize(source, r => r.GetInt64OrDefault(i, defaultValue));
        }

        /// <summary>
        /// Gets the <see cref="float"/> value of the specified column for the given records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<float> GetFloat(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetFloat(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="float"/>, or null, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<float?> GetNullableFloat(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetNullableFloat(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="float"/>, or the default value, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        public static IEnumerable<float> GetFloatOrDefault(this IEnumerable<IDataRecord> source, int i = 0, float defaultValue = default(float))
        {
            return Materialize(source, r => r.GetFloatOrDefault(i, defaultValue));
        }

        /// <summary>
        /// Gets the <see cref="double"/> value of the specified column for the given records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<double> GetDouble(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetDouble(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="double"/>, or null, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<double?> GetNullableDouble(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetNullableDouble(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="double"/>, or the default value, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        public static IEnumerable<double> GetDoubleOrDefault(this IEnumerable<IDataRecord> source, int i = 0, double defaultValue = default(double))
        {
            return Materialize(source, r => r.GetDoubleOrDefault(i, defaultValue));
        }

        /// <summary>
        /// Gets the <see cref="Decimal"/> value of the specified column for the given records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<decimal> GetDecimal(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetDecimal(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Decimal"/>, or null, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public static IEnumerable<decimal?> GetNullableDecimal(this IEnumerable<IDataRecord> source, int i = 0)
        {
            return Materialize(source, r => r.GetNullableDecimal(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Decimal"/>, or the default value, for all records.
        /// </summary>
        /// <param name="source">The records.</param>
        /// <param name="i">The zero-based column ordinal. </param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The enumeration of the values of the specified column.</returns>
        public static IEnumerable<decimal> GetDecimalOrDefault(this IEnumerable<IDataRecord> source, int i = 0, decimal defaultValue = default(decimal))
        {
            return Materialize(source, r => r.GetDecimalOrDefault(i, defaultValue));
        }

        #endregion
    }
}
