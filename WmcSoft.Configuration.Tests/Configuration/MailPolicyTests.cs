using System;
using System.Configuration;
using System.Net.Mail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Configuration
{
    [TestClass]
    public class MailPolicyTests
    {
        [TestMethod]
        public void CanLoadMailPolicies() {
            var section = (TestSection)ConfigurationManager.GetSection("wmc");
            var policy = section.MailPolicies["SendNewsletter"];
            Assert.IsNotNull(policy);
            Assert.AreEqual("Newsletter", policy.Subject);
            CollectionAssert.Contains(policy.ReplyTo, new MailAddress("noreply@wmcsoft.com"));
            CollectionAssert.Contains(policy.To, new MailAddress("subscribers@rules.wmcsoft.com"));
            CollectionAssert.Contains(policy.Bcc, new MailAddress("webmaster@wmcsoft.com"));
        }
    }
}
