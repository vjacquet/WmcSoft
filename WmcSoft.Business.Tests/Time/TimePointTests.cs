using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.IO;
using WmcSoft.Runtime.Serialization;

namespace WmcSoft.Time
{
    [TestClass]
    public class TimePointTests
    {
        [TestMethod]
        public void AssertDateTimeKindIsPreservedWhileSerializing()
        {
            using (var ms = new MemoryStream()) {
                var serializer = new BinarySerializer<DateTime>();

                var expected = new DateTime(2016, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                serializer.Serialize(ms, expected);

                ms.Rewind();
                var actual = serializer.Deserialize(ms);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void CanAddMonthsToTimePoint()
        {
            var date = new DateTime(1973, 05, 02, 0, 0, 0, DateTimeKind.Local);
            var timepoint = new TimePoint(date);
            var duration = Duration.Months(15);
            var actual = timepoint + duration;
            var expected = new TimePoint(new DateTime(1974, 08, 02, 0, 0, 0, DateTimeKind.Local));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanAddMSecondsToTimePoint()
        {
            var date = new DateTime(1973, 05, 02, 0, 0, 0, DateTimeKind.Local);
            var timepoint = new TimePoint(date);
            var duration = Duration.Seconds(15);
            var actual = timepoint + duration;
            var expected = new TimePoint(new DateTime(1973, 05, 02, 0, 0, 15, DateTimeKind.Local));
            Assert.AreEqual(expected, actual);
        }
    }
}
