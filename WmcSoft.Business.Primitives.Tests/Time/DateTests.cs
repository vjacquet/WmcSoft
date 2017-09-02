using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace WmcSoft.Time
{
    public class DateTests
    {
        private readonly ITestOutputHelper _output;

        public DateTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void AssertDateIsOfUnspecifiedKind()
        {
            var manual = new Date(1973, 5, 2);
            Assert.Equal(DateTimeKind.Unspecified, ((DateTime)manual).Kind);

            var dateTime = new DateTime(1973, 5, 2, 15, 30, 25, DateTimeKind.Local);
            var fromDateTime = (Date)dateTime;
            Assert.Equal(DateTimeKind.Unspecified, ((DateTime)fromDateTime).Kind);
        }

        [Fact]
        public void CheckAsTimepoint()
        {
            var manual = new Date(1973, 5, 2);
            var zone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            var tp = manual.AsTimepoint(zone);
            var dateTimeOffset = (DateTimeOffset)tp;
            var actual = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.DateTime, zone);
            Assert.Equal(1973, actual.Year);
            Assert.Equal(5, actual.Month);
            Assert.Equal(2, actual.Day);
        }

        [Fact]
        public void CanSerializeToXml()
        {
            var obj = new DateTestObject {
                Date = new Date(1977, 02, 22)
            };

            var serializer = new XmlSerializer(typeof(DateTestObject));
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb)) {
                serializer.Serialize(writer, obj);
            }

            var xml = sb.ToString();
            _output.WriteLine(xml);
            Assert.Contains(">1977-02-22<", xml);

            DateTestObject actual;
            using (var reader = XmlReader.Create(new StringReader(xml))) {
                actual = (DateTestObject)serializer.Deserialize(reader);
            }
            Assert.Equal(obj.Date, actual.Date);
        }

        [DataContract(Name = "TestObject")]
        public class DateTestObject
        {
            [DataMember]
            public Date Date { get; set; }
        }
    }
}
