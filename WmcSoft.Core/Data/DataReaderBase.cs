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
using System.Data.Common;

namespace WmcSoft.Data
{
    /// <summary>
    /// Base class for implementing DataReader.
    /// </summary>
    public abstract class DataReaderBase : IDataReader
    {
        #region IDataReader Membres

        /// <summary>
        /// Closes the <see cref="IDataReader"/> Object.
        /// </summary>
        public void Close() {
            Dispose();
        }

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        public int Depth
        {
            get { return 0; }
        }

        /// <summary>
        /// Returns a <see cref="DataTable"/> that describes the column metadata of the <see cref="IDataReader"/>.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> that describes the column metadata</returns>
        /// <exception cref="InvalidOperationException">The <see cref="IDataReader"/> is closed.</exception>
        public virtual DataTable GetSchemaTable() {
            var dt = new DataTable("SchemaTable");
            dt.Locale = System.Globalization.CultureInfo.InvariantCulture;

            dt.AddColumn<string>(SchemaTableColumn.ColumnName);
            dt.AddColumn<int>(SchemaTableColumn.ColumnOrdinal);
            dt.AddColumn<Type>(SchemaTableColumn.DataType);
            dt.AddColumn<int>(SchemaTableColumn.ColumnSize, defaultValue: -1);
            dt.AddColumn<short>(SchemaTableColumn.NumericPrecision);
            dt.AddColumn<short>(SchemaTableColumn.NumericScale);
            dt.AddColumn<int>(SchemaTableColumn.ProviderType);
            dt.AddColumn<bool>(SchemaTableColumn.IsLong);
            dt.AddColumn<bool>(SchemaTableColumn.AllowDBNull);
            dt.AddColumn<bool>(SchemaTableOptionalColumn.IsReadOnly, defaultValue: false);
            dt.AddColumn<bool>(SchemaTableOptionalColumn.IsRowVersion);
            dt.AddColumn<bool>(SchemaTableColumn.IsUnique);
            dt.AddColumn<bool>(SchemaTableColumn.IsKey);

            var length = FieldCount;
            for (int i = 0; i < length; i++) {
                var dr = GetSchemaRow(dt, i);
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Returns a <see cref="DataRow"/> that describes one column of the <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="i">The index of the column.</param>
        /// <returns>A <see cref="DataRow"/> that describes one column.</returns>
        protected virtual DataRow GetSchemaRow(DataTable dataTable, int i) {
            var dr = dataTable.NewRow();
            dr[0] = GetName(i);
            dr[1] = i;
            dr[2] = GetFieldType(i);
            return dr;
        }

        /// <summary>
        /// Gets a value indicating whether the data reader is closed.
        /// </summary>
        public bool IsClosed
        {
            get { return _disposed; }
        }

        /// <summary>
        /// Advances the data reader to the next result, when reading the results of SQL statements.
        /// </summary>
        /// <returns>true if there are more rows; otherwise, false.</returns>
        public bool NextResult() {
            return false;
        }

        /// <summary>
        /// Advances the <see cref="IDataReader"/> to the next record.
        /// </summary>
        /// <returns>true if there are more rows; otherwise, false.</returns>
        public abstract bool Read();

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        public int RecordsAffected
        {
            get { return -1; }
        }

        #endregion

        #region IDisposable Membres

        bool _disposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            if (_disposed)
                return;

            Dispose(true);
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
        }

        ~DataReaderBase() {
            Dispose(false);
        }

        #endregion

        #region IDataRecord Membres

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <value>When not positioned in a valid recordset, 0; otherwise, the number of columns in the current record. The default is -1.</value>
        public abstract int FieldCount
        {
            get;
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public bool GetBoolean(int i) {
            return (bool)GetValue(i);
        }

        /// <summary>
        /// Gets the 8-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public byte GetByte(int i) {
            return (byte)GetValue(i);
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldOffset">The index within the field from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of bytes read.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public long GetBytes(int i, long fieldoffset, byte[] buffer, int bufferoffset, int length) {
            var source = (byte[])GetValue(i);
            if (buffer == null)
                return source.LongLength;
            var count = Math.Min(source.LongLength - fieldoffset, length);
            Array.Copy(source, fieldoffset, buffer, bufferoffset, count);
            return count;
        }

        /// <summary>
        /// Gets the character value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public char GetChar(int i) {
            return (char)GetValue(i);
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer
        /// as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldOffset">The index within the field from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of characters read.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) {
            var source = (char[])GetValue(i);
            if (buffer == null)
                return source.LongLength;
            var count = Math.Min(source.LongLength - fieldoffset, length);
            Array.Copy(source, fieldoffset, buffer, bufferoffset, count);
            return count;
        }

