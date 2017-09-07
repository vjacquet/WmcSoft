using System;
using System.IO;
using Xunit;
using System.Runtime.Serialization.Formatters.Binary;

namespace WmcSoft.Time
{
    public class TimepointTests
    {
        [Fact]
        public void AssertDateTimeKindIsPreservedWhileSerializing()
        {
            using (var ms = new MemoryStream()) {
                var serializer = new BinaryFormatter();

                var expected = new DateTime(2016, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                serializer.Serialize(ms, expected);

                ms.Seek(0, SeekOrigin.Begin);
                var actual = serializer.Deserialize(ms);

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void CanAddMonthsToTimepoint()
        {
            var date = new DateTime(1973, 05, 02, 0, 0, 0, DateTimeKind.Local);
            var timepoint = new Timepoint(date);
            var duration = Duration.Months(15);
            var actual = timepoint + duration;
            var expected = new Timepoint(new DateTime(1974, 08, 02, 0, 0, 0, DateTimeKind.Local));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddMSecondsToTimepoint()
        {
            var date = new DateTime(1973, 05, 02, 0, 0, 0, DateTimeKind.Local);
            var timepoint = new Timepoint(date);
            var duration = Duration.Seconds(15);
            var actual = timepoint + duration;
            var expected = new Timepoint(new DateTime(1973, 05, 02, 0, 0, 15, DateTimeKind.Local));
            Assert.Equal(expected, actual);
        }
    }
}
