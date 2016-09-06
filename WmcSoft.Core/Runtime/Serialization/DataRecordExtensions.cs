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
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace WmcSoft.Runtime.Serialization
{
    public static class DataRecordExtensions
    {
        #region GetObjectFromBinary

        public static T GetObjectFromXml<T>(this IDataRecord record, int i, BinarySerializer<T> serializer)
            where T : class {
            if (record.IsDBNull(i))
                return null;

            var length = record.GetBytes(i, 0, null, 0, Int32.MaxValue);
            var buffer = new byte[length];
            length = record.GetBytes(i, 0, buffer, 0, checked((int)length));

            using (var ms = new MemoryStream(buffer, false)) {
                return serializer.Deserialize(ms);
            }
        }

        #endregion

        #region GetObjectFromXml

        public static T GetObjectFromXml<T>(this IDataRecord record, int i)
            where T : class {
            return GetObjectFromXml<T>(record, i, new DataContractSerializer(typeof(T)));
        }

        public static T GetObjectFromXml<T>(this IDataRecord record, int i, IDataContractSurrogate surrogate)
            where T : class {
            return GetObjectFromXml<T>(record, i, new DataContractSerializer(typeof(T), Type.EmptyTypes, Int16.MaxValue, false, true, surrogate));
        }

        public static T GetObjectFromXml<T>(this IDataRecord record, int i, XmlObjectSerializer serializer)
            where T : class {
            if (record.IsDBNull(i))
                return null;
            var xml = Convert.ToString(record.GetValue(i));
            if (string.IsNullOrEmpty(xml))
                return null;
            using (var reader = XmlReader.Create(new StringReader(xml))) {
                return (T)serializer.ReadObject(reader);
            }
        }

        public static T GetObjectFromXml<T>(this IDataRecord record, int i, XmlSerializer<T> serializer)
            where T : class {
            if (record.IsDBNull(i))
                return null;
            var xml = Convert.ToString(record.GetValue(i));
            if (string.IsNullOrEmpty(xml))
                return null;
            using (var reader = XmlReader.Create(new StringReader(xml))) {
                return serializer.Deserialize(reader);
            }
        }

        #endregion

        #region GetStream

        // TODO: Test
        class ColumnStream : Stream
        {
            readonly IDataRecord _record;
            readonly int _i;
            long _read;

            public ColumnStream(IDataRecord record, int i) {
                _record = record;
                _i = i;
            }

            public override bool CanRead { get { return true; } }
            public override bool CanWrite { get { return false; } }
            public override bool CanSeek { get { return false; } }

            public override long Length {
                get { throw new NotSupportedException(); }
            }

            public override long Position {
                get { return _read; }
                set { throw new NotSupportedException(); }
            }

            public override int Read(byte[] buffer, int offset, int count) {
                int read = checked((int)_record.GetBytes(_i, _read, buffer, offset, count));
                _read += read;
                return read;
            }

            public override void Flush() {
                throw new NotSupportedException();
            }

            public override long Seek(long offset, SeekOrigin origin) {
                throw new NotSupportedException();
            }

            public override void SetLength(long value) {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count) {
                throw new NotSupportedException();
            }
        }

        public static Stream GetStream(this IDataRecord record, int i) {
            return new ColumnStream(record, i);
        }

        public static Stream GetStream(this IDataRecord record, int i, int bufferSize) {
            return new BufferedStream(new ColumnStream(record, i), bufferSize);
        }

        #endregion
    }
}
