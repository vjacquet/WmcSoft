using System.Configuration;

namespace WmcSoft.Configuration
{
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

}
