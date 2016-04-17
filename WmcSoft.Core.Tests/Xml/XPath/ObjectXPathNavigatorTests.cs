using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Xml.XPath
{
    [TestClass]
    public class ObjectXPathNavigatorTests
    {
        [TestMethod]
        public void CanConvertObjectToXml() {
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
            Assert.IsNotNull(actual);
        }
    }

    public class Sample
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string[] Roles { get; set; }
    }
}
