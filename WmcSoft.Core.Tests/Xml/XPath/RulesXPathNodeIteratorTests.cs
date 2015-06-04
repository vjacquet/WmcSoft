using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Xml.XPath
{
    [TestClass]
    public class RulesXPathNodeIteratorTests
    {
        const string Data = @"<data>
    <item id=""a01"">A</item>
    <item id=""b01"">B</item>
    <item id=""c01"">C</item>
</data>";

        const string Rules = @"<rules>
  <ignore name=""ignore b01"" idrefs=""b01""/>
  <ignore name=""ignore d01"" idrefs=""d01""/>
</rules>";

        public static XPathNodeIterator CreateRuleSet(XPathNavigator current, XPathNodeIterator rules, Dictionary<string, XPathExpression> expressions) {
            return new RulesXPathNodeIterator(rules, current, expressions);
        }

        public static XPathNavigator CreateNavigator(string xml) {
            var document = new XmlDocument();
            document.LoadXml(xml);
            return document.DocumentElement.CreateNavigator();
        }

        [TestMethod]
        public void CheckRules() {
            var data = CreateNavigator(Data);
            var expressions = new Dictionary<string, XPathExpression>();
            expressions.Add("idrefs", XPathExpression.Compile("@id", data));
            var rules = CreateNavigator(Rules);

            var actual = new NameValueCollection();
            foreach (XPathNavigator item in data.SelectChildren(XPathNodeType.Element)) {
                var id = item.GetAttribute("id", "");
                foreach (XPathNavigator element in CreateRuleSet(item, rules.Select("ignore"), expressions)) {
                    var rule = element.GetAttribute("name", "");
                    actual.Add(id, rule);
                }
            }

            Assert.IsNull(actual["a01"]);
            Assert.IsNotNull(actual["b01"]);
            Assert.AreEqual(actual["b01"], "ignore b01");
            Assert.IsNull(actual["c01"]);
        }
    }
}
