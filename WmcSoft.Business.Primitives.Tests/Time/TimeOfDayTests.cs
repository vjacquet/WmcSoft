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
    public class TimeOfDayTests
    {
        private readonly ITestOutputHelper _output;

        public TimeOfDayTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CanCreateTimeOfDay()
        {
            var time = new TimeOfDay(8, 30);

            Assert.Equal(8, time.Hour);
            Assert.Equal(30, time.Minutes);
        }

        [Fact]
        public void CanDeconstructTimeOfDay()
        {
            var time = new TimeOfDay(8, 30);
            var (h, m) = time;

            Assert.Equal(8, h);
            Assert.Equal(30, m);
        }

        [Fact]
        public void CanCompareTimes()
        {
            var h0830 = new TimeOfDay(8, 30);
            var h1120 = new TimeOfDay(11, 20);

            Assert.False(h0830.IsAfter(h1120));
            Assert.True(h0830.IsBefore(h1120));
        }

        [Fact]
        public void CanConvertToDateTime()
        {
            var date = new Date(2016, 10, 1);
            var time = new TimeOfDay(8, 30);
            var zone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            var dateTimeOffset = time.On(date, zone);
            Assert.Equal(2016, dateTimeOffset.Year);
            Assert.Equal(10, dateTimeOffset.Month);
            Assert.Equal(1, dateTimeOffset.Day);
            Assert.Equal(8, dateTimeOffset.Hour);
            Assert.Equal(30, dateTimeOffset.Minute);
        }

        [Fact]
        public void CanSerializeToXml()
        {
            var obj = new TimeOfDayTestObject {
                TimeOfDay = new TimeOfDay(8, 30)
            };

            var serializer = new XmlSerializer(typeof(TimeOfDayTestObject));
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb)) {
                serializer.Serialize(writer, obj);
            }

            var xml = sb.ToString();
            _output.WriteLine(xml);
            Assert.Contains(">08:30<", xml);

            TimeOfDayTestObject actual;
            using (var reader = XmlReader.Create(new StringReader(xml))) {
                actual = (TimeOfDayTestObject)serializer.Deserialize(reader);
            }
            Assert.Equal(obj.TimeOfDay, actual.TimeOfDay);
        }

        [DataContract(Name = "TestObject")]
        public class TimeOfDayTestObject
        {
            [DataMember]
            public TimeOfDay TimeOfDay { get; set; }
        }
    }
}
