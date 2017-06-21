using System;
using System.IO;
using System.Text;
using Xunit;
using WmcSoft.Runtime.Serialization;

namespace WmcSoft.Business.RuleModel
{
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

        [Fact]
        public void CanDeserializeRuleModel()
        {
            var ruleSet = DeserializeRuleModel("TestRuleModel.rule");

            Assert.True(ruleSet.Version == "1.0");
            Assert.True(ruleSet.Name == "ruleSet1");
            Assert.Equal(2, ruleSet.Rules.Length);
        }

        [Fact]
        public void CanDeserializeRuleContext()
        {
            RuleContext ruleContext = DeserializeRuleContext("TestRuleContext.rulecontext");
            Assert.True(ruleContext.Version == "1.0");
        }

        [Fact]
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

            Assert.NotNull(actual);
            Assert.Equal(expected.Version, actual.Version);
            Assert.Equal(expected.Items, actual.Items);
        }

        [Fact]
        public void CheckRuleContextItems()
        {
            var ruleContext = DeserializeRuleContext("TestRuleContext.rulecontext");

            Assert.True(((Variable)ruleContext["variable1"]).Value == "5");
            Assert.True(((Variable)ruleContext["variable2"]).Value == "5");
            Assert.True(((Proposition)ruleContext["proposition1"]).Value == true);
        }

        [Fact]
        public void CanEvaluate()
        {
            var ruleSet = DeserializeRuleModel("TestRuleModel.rule");
            var ruleContext = DeserializeRuleContext("TestRuleContext.rulecontext");

            Assert.True(ruleSet.Evaluate(ruleContext));
        }
    }
}
