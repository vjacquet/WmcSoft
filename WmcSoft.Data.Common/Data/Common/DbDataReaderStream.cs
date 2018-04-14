#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Data.Common
{
    /// <summary>
    /// Exposes the bytes of a <see cref="IDataReader"/> field as a <see cref="Stream"/>.
    /// </summary>
    public class DbDataReaderStream : Stream
    {
        readonly IDataReader _reader;
        readonly int _ordinal;
        long _position;

        /// <summary>
        /// Creates a new instance of the <see cref="DbDataReaderStream"/>.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="ordinal">The zero based column ordinal.</param>
        public DbDataReaderStream(IDataReader reader, int ordinal)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            _reader = reader;
            _ordinal = ordinal;
            _position = 0L;
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => _reader.GetBytes(_ordinal, 0, null, 0, 0);

        public override long Position {
            get { return _position; }
            set { Seek(value, SeekOrigin.Begin); }
        }

        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            var length = Length;
            switch (origin) {
            case SeekOrigin.Current:
                _position += offset;
                break;
            case SeekOrigin.End:
                _position = length - offset;
                break;
            case SeekOrigin.Begin:
            default:
                _position = offset;
                break;
            }
            if (_position < 0L)
                _position = 0L;
            else if (_position >= length)
                _position = length - 1;
            return _position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var read = _reader.GetBytes(_ordinal, _position, buffer, offset, count);
            _position += read;
            return checked((int)read);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}
