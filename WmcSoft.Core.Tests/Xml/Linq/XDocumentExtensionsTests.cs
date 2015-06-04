using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Xml.Linq
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
        public void CheckSetValueWithDateTime() {
            var element = new XElement("Root");
            DateTime? dt;

            dt = null;
            element.SetValue(dt, "yyyyMMdd");
            Assert.AreEqual("", element.Value);

            dt = new DateTime(1789, 7, 14);
            element.SetValue(dt, "yyyyMMdd");
            Assert.AreEqual("17890714", element.Value);
        }

        [TestMethod]
        public void CheckSetValueWithInt() {
            var element = new XElement("Root");
            int? n;

            n = null;
            element.SetValue(n, "x");
            Assert.AreEqual("", element.Value);

            n = 42;
            element.SetValue(n, "x");
            Assert.AreEqual("2a", element.Value);
        }

    }
}
