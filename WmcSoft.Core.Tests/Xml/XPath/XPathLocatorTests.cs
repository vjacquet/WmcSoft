using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Properties;

namespace WmcSoft.Xml.XPath
{
    [TestClass]
    public class XPathLocatorTests
    {
        static XmlDocument document;

        [ClassInitialize]
        public static void Initialize(TestContext context) {
            document = new XmlDocument();
            document.LoadXml(Resources.XPathLocator);
        }

        [TestMethod]
        public void CanLocateDocument() {
            var xpath = document.GetXPath();
            Assert.AreEqual("/", xpath);
        }

        [TestMethod]
        public void CanLocateDocumentElement() {
            var xpath = document.DocumentElement.GetXPath();
            Assert.AreEqual("/rss", xpath);
        }

        [TestMethod]
        public void CanLocateAttribute() {
            var xpath = document.DocumentElement.Attributes["version"].GetXPath();
            Assert.AreEqual("/rss/@version", xpath);
        }

        [TestMethod]
        public void CanLocateXmlNS() {
            var expected = document.DocumentElement.Attributes[1];
            var xpath = expected.GetXPath();
            var actual = document.SelectSingleNode(xpath);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanLocateElement() {
            var expected = document.SelectSingleNode(".//item[3]/*[4]");
            var xpath = expected.GetXPath();

            var nsmgr = new XmlNamespaceManager(document.NameTable);
            var ns = document.NameTable.Get("dc");
            nsmgr.AddNamespace(ns, document.DocumentElement.GetNamespaceOfPrefix(ns));//@"http://purl.org/dc/elements/1.1/");
            var actual = document.SelectSingleNode(xpath, nsmgr);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanLocateNamedElement() {
            var expected = document.SelectSingleNode(".//item[3]/category[5]");
            var xpath = expected.GetXPath();
            var actual = document.SelectSingleNode(xpath);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanLocateProcessingInstruction() {
            var expected = document.SelectSingleNode("/processing-instruction('xml-stylesheet')");
            var xpath = expected.GetXPath();
            var actual = document.SelectSingleNode(xpath);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanLocateTextElements() {
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

            Assert.AreEqual(expected, actual);
        }
    }
}
