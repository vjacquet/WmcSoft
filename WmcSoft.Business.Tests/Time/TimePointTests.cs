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
        public void AssertDateTimeKindIsPreservedWhileSerializing() {
            using (var ms = new MemoryStream()) {
                var serializer = new BinarySerializer<DateTime>();

                var expected = new DateTime(2016, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                serializer.Serialize(ms, expected);

                ms.Rewind();
                var actual = serializer.Deserialize(ms);

                Assert.AreEqual(expected, actual);
            }
        }
    }
}
