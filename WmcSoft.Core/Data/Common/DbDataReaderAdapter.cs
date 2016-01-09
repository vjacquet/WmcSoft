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
using System.Collections;
using System.Data;
using System.Data.Common;

namespace WmcSoft.Data.Common
{
    sealed class DbDataReaderAdapter : DbDataReader
    {
        readonly IDataReader _reader;

        public DbDataReaderAdapter(IDataReader reader) {
            _reader = reader;
        }

        public override object this[string name]
        {
            get { return _reader[name]; }
        }

        public override object this[int ordinal]
        {
            get { return _reader[ordinal]; }
        }

        public override int Depth
        {
            get { return _reader.Depth; }
        }

        public override int FieldCount
        {
            get { return _reader.FieldCount; }
        }

        public override bool HasRows
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool IsClosed
        {
            get { return _reader.IsClosed; }
        }

        public override int RecordsAffected
        {
            get { return _reader.RecordsAffected; }
        }

        public override void Close() {
            _reader.Close();
        }

        public override bool GetBoolean(int ordinal) {
            return _reader.GetBoolean(ordinal);
        }

        public override byte GetByte(int ordinal) {
            return _reader.GetByte(ordinal);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) {
            return _reader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public override char GetChar(int ordinal) {
            return _reader.GetChar(ordinal);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length) {
            return _reader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
        }

        public override string GetDataTypeName(int ordinal) {
            return _reader.GetDataTypeName(ordinal);
        }

        public override DateTime GetDateTime(int ordinal) {
            return _reader.GetDateTime(ordinal);
        }

        public override decimal GetDecimal(int ordinal) {
            return _reader.GetDecimal(ordinal);
        }

        public override double GetDouble(int ordinal) {
            return _reader.GetDouble(ordinal);
        }

        public override IEnumerator GetEnumerator() {
            return new DbEnumerator(_reader);
        }

        public override Type GetFieldType(int ordinal) {
            return _reader.GetFieldType(ordinal);
        }

        public override float GetFloat(int ordinal) {
            return _reader.GetFloat(ordinal);
        }

        public override Guid GetGuid(int ordinal) {
            return _reader.GetGuid(ordinal);
        }

        public override short GetInt16(int ordinal) {
            return _reader.GetInt16(ordinal);
        }

        public override int GetInt32(int ordinal) {
            return _reader.GetInt32(ordinal);
        }

        public override long GetInt64(int ordinal) {
            return _reader.GetInt64(ordinal);
        }

        public override string GetName(int ordinal) {
            return _reader.GetName(ordinal);
        }

        public override int GetOrdinal(string name) {
            return _reader.GetOrdinal(name);
        }

        public override DataTable GetSchemaTable() {
            return _reader.GetSchemaTable();
        }

        public override string GetString(int ordinal) {
            return _reader.GetString(ordinal);
        }

        public override object GetValue(int ordinal) {
            return _reader.GetValue(ordinal);
        }

        public override int GetValues(object[] values) {
            return _reader.GetValues(values);
        }

        public override bool IsDBNull(int ordinal) {
            return _reader.GetBoolean(ordinal);
        }

        public override bool NextResult() {
            return _reader.NextResult();
        }

        public override bool Read() {
            return _reader.Read();
        }
    }
}
