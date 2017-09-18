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
using System.IO;

namespace WmcSoft.IO
{
    /// <summary>
    /// Base class when creating streams decorating another one.
    /// </summary>
    [Serializable]
    public abstract class StreamDecorator : Stream
    {
        #region Fields

        private readonly Stream _underlying;

        #endregion

        #region Lifecycle

        private StreamDecorator()
        {
        }

        protected StreamDecorator(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            _underlying = stream;
        }

        public override void Close()
        {
            _underlying.Close();
        }

        #endregion

        #region Info class Methods & Properties

        public override bool CanRead {
            get { return _underlying.CanRead; }
        }

        public override bool CanSeek {
            get { return _underlying.CanSeek; }
        }

        public override bool CanWrite {
            get { return _underlying.CanWrite; }
        }

        public override long Length {
            get { return _underlying.Length; }
        }

        public override long Position {
            get { return _underlying.Position; }
            set { _underlying.Position = value; }
        }

        public override bool CanTimeout {
            get { return _underlying.CanTimeout; }
        }

        #endregion

        #region Read Methods & Properties

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _underlying.BeginRead(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return _underlying.EndRead(asyncResult);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _underlying.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _underlying.Seek(offset, origin);
        }

        public override int ReadTimeout {
            get { return _underlying.ReadTimeout; }
            set { _underlying.ReadTimeout = value; }
        }
        #endregion

        #region Write Methods & Properties

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _underlying.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            _underlying.EndWrite(asyncResult);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _underlying.Write(buffer, offset, count);
        }

        public override void Flush()
        {
            _underlying.Flush();
        }

        public override void SetLength(long value)
        {
            _underlying.SetLength(value);
        }

        public override int WriteTimeout {
            get { return _underlying.WriteTimeout; }
            set { _underlying.WriteTimeout = value; }
        }
        #endregion
    }
}
