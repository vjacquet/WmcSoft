using System.IO;

namespace WmcSoft.IO
{
    class MockConstrainedStream : ConstrainedStreamDecorator
    {
        private readonly bool _canRead;
        private readonly bool _canWrite;
        private readonly bool _canSeek;

        public MockConstrainedStream(Stream stream, bool canRead = true, bool canWrite = true, bool canSeek = true)
            : base(stream) {
            _canRead = canRead;
            _canWrite = canWrite;
            _canSeek = canSeek;
        }

        public MockConstrainedStream(bool canRead = true, bool canWrite = true, bool canSeek = true)
            : this(new MemoryStream(), canRead, canWrite, canSeek) {
        }

        public override bool CanRead { get { return _canRead; } }
        public override bool CanWrite { get { return _canWrite; } }
        public override bool CanSeek { get { return _canSeek; } }
    }
}