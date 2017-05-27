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
    /// 
    /// </summary>
    public class MirroringStream : Stream
    {
        #region Fields

        private Stream master;
        private Stream mirror;

        #endregion

        #region Lifecycle

        public MirroringStream(Stream master, Stream mirror)
        {
            this.master = master;
            this.mirror = mirror;
        }

        public override void Close()
        {
            this.master.Close();
            this.mirror.Close();
            base.Close();
        }

        #endregion

        #region Info class Methods & Properties

        public override bool CanRead {
            get {
                return master.CanRead;
            }
        }

        public override bool CanSeek {
            get {
                return master.CanSeek;
            }
        }

        public override bool CanWrite {
            get {
                return master.CanWrite;
            }
        }

        public override long Length {
            get {
                return master.Length;
            }
        }

        public override long Position {
            get {
                return master.Position;
            }
            set {
                master.Position = value;
            }
        }

        public override bool CanTimeout {
            get {
                return master.CanTimeout;
            }
        }

        #endregion

        #region Read Methods & Properties

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (!mirror.CanSeek && !mirror.CanRead)
                throw new NotSupportedException();
            return master.BeginRead(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            int read = master.EndRead(asyncResult);

            if (mirror.CanSeek)
                mirror.Seek(read, SeekOrigin.Current);
            else {
                byte[] temporaryBuffer = new byte[read];
                mirror.Read(temporaryBuffer, 0, read);
            }

            return read;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = master.Read(buffer, offset, count);

            if (mirror.CanSeek)
                mirror.Seek(read, SeekOrigin.Current);
            else {
                byte[] temporaryBuffer = new byte[read];
                mirror.Read(temporaryBuffer, 0, read);
            }

            return read;
        }


        public override long Seek(long offset, SeekOrigin origin)
        {
            long retVal = master.Seek(offset, origin);

            mirror.Seek(offset, origin);

            return retVal;
        }

        public override int ReadTimeout {
            get {
                return master.ReadTimeout;
            }
            set {
                master.ReadTimeout = value;
            }
        }
        #endregion

        #region Write Methods & Properties

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            IAsyncResult retVal = master.BeginWrite(buffer, offset, count, callback, state);

            mirror.Write(buffer, offset, count);
            mirror.Flush();

            return retVal;
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            master.EndWrite(asyncResult);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            master.Write(buffer, offset, count);

            mirror.Write(buffer, offset, count);
            mirror.Flush();
        }

        public override void Flush()
        {
            master.Flush();
        }

        public override void SetLength(long value)
        {
            master.SetLength(value);
            mirror.SetLength(value);
        }

        public override int WriteTimeout {
            get {
                return master.WriteTimeout;
            }
            set {
                master.WriteTimeout = value;
            }
        }
        #endregion
    }
}
