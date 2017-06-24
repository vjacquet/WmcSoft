using System.Xml;
using Xunit;

namespace WmcSoft.Xml
{
    public class XmlNodeWriterTests
    {
        [Fact]
        public void CheckReplaceOnCurrentNode()
        {
            var document = new XmlDocument();
            document.InnerXml = "<root><a/></root>";
            using (var writer = document.DocumentElement.ReplaceSelf()) {
                writer.WriteStartElement("b");
            }
            Assert.Equal("b", document.DocumentElement.LocalName);
        }

        [Fact]
        public void CheckReplaceContentOnCurrentNode()
        {
            var document = new XmlDocument();
            document.InnerXml = "<root><a/><a/></root>";
            using (var writer = document.DocumentElement.ReplaceContent()) {
                writer.WriteStartElement("b");
            }
            Assert.Equal("b", document.DocumentElement.FirstChild.LocalName);
        }
    }
}
