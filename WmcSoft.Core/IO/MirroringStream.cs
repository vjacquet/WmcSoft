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
    /// Dumps the data written into another stream.
    /// </summary>
    public class MirroringStream : Stream
    {
        #region Fields

        private Stream _master;
        private Stream _mirror;

        #endregion

        #region Lifecycle

        public MirroringStream(Stream master, Stream mirror)
        {
            if (master == null) throw new ArgumentNullException(nameof(master));
            if (mirror == null) throw new ArgumentNullException(nameof(mirror));

            _master = master;
            _mirror = mirror;
        }

        public override void Close()
        {
            _master.Close();
            _mirror.Close();
            base.Close();
        }

        #endregion

        #region Info class Methods & Properties

        public override bool CanRead {
            get { return _master.CanRead; }
        }

        public override bool CanSeek {
            get { return _master.CanSeek; }
        }

        public override bool CanWrite {
            get { return _master.CanWrite; }
        }

        public override long Length {
            get { return _master.Length; }
        }

        public override long Position {
            get { return _master.Position; }
            set { _master.Position = value; }
        }

        public override bool CanTimeout {
            get { return _master.CanTimeout; }
        }

        #endregion

        #region Read Methods & Properties

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (!_mirror.CanSeek && !_mirror.CanRead)
                throw new NotSupportedException();
            return _master.BeginRead(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            int read = _master.EndRead(asyncResult);

            if (_mirror.CanSeek)
                _mirror.Seek(read, SeekOrigin.Current);
            else {
                byte[] temporaryBuffer = new byte[read];
                _mirror.Read(temporaryBuffer, 0, read);
            }

            return read;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = _master.Read(buffer, offset, count);

            if (_mirror.CanSeek)
                _mirror.Seek(read, SeekOrigin.Current);
            else {
                byte[] temporaryBuffer = new byte[read];
                _mirror.Read(temporaryBuffer, 0, read);
            }

            return read;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long retVal = _master.Seek(offset, origin);

            _mirror.Seek(offset, origin);

            return retVal;
        }

        public override int ReadTimeout {
            get {                return _master.ReadTimeout;            }
            set {                _master.ReadTimeout = value;            }
        }
        #endregion

        #region Write Methods & Properties

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            IAsyncResult retVal = _master.BeginWrite(buffer, offset, count, callback, state);

            _mirror.Write(buffer, offset, count);
            _mirror.Flush();

            return retVal;
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            _master.EndWrite(asyncResult);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _master.Write(buffer, offset, count);

            _mirror.Write(buffer, offset, count);
            _mirror.Flush();
        }

        public override void Flush()
        {
            _master.Flush();
        }

        public override void SetLength(long value)
        {
            _master.SetLength(value);
            _mirror.SetLength(value);
        }

        public override int WriteTimeout {
            get {
                return _master.WriteTimeout;
            }
            set {
                _master.WriteTimeout = value;
            }
        }
        #endregion
    }
}
