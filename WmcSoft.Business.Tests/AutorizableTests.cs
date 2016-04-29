using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Business
{
    [TestClass]
    public class AutorizableTests
    {
        public class Entity : IAuthorizable
        {
            public string AuthorizedBy { get; set; }

            public DateTime? AuthorizedOn { get; set; }

            public void Authorize() {
                throw new NotImplementedException();
            }

            public bool CanAuthorize() {
                return true;
            }

            public bool CanRevoke() {
                return true;
            }

            public void Revoke() {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void CheckIsAutorized() {
            var a = new Entity { };
            var b = new Entity { AuthorizedBy = "me", AuthorizedOn = new DateTime(2016, 01, 16) };
            var today = new DateTime(2016, 01, 22);
            var before = new DateTime(2015, 01, 22);

            Assert.IsFalse(a.IsAuthorized());
            Assert.IsTrue(b.IsAuthorized());
            Assert.IsFalse(a.IsAuthorized(today));
            Assert.IsTrue(b.IsAuthorized(today));
            Assert.IsFalse(b.IsAuthorized(before));
        }
    }
}
