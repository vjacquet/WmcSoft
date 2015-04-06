using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Tests.Properties;
using WmcSoft.Xml;

namespace WmcSoft.Tests.Xml.XPath
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
        public void CanLocateDocumentElement() {
            string xpath = document.DocumentElement.GetXPath();

            Assert.AreEqual("/rss", xpath);
        }

        [TestMethod]
        public void CanLocateAttribute() {
            string xpath = document.DocumentElement.Attributes["version"].GetXPath();

            Assert.AreEqual("/rss/@version", xpath);
        }

        [TestMethod]
        public void CanLocateXmlNS() {
            XmlNode expected = document.DocumentElement.Attributes[1];

            string xpath = expected.GetXPath();
            XmlNode actual = document.SelectSingleNode(xpath);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanLocateElement() {
            XmlNode expected = document.SelectSingleNode(".//item[3]/*[4]");

            string xpath = expected.GetXPath();

            var nsmgr = new XmlNamespaceManager(document.NameTable);
            string ns = document.NameTable.Get("dc");
            nsmgr.AddNamespace(ns, document.DocumentElement.GetNamespaceOfPrefix(ns));//@"http://purl.org/dc/elements/1.1/");
            XmlNode actual = document.SelectSingleNode(xpath, nsmgr);

            Assert.AreEqual(expected, actual);
        }
    }
}
