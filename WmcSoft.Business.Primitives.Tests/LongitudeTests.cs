using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace WmcSoft
{
    public class LongitudeTests
    {
        private readonly ITestOutputHelper _output;

        public LongitudeTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CanToStringLongitude()
        {
            var iv = CultureInfo.InvariantCulture;
            var l = new Longitude(49, 30, 15);

            Assert.Equal("49° 30' 15\"", l.ToString(iv));
            Assert.Equal("49° 30.25'", l.ToString("M", iv));
            Assert.Equal("49.5042°", l.ToString("D", iv));
        }

        [Fact]
        public void CanToStringLatitudeWithNegativeValues()
        {
            var iv = CultureInfo.InvariantCulture;
            var l = new Latitude(-49, 30);

            Assert.Equal("-49° 30' 00\"", l.ToString(iv));
            Assert.Equal("-49° 30.00'", l.ToString("M", iv));
            Assert.Equal("-49.5000°", l.ToString("D", iv));
        }

        [Fact]
        public void CanDeconstructLongitude()
        {
            var l = new Longitude(10, 15, 30);
            var (d, m, s) = l;

            Assert.Equal(10, d);
            Assert.Equal(15, m);
            Assert.Equal(30, s);
        }

        [Fact]
        public void CanDeconstructLatitude()
        {
            var l = new Latitude(-10, 15, 30);
            var (d, m, s) = l;

            Assert.Equal(-10, d);
            Assert.Equal(15, m);
            Assert.Equal(30, s);
        }

        [Fact]
        public void CanSerializeToXml()
        {
            var obj = new TestObject {
                Longitude = new Longitude(1.2345m)
            };

            var serializer = new XmlSerializer(typeof(TestObject));
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb)) {
                serializer.Serialize(writer, obj);
            }

            var xml = sb.ToString();
            _output.WriteLine(xml);
            Assert.Contains(">1.2345<", xml);

            TestObject actual;
            using (var reader = XmlReader.Create(new StringReader(xml))) {
                actual = (TestObject)serializer.Deserialize(reader);
            }
            Assert.Equal(obj.Longitude, actual.Longitude);
        }

        [Fact]
        public void CanSerializeToJson()
        {
            var obj = new TestObject {
                Longitude = new Longitude(2.294481m)
            };

            var json = JsonConvert.SerializeObject(obj);
            Assert.Contains("\"+002.294481\"", json);

            var actual = JsonConvert.DeserializeObject<TestObject>(json);
            Assert.Equal(obj.Longitude, actual.Longitude);
        }

        [DataContract(Name = "TestObject")]
        public class TestObject
        {
            [DataMember]
            public Longitude Longitude { get; set; }
        }
    }
}
