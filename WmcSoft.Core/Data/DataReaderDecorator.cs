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
        private readonly IDataReader _underlying;

        protected DataReaderDecorator(IDataReader dataReader) {
            _underlying = dataReader;
        }

        public virtual void Close() {
            _underlying.Close();
        }

        public virtual int Depth {
            get { return _underlying.Depth; }
        }

        public virtual DataTable GetSchemaTable() {
            return _underlying.GetSchemaTable();
        }

        public virtual bool IsClosed {
            get { return _underlying.IsClosed; }
        }

        public virtual bool NextResult() {
            return _underlying.NextResult();
        }

        public virtual bool Read() {
            return _underlying.Read();
        }

        public virtual int RecordsAffected {
            get { return _underlying.RecordsAffected; }
        }

        public virtual void Dispose() {
            _underlying.Dispose();
        }

        public virtual int FieldCount {
            get { return _underlying.FieldCount; }
        }

        public virtual bool GetBoolean(int i) {
            return _underlying.GetBoolean(i);
        }

        public virtual byte GetByte(int i) {
            return _underlying.GetByte(i);
        }

        public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) {
            return _underlying.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public virtual char GetChar(int i) {
            return _underlying.GetChar(i);
        }

        public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) {
            return _underlying.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public virtual IDataReader GetData(int i) {
            return _underlying.GetData(i);
        }

        public virtual string GetDataTypeName(int i) {
            return _underlying.GetDataTypeName(i);
        }

        public virtual DateTime GetDateTime(int i) {
            return _underlying.GetDateTime(i);
        }

        public virtual decimal GetDecimal(int i) {
            return _underlying.GetDecimal(i);
        }

        public virtual double GetDouble(int i) {
            return _underlying.GetDouble(i);
        }

        public virtual Type GetFieldType(int i) {
            return _underlying.GetFieldType(i);
        }

        public virtual float GetFloat(int i) {
            return _underlying.GetFloat(i);
        }

        public virtual Guid GetGuid(int i) {
            return _underlying.GetGuid(i);
        }

        public virtual short GetInt16(int i) {
            return _underlying.GetInt16(i);
        }

        public virtual int GetInt32(int i) {
            return _underlying.GetInt32(i);
        }

        public virtual long GetInt64(int i) {
            return _underlying.GetInt64(i);
        }

        public virtual string GetName(int i) {
            return _underlying.GetName(i);
        }

        public virtual int GetOrdinal(string name) {
            return _underlying.GetOrdinal(name);
        }

        public virtual string GetString(int i) {
            return _underlying.GetString(i);
        }

        public virtual object GetValue(int i) {
            return _underlying.GetValue(i);
        }

        public virtual int GetValues(object[] values) {
            return _underlying.GetValues(values);
        }

        public virtual bool IsDBNull(int i) {
            return _underlying.IsDBNull(i);
        }

        public virtual object this[string name] {
            get { return _underlying[name]; }
        }

        public virtual object this[int i] {
            get { return _underlying[i]; }
        }
    }
}
