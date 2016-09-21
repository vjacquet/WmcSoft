using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.IO
{
    [TestClass]
    public class ConstrainedStreamDecoratorTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void WriteThrowsWhenCanWriteReturnsFalse() {
            var stream = new MockConstrainedStream(canWrite: false);
            var buffer = new byte[] { 0, 1, 2, 3, 4 };

            Assert.AreEqual(false, stream.CanWrite);
            stream.Write(buffer, 0, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ReadThrowsWhenCanReadReturnsFalse() {
            var buffer = new byte[] { 0, 1, 2, 3, 4 };
            var stream = new MockConstrainedStream(new MemoryStream(buffer), canRead: false);

            Assert.AreEqual(false, stream.CanRead);
            stream.Read(buffer, 0, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void SeekThrowsWhenCanSeekReturnsFalse() {
            var buffer = new byte[] { 0, 1, 2, 3, 4 };
            var stream = new MockConstrainedStream(new MemoryStream(buffer), canRead: true, canSeek: false);

            Assert.AreEqual(false, stream.CanSeek);
            stream.Seek(0, SeekOrigin.End);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PositionThrowsWhenCanSeekReturnsFalse() {
            var buffer = new byte[] { 0, 1, 2, 3, 4 };
            var stream = new MockConstrainedStream(new MemoryStream(buffer), canRead: true, canSeek: false);

            Assert.AreEqual(false, stream.CanSeek);
            var position = stream.Position;
        }
    }
}