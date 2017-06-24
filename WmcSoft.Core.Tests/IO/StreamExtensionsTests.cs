using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace WmcSoft.IO
{
    public class StreamExtensionsTests
    {
        class Progress : IProgress<long>
        {
            readonly object _syncRoot = new object();
            readonly List<long> _notifications = new List<long>();

            #region IProgress<long> Members

            public void Report(long value)
            {
                lock (_syncRoot) {
                    _notifications.Add(value);
                }
            }

            #endregion

            public List<long> Notifications { get { return _notifications; } }
        }

        class HideSeekableStream : ConstrainedStreamDecorator
        {
            public HideSeekableStream(Stream stream) : base(stream)
            {
            }

            public override bool CanSeek { get { return false; } }
        }

        [Fact]
        public void CheckCopyToWithProgress()
        {
            var buffer = new byte[1024];
            var random = new Random(1664);
            random.NextBytes(buffer);

            var progress = new Progress();

            using (var source = new MemoryStream(buffer))
            using (var destination = new MemoryStream()) {
                source.CopyToAsync(destination, 256, progress).Wait();

                Assert.Equal(buffer, destination.GetBuffer());
            }

            var expected = new[] { 256L, 512L, 768L, 1024L };
            var actual = progress.Notifications;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckSkipOnSeekableStream()
        {
            var expected = 63;
            var buffer = new byte[0x8000];
            buffer[0x2048] = (byte)expected;
            using (var stream = new MemoryStream(buffer)) {
                stream.Skip(0x2048);
                var actual = stream.ReadByte();

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void CheckSkipOnNonSeekableStream()
        {
            var expected = 63;
            var buffer = new byte[0x8000];
            buffer[0x2048] = (byte)expected;
            using (var stream = new HideSeekableStream(new MemoryStream(buffer))) {
                stream.Skip(0x2048);
                var actual = stream.ReadByte();

                Assert.Equal(expected, actual);
            }
        }
    }
}
