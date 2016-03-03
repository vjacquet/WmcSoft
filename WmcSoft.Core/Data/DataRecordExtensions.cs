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
using System.Data;
using System.Diagnostics;

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
        public static TEnum GetEnum<TEnum>(this IDataRecord record, int i) {
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
        public static string GetNullableString(this IDataRecord record, int i) {
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
        public static bool? GetNullableBoolean(this IDataRecord record, int i) {
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
        public static byte? GetNullableByte(this IDataRecord record, int i) {
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
        public static char? GetNullableChar(this IDataRecord record, int i) {
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
        public static DateTime? GetNullableDateTime(this IDataRecord record, int i) {
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
        public static Guid? GetNullableGuid(this IDataRecord record, int i) {
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
        public static short? GetNullableInt16(this IDataRecord record, int i) {
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
        public static int? GetNullableInt32(this IDataRecord record, int i) {
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

        public static long? GetNullableInt64(this IDataRecord record, int i) {
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
        public static float? GetNullableFloat(this IDataRecord record, int i) {
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
        public static double? GetNullableDouble(this IDataRecord record, int i) {
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
        public static double? GetNullableDecimal(this IDataRecord record, int i) {
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
        public static string GetStringOrDefault(this IDataRecord record, int i, string defaultValue = "") {
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
        public static bool GetBooleanOrDefault(this IDataRecord record, int i, bool defaultValue = false) {
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
        public static byte GetByteOrDefault(this IDataRecord record, int i, byte defaultValue = default(byte)) {
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
        public static char GetCharOrDefault(this IDataRecord record, int i, char defaultValue = default(char)) {
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
        public static DateTime GetDateTimeOrDefault(this IDataRecord record, int i, DateTime defaultValue = default(DateTime)) {
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
        public static Guid GetGuidOrDefault(this IDataRecord record, int i, Guid defaultValue = default(Guid)) {
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
        public static short GetInt16OrDefault(this IDataRecord record, int i, short defaultValue = 0) {
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
        public static int GetInt32OrDefault(this IDataRecord record, int i, int defaultValue = 0) {
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
        public static long GetInt64OrDefault(this IDataRecord record, int i, long defaultValue = 0) {
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
        public static float GetFloatOrDefault(this IDataRecord record, int i, float defaultValue = 0f) {
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
        public static double GetDoubleOrDefault(this IDataRecord record, int i, double defaultValue = 0d) {
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
        public static decimal GetDecimalOrDefault(this IDataRecord record, int i, decimal defaultValue = 0m) {
            if (record.IsDBNull(i))
                return defaultValue;
            return record.GetDecimal(i);
        }

        #endregion
    }
}
