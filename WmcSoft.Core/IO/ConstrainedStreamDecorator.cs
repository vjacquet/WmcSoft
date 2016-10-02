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
    /// Base decorator that checks CanRead, CanWrite & CanSeek before executing any operation.
    /// </summary>
    /// <remarks>CanRead, CanWrite and CanSeek returns false.</remarks>
    public abstract class ConstrainedStreamDecorator : StreamDecorator
    {
        #region Lifecycle

        protected ConstrainedStreamDecorator(Stream stream)
            : base(stream) {
        }

        #endregion

        #region Stream decoration

        public override long Length {
            get {
                if (!CanSeek)
                    throw new NotSupportedException();
                return base.Length;
            }
        }

        public override long Position {
            get {
                if (!CanSeek)
                    throw new NotSupportedException();
                return base.Position;
            }
            set {
                if (!CanSeek)
                    throw new NotSupportedException();
                base.Position = value;
            }
        }

        public override long Seek(long offset, SeekOrigin origin) {
            if (!CanSeek)
                throw new NotSupportedException();
            return base.Seek(offset, origin);
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            if (!CanRead)
                throw new NotSupportedException();
            return base.BeginRead(buffer, offset, count, callback, state);
        }

        public override int Read(byte[] buffer, int offset, int count) {
            if (!CanRead)
                throw new NotSupportedException();
            return base.Read(buffer, offset, count);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            if (!CanWrite)
                throw new NotSupportedException();
            return base.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Write(byte[] buffer, int offset, int count) {
            if (!CanWrite)
                throw new NotSupportedException();
            base.Write(buffer, offset, count);
        }

        public override void SetLength(long value) {
            if (!CanWrite || !CanSeek)
                throw new NotSupportedException();
            base.SetLength(value);
        }

        #endregion
    }
}