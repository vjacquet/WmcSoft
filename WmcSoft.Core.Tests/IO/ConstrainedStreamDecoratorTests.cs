using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.IO.Tests
{
    [TestClass]
    public class ConstrainedStreamDecoratorTests
    {
        class MockStream : ConstrainedStreamDecorator
        {
            private readonly bool _canRead;
            private readonly bool _canWrite;
            private readonly bool _canSeek;

            public MockStream(bool canRead = true, bool canWrite = true, bool canSeek = true)
                : base(new MemoryStream()) {
                _canRead = canRead;
                _canWrite = canWrite;
                _canSeek = canSeek;
            }

            public override bool CanRead {
                get { return _canRead; }
            }

            public override bool CanWrite {
                get { return _canWrite; }
            }
            public override bool CanSeek {
                get { return _canSeek; }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void WriteThrowsWhenCanWriteReturnsFalse() {
            var stream = new MockStream(canWrite: false);
            var buffer = new byte[] { 0, 1, 2, 3, 4 };

            Assert.AreEqual(false, stream.CanWrite);
            stream.Write(buffer, 0, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ReadThrowsWhenCanReadReturnsFalse() {
            var stream = new MockStream(canRead: false);
            var buffer = new byte[] { 0, 1, 2, 3, 4 };

            Assert.AreEqual(false, stream.CanRead);
            stream.Read(buffer, 0, 5);
        }
    }
}
