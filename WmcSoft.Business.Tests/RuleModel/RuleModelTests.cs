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
            var serializer = new RuleContextSerializer();
            var sb = new StringBuilder();
            var w = new StringWriter(sb);
            serializer.Serialize(w, ruleContext);
        }

        [TestMethod]
        public void CanSerializeRuleContext() {
            RuleContext ruleContext = DeserializeRuleContext("TestRuleContext.rulecontext");

            Assert.IsTrue(ruleContext.Version == "1.0");
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
