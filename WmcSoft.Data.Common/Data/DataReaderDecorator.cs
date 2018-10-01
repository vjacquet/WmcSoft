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

namespace WmcSoft.Data
{
    /// <summary>
    /// Base class to implement a DataReader decorator.
    /// </summary>
    public abstract class DataReaderDecorator : IDataReader
    {
        private readonly IDataReader underlying;

        protected DataReaderDecorator(IDataReader dataReader)
        {
            underlying = dataReader;
        }

        /// <summary>
        /// Closes the <see cref="IDataReader"/> Object.
        /// </summary>
        public virtual void Close()
        {
            underlying.Close();
        }

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        public virtual int Depth {
            get { return underlying.Depth; }
        }

        /// <summary>
        /// Returns a <see cref="DataTable"/> that describes the column metadata of the <see cref="IDataReader"/>.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> that describes the column metadata</returns>
        /// <exception cref="InvalidOperationException">The <see cref="IDataReader"/> is closed.</exception>
        public virtual DataTable GetSchemaTable()
        {
            return underlying.GetSchemaTable();
        }

        /// <summary>
        /// Gets a value indicating whether the data reader is closed.
        /// </summary>
        public virtual bool IsClosed {
            get { return underlying.IsClosed; }
        }

        /// <summary>
        /// Advances the data reader to the next result, when reading the results of SQL statements.
        /// </summary>
        /// <returns>true if there are more rows; otherwise, false.</returns>
        public virtual bool NextResult()
        {
            return underlying.NextResult();
        }

        /// <summary>
        /// Advances the <see cref="IDataReader"/> to the next record.
        /// </summary>
        /// <returns>true if there are more rows; otherwise, false.</returns>
        public virtual bool Read()
        {
            return underlying.Read();
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        public virtual int RecordsAffected {
            get { return underlying.RecordsAffected; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) {
                underlying.Dispose();
            }
        }

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <value>When not positioned in a valid recordset, 0; otherwise, the number of columns in the current record. The default is -1.</value>
        public virtual int FieldCount {
            get { return underlying.FieldCount; }
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual bool GetBoolean(int i)
        {
            return underlying.GetBoolean(i);
        }

        /// <summary>
        /// Gets the 8-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual byte GetByte(int i)
        {
            return underlying.GetByte(i);
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
        public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return underlying.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        /// <summary>
        /// Gets the character value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual char GetChar(int i)
        {
            return underlying.GetChar(i);
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
        public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return underlying.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        /// <summary>
        /// Returns an <see cref="System.Data.IDataReader"/> for the specified column ordinal.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The <see cref="System.Data.IDataReader"/> for the specified column ordinal.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual IDataReader GetData(int i)
        {
            return underlying.GetData(i);
        }

        /// <summary>
        /// Gets the data type information for the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The data type information for the specified field.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual string GetDataTypeName(int i)
        {
            return underlying.GetDataTypeName(i);
        }

        /// <summary>
        /// Gets the date and time value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual DateTime GetDateTime(int i)
        {
            return underlying.GetDateTime(i);
        }

        /// <summary>
        /// Gets the fixed-position numeric value of the specified field.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual decimal GetDecimal(int i)
        {
            return underlying.GetDecimal(i);
        }

        /// <summary>
        /// Gets the double-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual double GetDouble(int i)
        {
            return underlying.GetDouble(i);
        }

        /// <summary>
        /// Gets the <see cref="System.Type"/> information corresponding to the type of <see cref="System.Object"/> that
        /// would be returned from <see cref="System.Data.IDataRecord.GetValue(System.Int32)"/>.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The<see cref="System.Type"/> information corresponding to the type of <see cref="System.Object"/> that would
        /// be returned from <see cref="System.Data.IDataRecord.GetValue(System.Int32)"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual Type GetFieldType(int i)
        {
            return underlying.GetFieldType(i);
        }

        /// <summary>
        /// Gets the single-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual float GetFloat(int i)
        {
            return underlying.GetFloat(i);
        }

        /// <summary>
        /// Gets the GUID value of the specified field.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual Guid GetGuid(int i)
        {
            return underlying.GetGuid(i);
        }

        /// <summary>
        /// Gets the 16-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual short GetInt16(int i)
        {
            return underlying.GetInt16(i);
        }

        /// <summary>
        /// Gets the 32-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual int GetInt32(int i)
        {
            return underlying.GetInt32(i);
        }

        /// <summary>
        /// Gets the 64-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual long GetInt64(int i)
        {
            return underlying.GetInt64(i);
        }

        /// <summary>
        /// Gets the name for the field to find.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The name of the field or the empty string (""), if there is no value to return.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual string GetName(int i)
        {
            return underlying.GetName(i);
        }

        /// <summary>
        /// Return the index of the named field.
        /// </summary>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The index of the named field.</returns>
        public virtual int GetOrdinal(string name)
        {
            return underlying.GetOrdinal(name);
        }

        /// <summary>
        /// Gets the string value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual string GetString(int i)
        {
            return underlying.GetString(i);
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual object GetValue(int i)
        {
            return underlying.GetValue(i);
        }

        /// <summary>
        /// Populates an array of objects with the column values of the current record.
        /// </summary>
        /// <param name="values">An array of <see cref="System.Object"/> to copy the attribute fields into.</param>
        /// <returns>The number of instances of <see cref="System.Object"/> in the array.</returns>
        public virtual int GetValues(object[] values)
        {
            return underlying.GetValues(values);
        }

        /// <summary>
        /// Return whether the specified field is set to null.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>true if the specified field is set to null; otherwise, false.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual bool IsDBNull(int i)
        {
            return underlying.IsDBNull(i);
        }

        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The column with the specified name as an <see cref="System.Object"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">No column with the specified name was found.</exception>
        public virtual object this[string name] {
            get { return underlying[name]; }
        }

        /// <summary>
        /// Gets the column located at the specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the column to get.</param>
        /// <returns>The column with the specified name as an <see cref="System.Object"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public virtual object this[int i] {
            get { return underlying[i]; }
        }
    }
}
