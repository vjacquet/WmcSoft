using System.Configuration;
using System.Net.Mail;
using Xunit;

namespace WmcSoft.Configuration
{
    public class MailPolicyTests
    {
        [Fact]
        public void CanLoadMailPolicies()
        {
            var section = (TestSection)ConfigurationManager.GetSection("wmc");
            var policy = section.MailPolicies["SendNewsletter"];
            Assert.NotNull(policy);
            Assert.Equal("Newsletter", policy.Subject);
            Assert.Contains(new MailAddress("noreply@wmcsoft.com"), policy.ReplyTo);
            Assert.Contains(new MailAddress("subscribers@rules.wmcsoft.com"), policy.To);
            Assert.Contains(new MailAddress("webmaster@wmcsoft.com"), policy.Bcc);
        }
    }
}