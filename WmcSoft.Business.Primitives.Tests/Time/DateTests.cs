using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
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

        [Theory]
        [InlineData("2017-09-01")]
        [InlineData("2017-09-02")]
        [InlineData("2017-09-03")]
        [InlineData("2017-09-04")]
        [InlineData("2017-09-05")]
        [InlineData("2017-09-06")]
        [InlineData("2017-09-07")]
        [InlineData("2017-09-12")]
        [InlineData("2016-02-29")]
        public void CanCreateDateAndGetDateParts(DateTime dateTime)
        {
            var date = new Date(dateTime.Year, dateTime.Month, dateTime.Day);
            Assert.Equal(date.Year, dateTime.Year);
            Assert.Equal(date.Month, dateTime.Month);
            Assert.Equal(date.Day, dateTime.Day);
            Assert.Equal(date.DayOfWeek, dateTime.DayOfWeek);
        }

        [Theory]
        [InlineData("2016-02-28")]
        [InlineData("2016-02-29")]
        [InlineData("2016-11-30")]
        public void CanAddParts(DateTime dateTime)
        {
            var date = new Date(dateTime.Year, dateTime.Month, dateTime.Day);

            Assert.Equal(dateTime.AddDays(1), (DateTime)date.AddDays(1));
            Assert.Equal(dateTime.AddMonths(1), (DateTime)date.AddMonths(1));
            Assert.Equal(dateTime.AddYears(1), (DateTime)date.AddYears(1));
        }

        [Fact]
        public void DateMinValueAndMaxValueAreCompatibleWithDateTime()
        {
            Assert.Equal(DateTime.MinValue.Date, (DateTime)Date.MinValue);
            Assert.Equal(DateTime.MaxValue.Date, (DateTime)Date.MaxValue);
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

        [Fact]
        public void CanSerializeToJson()
        {
            var obj = new DateTestObject {
                Date = new Date(1977, 02, 22)
            };

            var json = JsonConvert.SerializeObject(obj);
            Assert.Contains("\"1977-02-22\"", json);

            var actual = JsonConvert.DeserializeObject<DateTestObject>(json);
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
