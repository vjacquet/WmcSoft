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
    /// Base class for implementing DataReader.
    /// </summary>
    public abstract class DataReaderBase : IDataReader
    {
        #region IDataReader Membres

        public void Close() {
            Dispose();
        }

        public int Depth {
            get { return 0; }
        }

        public virtual DataTable GetSchemaTable() {
            var dt = new DataTable();
            var length = FieldCount;
            for (int i = 0; i < length; i++) {
                dt.Columns.Add(GetName(i), GetFieldType(i));
            }
            return dt;
        }

        public bool IsClosed {
            get { return _disposed; }
        }

        public bool NextResult() {
            return false;
        }

        public abstract bool Read();

        public int RecordsAffected {
            get { return 0; }
        }

        #endregion

        #region IDisposable Membres

        bool _disposed;

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

        public abstract int FieldCount {
            get;
        }

        public bool GetBoolean(int i) {
            return (bool)GetValue(i);
        }

        public byte GetByte(int i) {
            return (byte)GetValue(i);
        }

        public long GetBytes(int i, long fieldoffset, byte[] buffer, int bufferoffset, int length) {
            var source = (byte[])GetValue(i);
            if (buffer == null)
                return source.LongLength;
            var count = Math.Min(source.LongLength - fieldoffset, length);
            Array.Copy(source, fieldoffset, buffer, bufferoffset, count);
            return count;
        }

        public char GetChar(int i) {
            return (char)GetValue(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) {
            var source = (char[])GetValue(i);
            if (buffer == null)
                return source.LongLength;
            var count = Math.Min(source.LongLength - fieldoffset, length);
            Array.Copy(source, fieldoffset, buffer, bufferoffset, count);
            return count;
        }

        public virtual IDataReader GetData(int i) {
            return (IDataReader)GetValue(i);
        }

        public virtual string GetDataTypeName(int i) {
            return GetFieldType(i).Name;
        }

        public DateTime GetDateTime(int i) {
            return (DateTime)GetValue(i);
        }

        public decimal GetDecimal(int i) {
            return (decimal)GetValue(i);
        }

        public double GetDouble(int i) {
            return (double)GetValue(i);
        }

        public abstract Type GetFieldType(int i);

        public float GetFloat(int i) {
            return (float)GetValue(i);
        }

        public Guid GetGuid(int i) {
            return (Guid)GetValue(i);
        }

        public short GetInt16(int i) {
            return (short)GetValue(i);
        }

        public int GetInt32(int i) {
            return (int)GetValue(i);
        }

        public long GetInt64(int i) {
            return (long)GetValue(i);
        }

        public abstract string GetName(int i);

        public abstract int GetOrdinal(string name);

        public string GetString(int i) {
            return (string)GetValue(i);
        }

        public abstract object GetValue(int i);

        public virtual int GetValues(object[] values) {
            int count = Math.Min(values.Length, FieldCount);
            for (int i = 0; i != count; ++i) {
                values[i] = GetValue(i);
            }
            return count;
        }

        public virtual bool IsDBNull(int i) {
            var value = GetValue(i);
            return value == null || value == System.DBNull.Value;
        }

        public object this[string name] {
            get { return GetValue(name); }
        }

        public object this[int i] {
            get { return GetValue(i); }
        }

        protected virtual object GetValue(string name) {
            return GetValue(GetOrdinal(name));
        }

        #endregion
    }
}
