using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Xml
{
    [TestClass]
    public class XmlNodeWriterTests
    {
        [TestMethod]
        public void CheckReplaceOnCurrentNode() {
            var document = new XmlDocument();
            document.InnerXml = "<root><a/></root>";
            using (var writer = document.DocumentElement.ReplaceSelf()) {
                writer.WriteStartElement("b");
            }
            Assert.AreEqual("b", document.DocumentElement.LocalName);
        }

        [TestMethod]
        public void CheckReplaceContentOnCurrentNode() {
            var document = new XmlDocument();
            document.InnerXml = "<root><a/><a/></root>";
            using (var writer = document.DocumentElement.ReplaceContent()) {
                writer.WriteStartElement("b");
            }
            Assert.AreEqual("b", document.DocumentElement.FirstChild.LocalName);
        }
    }
}
