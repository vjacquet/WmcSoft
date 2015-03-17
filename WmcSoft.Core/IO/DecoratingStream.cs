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
    public class DecoratingStream : Stream
    {
        #region Lifecycle

        private DecoratingStream() {
        }

        protected DecoratingStream(Stream stream) {
            if (stream == null) {
                throw new ArgumentNullException("decoratedStream");
            }
            this.decoratedStream = stream;
        }

        public override void Close() {
            decoratedStream.Close();
        }

        #endregion

        #region Class Properties

        protected Stream DecoratedStream {
            get { return this.decoratedStream; }
        }
        private Stream decoratedStream;

        #endregion

        #region Info class Methods & Properties

        public override bool CanRead {
            get { return decoratedStream.CanRead; }
        }

        public override bool CanSeek {
            get { return decoratedStream.CanSeek; }
        }

        public override bool CanWrite {
            get { return decoratedStream.CanWrite; }
        }

        public override long Length {
            get { return decoratedStream.Length; }
        }

        public override long Position {
            get { return decoratedStream.Position; }
            set { decoratedStream.Position = value; }
        }

        public override bool CanTimeout {
            get { return decoratedStream.CanTimeout; }
        }

        #endregion

        #region Read Methods & Properties

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            return decoratedStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult) {
            return decoratedStream.EndRead(asyncResult);
        }

        public override int Read(byte[] buffer, int offset, int count) {
            return decoratedStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin) {
            return decoratedStream.Seek(offset, origin);
        }

        public override int ReadTimeout {
            get { return decoratedStream.ReadTimeout; }
            set { decoratedStream.ReadTimeout = value; }
        }
        #endregion

        #region Write Methods & Properties

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            return decoratedStream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult) {
            decoratedStream.EndWrite(asyncResult);
        }

        public override void Write(byte[] buffer, int offset, int count) {
            decoratedStream.Write(buffer, offset, count);
        }

        public override void Flush() {
            decoratedStream.Flush();
        }

        public override void SetLength(long value) {
            decoratedStream.SetLength(value);
        }

        public override int WriteTimeout {
            get { return decoratedStream.WriteTimeout; }
            set { decoratedStream.WriteTimeout = value; }
        }
        #endregion
    }
}
