using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Xml.Linq.Tests
{
    [TestClass]
    public class XDocumentExtensionsTests
    {
        [TestMethod]
        public void CheckDescendant() {
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
            Assert.AreEqual("info5", element.Value);
        }

        [TestMethod]
        public void CheckSetValue() {
            var element = new XElement("Root");
            var date = new DateTime(1789, 7, 14);
            element.SetValue(date, "yyyyMMdd");
            Assert.AreEqual("17890714", element.Value);
        }
    }
}
