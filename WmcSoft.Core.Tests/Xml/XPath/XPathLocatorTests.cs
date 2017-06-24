using System;
using System.Xml;
using Xunit;
using WmcSoft.Properties;

namespace WmcSoft.Xml.XPath
{
    public class XPathLocatorTests : IClassFixture<XPathLocatorTests.Fixture>
    {
        public class Fixture : XmlDocument
        {
            public Fixture()
            {
                LoadXml(TestResources.XPathLocator);
            }
        }

        readonly XmlDocument _document;

        public XPathLocatorTests(Fixture fixture)
        {
            _document = fixture;
        }

        [Fact]
        public void CanLocateDocument()
        {
            var xpath = _document.GetXPath();
            Assert.Equal("/", xpath);
        }

        [Fact]
        public void CanLocateDocumentElement()
        {
            var xpath = _document.DocumentElement.GetXPath();
            Assert.Equal("/rss", xpath);
        }

        [Fact]
        public void CanLocateAttribute()
        {
            var xpath = _document.DocumentElement.Attributes["version"].GetXPath();
            Assert.Equal("/rss/@version", xpath);
        }

        [Fact]
        public void CanLocateXmlNS()
        {
            var expected = _document.DocumentElement.Attributes[1];
            var xpath = expected.GetXPath();
            var actual = _document.SelectSingleNode(xpath);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanLocateElement()
        {
            var expected = _document.SelectSingleNode(".//item[3]/*[4]");
            var xpath = expected.GetXPath();

            var nsmgr = new XmlNamespaceManager(_document.NameTable);
            var ns = _document.NameTable.Get("dc");
            nsmgr.AddNamespace(ns, _document.DocumentElement.GetNamespaceOfPrefix(ns));//@"http://purl.org/dc/elements/1.1/");
            var actual = _document.SelectSingleNode(xpath, nsmgr);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanLocateNamedElement()
        {
            var expected = _document.SelectSingleNode(".//item[3]/category[5]");
            var xpath = expected.GetXPath();
            var actual = _document.SelectSingleNode(xpath);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanLocateProcessingInstruction()
        {
            var expected = _document.SelectSingleNode("/processing-instruction('xml-stylesheet')");
            var xpath = expected.GetXPath();
            var actual = _document.SelectSingleNode(xpath);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanLocateTextElements()
        {
            var sample = new XmlDocument();
            sample.LoadXml("<root />");
            sample.DocumentElement.AppendChild(sample.CreateTextNode("a"));
            sample.DocumentElement.AppendChild(sample.CreateTextNode("b"));
            sample.DocumentElement.AppendChild(sample.CreateElement("element"));
            sample.DocumentElement.AppendChild(sample.CreateTextNode("c"));
            sample.DocumentElement.AppendChild(sample.CreateElement("element"));
            sample.DocumentElement.AppendChild(sample.CreateTextNode("d"));
            sample.DocumentElement.AppendChild(sample.CreateTextNode("e"));
            sample.DocumentElement.AppendChild(sample.CreateTextNode("f"));

            var expected = sample.SelectSingleNode("/root/text()[3]");
            var xpath = expected.GetXPath();
            var actual = sample.SelectSingleNode(xpath);

            Assert.Equal(expected, actual);
        }
    }
}