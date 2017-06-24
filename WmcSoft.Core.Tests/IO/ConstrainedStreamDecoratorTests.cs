using System;
using System.IO;
using Xunit;

namespace WmcSoft.IO
{
    public class ConstrainedStreamDecoratorTests
    {
        [Fact]
        public void WriteThrowsWhenCanWriteReturnsFalse()
        {
            var stream = new MockConstrainedStream(canWrite: false);
            var buffer = new byte[] { 0, 1, 2, 3, 4 };

            Assert.False(stream.CanWrite);
            Assert.Throws<NotSupportedException>(() => stream.Write(buffer, 0, 5));
        }

        [Fact]
        public void ReadThrowsWhenCanReadReturnsFalse()
        {
            var buffer = new byte[] { 0, 1, 2, 3, 4 };
            var stream = new MockConstrainedStream(new MemoryStream(buffer), canRead: false);

            Assert.False(stream.CanRead);
            Assert.Throws<NotSupportedException>(() => stream.Read(buffer, 0, 5));
        }

        [Fact]
        public void SeekThrowsWhenCanSeekReturnsFalse()
        {
            var buffer = new byte[] { 0, 1, 2, 3, 4 };
            var stream = new MockConstrainedStream(new MemoryStream(buffer), canRead: true, canSeek: false);

            Assert.False(stream.CanSeek);
            Assert.Throws<NotSupportedException>(() => stream.Seek(0, SeekOrigin.End));
        }

        [Fact]
        public void PositionThrowsWhenCanSeekReturnsFalse()
        {
            var buffer = new byte[] { 0, 1, 2, 3, 4 };
            var stream = new MockConstrainedStream(new MemoryStream(buffer), canRead: true, canSeek: false);

            Assert.False(stream.CanSeek);
            Assert.Throws<NotSupportedException>(() => stream.Position);
        }
    }
}