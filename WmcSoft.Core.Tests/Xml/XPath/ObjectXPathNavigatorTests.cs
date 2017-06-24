using System;
using System.Text;
using System.Xml;
using Xunit;

namespace WmcSoft.Xml.XPath
{
    public class ObjectXPathNavigatorTests
    {
        [Fact]
        public void CanConvertObjectToXml()
        {
            var data = new Sample {
                Name = "name",
                Date = new DateTime(1973, 5, 2),
                //Roles = new[] { "Administrator", "Contributor" },
            };
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings {
                Indent = true,
            };
            var nav = new ObjectXPathNavigator(data);
            using (var writer = XmlWriter.Create(sb, settings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("data");
                writer.WriteNode(nav, true);
            }
            var actual = sb.ToString();
            Assert.NotNull(actual);
        }
    }

    public class Sample
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string[] Roles { get; set; }
    }
}
