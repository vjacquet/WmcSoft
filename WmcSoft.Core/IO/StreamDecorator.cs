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

        private readonly Stream underlying;

        #endregion

        #region Lifecycle

        private StreamDecorator()
        {
        }

        protected StreamDecorator(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            underlying = stream;
        }

        public override void Close()
        {
            underlying.Close();
        }

        #endregion

        #region Info class Methods & Properties

        public override bool CanRead => underlying.CanRead;

        public override bool CanSeek => underlying.CanSeek;

        public override bool CanWrite => underlying.CanWrite;

        public override long Length => underlying.Length;

        public override long Position {
            get { return underlying.Position; }
            set { underlying.Position = value; }
        }

        public override bool CanTimeout => underlying.CanTimeout;

        #endregion

        #region Read Methods & Properties

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return underlying.BeginRead(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return underlying.EndRead(asyncResult);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return underlying.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return underlying.Seek(offset, origin);
        }

        public override int ReadTimeout {
            get { return underlying.ReadTimeout; }
            set { underlying.ReadTimeout = value; }
        }

        #endregion

        #region Write Methods & Properties

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return underlying.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            underlying.EndWrite(asyncResult);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            underlying.Write(buffer, offset, count);
        }

        public override void Flush()
        {
            underlying.Flush();
        }

        public override void SetLength(long value)
        {
            underlying.SetLength(value);
        }

        public override int WriteTimeout {
            get { return underlying.WriteTimeout; }
            set { underlying.WriteTimeout = value; }
        }

        #endregion
    }
}
