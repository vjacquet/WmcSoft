using System.Configuration;

namespace WmcSoft.Configuration
{
    public class TestSection : ConfigurationSection
    {
        public TestSection() {
        }

        [ConfigurationProperty("mailPolicies")]
        public MailPolicyCollection MailPolicies {
            get {
                return (MailPolicyCollection)this["mailPolicies"];
            }
        }
    }
}
