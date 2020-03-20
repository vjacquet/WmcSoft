using System.Net.Mail;
using Xunit;

namespace WmcSoft.Net.Mail
{
    public class SmtpExtensionsTests
    {
        [Fact]
        public void CanAddRangeWithoutCollision()
        {
            var actual = new[] {
                new MailAddress("john@example.com"),
                new MailAddress("jane@example.com"),
            };

            var addresses = new MailAddressCollection();
            addresses.AddRange(actual);

            Assert.Equal(2, addresses.Count);
        }

        [Fact]
        public void CanAddRangeWithCollisions()
        {
            var actual = new[] {
                new MailAddress("john@example.com"),
                new MailAddress("jane@example.com"),
                new MailAddress("john@example.com"),
            };

            var addresses = new MailAddressCollection();
            addresses.AddRange(actual);

            Assert.Equal(2, addresses.Count);
        }

        [Fact]
        public void CanRemoveAll()
        {
            var actual = new[] {
                new MailAddress("john@example.com"),
                new MailAddress("jane@example.com"),
            };

            var addresses = new MailAddressCollection();
            addresses.Add(new MailAddress("jane@example.com"));
            addresses.Add(new MailAddress("john@example.com"));
            addresses.RemoveAll(actual);

            Assert.Empty(addresses);
        }
    }
}
