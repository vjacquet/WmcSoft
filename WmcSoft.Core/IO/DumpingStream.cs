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
    /// Dumps the data read into another stream.
    /// </summary>
    public class DumpingStream : Stream
    {
        private readonly Stream underlying;
        private readonly Stream dump;

        public DumpingStream(Stream stream, Stream dump)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (dump == null) throw new ArgumentNullException(nameof(dump));

            underlying = stream;
            this.dump = dump;
        }

        public override void Close()
        {
            dump.Close();
            underlying.Close();
            base.Close();
        }

        public override bool CanRead => underlying.CanRead;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override void Flush()
        {
            dump.Flush();
        }

        public override long Length => throw new NotSupportedException();

        public override long Position {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int readCount = underlying.Read(buffer, offset, count);
            dump.Write(buffer, offset, readCount);
            return readCount;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}
