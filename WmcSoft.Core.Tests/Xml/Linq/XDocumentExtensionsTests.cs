using System;
using System.Xml.Linq;
using Xunit;

namespace WmcSoft.Xml.Linq
{
    public class XDocumentExtensionsTests
    {
        [Fact]
        public void CheckDescendant()
        {
            var document = new XDocument(
                new XElement("Root",
                    new XElement("Child1", "data1"),
                    new XElement("Child2",
                        new XElement("Info", "info5"),
                        new XElement("Info", "info6")
                    ),
                    new XElement("Child3", "data3"),
                    new XElement("Child2",
                        new XElement("Info", "info7"),
                        new XElement("Info", "info8")
                    )
                )
            );

            var element = document.Descendant("Info");
            Assert.Equal("info5", element.Value);
        }

        [Fact]
        public void CheckSetValueWithDateTime()
        {
            var element = new XElement("Root");
            DateTime? dt;

            dt = null;
            element.SetValue(dt, "yyyyMMdd");
            Assert.Equal("", element.Value);

            dt = new DateTime(1789, 7, 14);
            element.SetValue(dt, "yyyyMMdd");
            Assert.Equal("17890714", element.Value);
        }

        [Fact]
        public void CheckSetValueWithInt()
        {
            var element = new XElement("Root");
            int? n;

            n = null;
            element.SetValue(n, "x");
            Assert.Equal("", element.Value);

            n = 42;
            element.SetValue(n, "x");
            Assert.Equal("2a", element.Value);
        }

    }
}
