using System;
using System.IO;
using System.Text;
using Xunit;

namespace WmcSoft.IO.Sources
{
    public class MostRecentStreamSourceTests
    {
        class StubTimestampStreamSource : ITimestampStreamSource
        {
            private readonly byte[] data;

            public StubTimestampStreamSource(string data, DateTime? timestamp = null, bool supportTimestamp = true)
            {
                this.data = Encoding.UTF8.GetBytes(data);
                Timestamp = timestamp;
                SupportTimestamp = supportTimestamp || timestamp.HasValue;
            }

            public bool SupportTimestamp { get; }

            public DateTime? Timestamp { get; }

            public Stream OpenSource()
            {
                return new MemoryStream(data);
            }
        }

        [Fact]
        public void CanIdentifyMostRecentStreamSource()
        {
            var a = new StubTimestampStreamSource("a", null);
            var b = new StubTimestampStreamSource("b", new DateTime(2017, 1, 1));
            var c = new StubTimestampStreamSource("c", null);
            var d = new StubTimestampStreamSource("d", new DateTime(2018, 1, 1));
            var e = new StubTimestampStreamSource("e", new DateTime(2016, 1, 1));
            var source = new MostRecentStreamSource(a, b, c, d, e);
            Assert.True(source.SupportTimestamp);
            Assert.Equal(d.Timestamp, source.Timestamp);
            using (var reader = new StreamReader(source.OpenSource(), Encoding.UTF8)) {
                var actual = reader.ReadToEnd();
                Assert.Equal("d", actual);
            }
        }
    }
}
