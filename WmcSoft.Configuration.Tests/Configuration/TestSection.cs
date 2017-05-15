using System.Configuration;
using System.Linq;
using WmcSoft.Diagnostics.Checkpoints;

namespace WmcSoft.Configuration
{
    public class TestSection : ConfigurationSection
    {
        public TestSection()
        {
        }

        [ConfigurationProperty("mailPolicies")]
        public MailPolicyCollection MailPolicies {
            get { return (MailPolicyCollection)this["mailPolicies"]; }
        }

        [ConfigurationProperty("checkpoints")]
        public CheckpointCollection Checkpoints {
            get { return (CheckpointCollection)this["checkpoints"]; }
        }
    }

    public class CheckpointElement : FactoryElement<ICheckpoint>
    {
        [ConfigurationProperty("description", IsRequired = false, DefaultValue = "Checkpoint {0}")]
        public string Description {
            get { return (string)this["description"]; }
            set { this["description"] = value; }
        }

        [ConfigurationProperty("level", IsRequired = false, DefaultValue = 0)]
        [IntegerValidator(MinValue = 0, MaxValue = 10)]
        public int Level {
            get { return (int)this["level"]; }
            set { this["level"] = value; }
        }
    }

    [ConfigurationCollection(typeof(CheckpointElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class CheckpointCollection : ConfigurationElementCollection<CheckpointElement>
    {
        public new CheckpointElement this[string name] {
            get {
                return this.Cast<CheckpointElement>()
                    .SingleOrDefault(t => t.Name == name);
            }
        }
    }
}