        /// <summary>
        /// Returns an <see cref="System.Data.IDataReader"/> for the specified column ordinal.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The <see cref="System.Data.IDataReader"/> for the specified column ordinal.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual IDataReader GetData(int i) {
            return (IDataReader)GetValue(i);
        }

        /// <summary>
        /// Gets the data type information for the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The data type information for the specified field.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual string GetDataTypeName(int i) {
            return GetFieldType(i).Name;
        }

        /// <summary>
        /// Gets the date and time value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public DateTime GetDateTime(int i) {
            return (DateTime)GetValue(i);
        }

        /// <summary>
        /// Gets the fixed-position numeric value of the specified field.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public decimal GetDecimal(int i) {
            return (decimal)GetValue(i);
        }

        /// <summary>
        /// Gets the double-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public double GetDouble(int i) {
            return (double)GetValue(i);
        }

        /// <summary>
        /// Gets the <see cref="System.Type"/> information corresponding to the type of <see cref="System.Object"/> that
        /// would be returned from <see cref="System.Data.IDataRecord.GetValue(System.Int32)"/>.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The<see cref="System.Type"/> information corresponding to the type of <see cref="System.Object"/> that would
        /// be returned from <see cref="System.Data.IDataRecord.GetValue(System.Int32)"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public abstract Type GetFieldType(int i);

        /// <summary>
        /// Gets the single-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public float GetFloat(int i) {
            return (float)GetValue(i);
        }

        /// <summary>
        /// Gets the GUID value of the specified field.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public Guid GetGuid(int i) {
            return (Guid)GetValue(i);
        }

        /// <summary>
        /// Gets the 16-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public short GetInt16(int i) {
            return (short)GetValue(i);
        }

        /// <summary>
        /// Gets the 32-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public int GetInt32(int i) {
            return (int)GetValue(i);
        }

        /// <summary>
        /// Gets the 64-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public long GetInt64(int i) {
            return (long)GetValue(i);
        }

        /// <summary>
        /// Gets the name for the field to find.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The name of the field or the empty string (""), if there is no value to return.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public abstract string GetName(int i);

        /// <summary>
        /// Return the index of the named field.
        /// </summary>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The index of the named field.</returns>
        public abstract int GetOrdinal(string name);

        /// <summary>
        /// Gets the string value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public string GetString(int i) {
            return (string)GetValue(i);
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public abstract object GetValue(int i);

        /// <summary>
        /// Populates an array of objects with the column values of the current record.
        /// </summary>
        /// <param name="values">An array of <see cref="System.Object"/> to copy the attribute fields into.</param>
        /// <returns>The number of instances of <see cref="System.Object"/> in the array.</returns>
        public virtual int GetValues(object[] values) {
            int count = Math.Min(values.Length, FieldCount);
            for (int i = 0; i != count; ++i) {
                values[i] = GetValue(i);
            }
            return count;
        }

        /// <summary>
        /// Return whether the specified field is set to null.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>true if the specified field is set to null; otherwise, false.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual bool IsDBNull(int i) {
            var value = GetValue(i);
            return value == null || value == System.DBNull.Value;
        }

        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The column with the specified name as an <see cref="System.Object"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">No column with the specified name was found.</exception>
        public object this[string name]
        {
            get { return GetValue(name); }
        }

        /// <summary>
        /// Gets the column located at the specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the column to get.</param>
        /// <returns>The column with the specified name as an <see cref="System.Object"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public object this[int i]
        {
            get { return GetValue(i); }
        }

        protected virtual object GetValue(string name) {
            return GetValue(GetOrdinal(name));
        }

        #endregion
    }
}
