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
        long _length;

        public override int Read(byte[] buffer, int offset, int count) {
            return 0;
        }

        public override void Flush() {
        }

        public override void Write(byte[] buffer, int offset, int count) {
            _length += count;
        }

        public override bool CanRead {
            get { return true; }
        }

        public override bool CanSeek {
            get { return false; }
        }

        public override bool CanWrite {
            get { return true; }
        }

        public override long Length {
            get { return _length; }
        }

        public override long Position {
            get { return _length; }
            set { throw new NotImplementedException(); }
        }

        public override void SetLength(long value) {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin) {
            throw new NotImplementedException();
        }
    }
}
