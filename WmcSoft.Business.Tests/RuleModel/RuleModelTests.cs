using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Runtime.Serialization;
using System.Text;

namespace WmcSoft.Business.RuleModel
{
    [TestClass]
    public class RuleModelTests
    {
        private const string basepath = @".\RuleModel";

        RuleSet DeserializeRuleModel(string fileName) {
            FileStream ifs = new FileStream(
                Path.Combine(basepath, fileName),
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

            try {
                var serializer = new XmlSerializer<RuleSet>();
                var ruleSet = (RuleSet)serializer.Deserialize(ifs);
                return ruleSet;
            }
            finally {
                ifs.Close();
            }
        }

        [TestMethod]
        public void CanDeserializeRuleModel() {
            RuleSet ruleSet = DeserializeRuleModel("TestRuleModel.rule");

            Assert.IsTrue(ruleSet.Version == "1.0");
            Assert.IsTrue(ruleSet.Name == "ruleSet1");
            Assert.AreEqual(2, ruleSet.Rules.Length);
        }

        RuleContext DeserializeRuleContext(string fileName) {
            FileStream ifs = new FileStream(
                Path.Combine(basepath, fileName),
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

            try {
                var serializer = new RuleContextSerializer();// new XmlSerializer<RuleContext>();
                var ruleContext = serializer.Deserialize(ifs);
                return ruleContext;
            }
            finally {
                ifs.Close();
            }
        }

        [TestMethod]
        public void CanDeserializeRuleContext() {
            RuleContext ruleContext = DeserializeRuleContext("TestRuleContext.rulecontext");
            Assert.IsTrue(ruleContext.Version == "1.0");
        }

        [TestMethod]
        public void CanSerializeRuleContext() {
            var expected = new RuleContext { Version = "1.0" };
            expected.Items = new RuleElement[] {
                new Variable { Name="var1", Value="value1" },
                new Variable { Name="var2", Value="value2" },
                new Proposition { Name="prop1", Value=true },
                new RuleOverride { Name="rule1", Value=true, Reference="ref", Why="because", When=new DateTime(2015, 01, 22) },
            };

            var serializer = new RuleContextSerializer();
            var sb = new StringBuilder();
            var w = new StringWriter(sb);
            serializer.Serialize(w, expected);
            var actual = serializer.Deserialize(new StringReader(sb.ToString()));

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Version, actual.Version);
            CollectionAssert.AreEquivalent(expected.Items, actual.Items);
        }

        [TestMethod]
        public void CheckRuleContextItems() {
            RuleContext ruleContext = DeserializeRuleContext("TestRuleContext.rulecontext");

            Assert.IsTrue(((Variable)ruleContext["variable1"]).Value == "5");
            Assert.IsTrue(((Variable)ruleContext["variable2"]).Value == "5");
            Assert.IsTrue(((Proposition)ruleContext["proposition1"]).Value == true);
        }

        [TestMethod]
        public void CanEvaluate() {
            RuleSet ruleSet = DeserializeRuleModel("TestRuleModel.rule");
            RuleContext ruleContext = DeserializeRuleContext("TestRuleContext.rulecontext");

            Assert.IsTrue(ruleSet.Evaluate(ruleContext));
        }
    }
}
