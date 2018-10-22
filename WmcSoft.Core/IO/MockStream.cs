using System;
using System.IO;

namespace WmcSoft.IO
{
    /// <summary>
    /// Stream that reads nothing and keeps track of the count of 
    /// bytes being written.
    /// </summary>
    public class MockStream : Stream
    {
        long length;

        public override int Read(byte[] buffer, int offset, int count)
        {
            return 0;
        }

        public override void Flush()
        {
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            length += count;
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => length;

        public override long Position {
            get { return length; }
            set { throw new NotImplementedException(); }
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }
    }
}
