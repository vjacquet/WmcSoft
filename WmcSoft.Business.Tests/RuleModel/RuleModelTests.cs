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
        #region Helpers

        private const string basepath = @".\RuleModel";

        RuleSet DeserializeRuleModel(string fileName)
        {
            using (var ifs = new FileStream(Path.Combine(basepath, fileName), FileMode.Open, FileAccess.Read, FileShare.Read)) {
                var serializer = new XmlSerializer<RuleSet>();
                return serializer.Deserialize(ifs);
            }
        }

        RuleContext DeserializeRuleContext(string fileName)
        {
            using (var ifs = new FileStream(Path.Combine(basepath, fileName), FileMode.Open, FileAccess.Read, FileShare.Read)) {
                var serializer = new RuleContextSerializer();// new XmlSerializer<RuleContext>();
                return serializer.Deserialize(ifs);
            }
        }

        #endregion

        [TestMethod]
        public void CanDeserializeRuleModel()
        {
            var ruleSet = DeserializeRuleModel("TestRuleModel.rule");

            Assert.IsTrue(ruleSet.Version == "1.0");
            Assert.IsTrue(ruleSet.Name == "ruleSet1");
            Assert.AreEqual(2, ruleSet.Rules.Length);
        }

        [TestMethod]
        public void CanDeserializeRuleContext()
        {
            RuleContext ruleContext = DeserializeRuleContext("TestRuleContext.rulecontext");
            Assert.IsTrue(ruleContext.Version == "1.0");
        }

        [TestMethod]
        public void CanSerializeRuleContext()
        {
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
        public void CheckRuleContextItems()
        {
            var ruleContext = DeserializeRuleContext("TestRuleContext.rulecontext");

            Assert.IsTrue(((Variable)ruleContext["variable1"]).Value == "5");
            Assert.IsTrue(((Variable)ruleContext["variable2"]).Value == "5");
            Assert.IsTrue(((Proposition)ruleContext["proposition1"]).Value == true);
        }

        [TestMethod]
        public void CanEvaluate()
        {
            var ruleSet = DeserializeRuleModel("TestRuleModel.rule");
            var ruleContext = DeserializeRuleContext("TestRuleContext.rulecontext");

            Assert.IsTrue(ruleSet.Evaluate(ruleContext));
        }
    }
}
