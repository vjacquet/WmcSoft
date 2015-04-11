using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Configuration.Tests
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
