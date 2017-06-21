using System;
using Xunit;

namespace WmcSoft.Business
{
    public class AutorizableTests
    {
        public class Entity : IAuthorizable
        {
            public string AuthorizedBy { get; set; }

            public DateTime? AuthorizedOn { get; set; }

            public void Authorize()
            {
                throw new NotImplementedException();
            }

            public bool CanAuthorize()
            {
                return true;
            }

            public bool CanRevoke()
            {
                return true;
            }

            public void Revoke()
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void CheckIsAutorized()
        {
            var a = new Entity { };
            var b = new Entity { AuthorizedBy = "me", AuthorizedOn = new DateTime(2016, 01, 16) };
            var today = new DateTime(2016, 01, 22);
            var before = new DateTime(2015, 01, 22);

            Assert.False(a.IsAuthorized());
            Assert.True(b.IsAuthorized());
            Assert.False(a.IsAuthorized(today));
            Assert.True(b.IsAuthorized(today));
            Assert.False(b.IsAuthorized(before));
        }
    }
}