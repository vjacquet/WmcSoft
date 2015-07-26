using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Business.RuleModel
{
    /// <summary>
    /// Description résumée de RuleModelTests.
    /// </summary>
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
                var serializer = new XmlSerializer(typeof(RuleSet));
                var ruleSet = (RuleSet)serializer.Deserialize(ifs);
                return ruleSet;
            }
            finally {
                ifs.Close();
            }
        }

        [TestMethod]
        public void DeserializeRuleModel() {
            RuleSet ruleSet = DeserializeRuleModel("TestRuleModel.rule");

            Assert.IsTrue(ruleSet.Version == "1.0");
            Assert.IsTrue(ruleSet.Name == "ruleSet1");
        }

        RuleContext DeserializeRuleContext(string fileName) {
            FileStream ifs = new FileStream(
                Path.Combine(basepath, fileName),
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

            try {
                XmlSerializer serializer = new XmlSerializer(typeof(RuleContext));
                RuleContext ruleContext = (RuleContext)serializer.Deserialize(ifs);
                return ruleContext;
            }
            finally {
                ifs.Close();
            }
        }

        [TestMethod]
        public void DeserializeRuleContext() {
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
        public void Evaluate() {
            RuleSet ruleSet = DeserializeRuleModel("TestRuleModel.rule");
            RuleContext ruleContext = DeserializeRuleContext("TestRuleContext.rulecontext");

            Assert.IsTrue(ruleSet.Evaluate(ruleContext));
        }
    }
}
