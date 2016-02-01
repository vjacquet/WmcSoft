using System.Security.Permissions;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Security
{
    [TestClass]
    public class SecurityExtensionsTests
    {
        [TestMethod]
        public void CheckIsGranted() {
            var method = typeof(SecuredObject).GetMethod("ForAdministratorOnly");

            Assert.IsFalse(method.IsGranted());

            var admin = new GenericPrincipal(new GenericIdentity("admin"), new string[] { "Administrator" });
            admin.IsGranted(method);
        }

        [TestMethod]
        public void CheckImpersonate() {
            var method = typeof(SecuredObject).GetMethod("ForAdministratorOnly");
            var admin = new GenericPrincipal(new GenericIdentity("admin"), new string[] { "Administrator" });
            using (admin.Impersonate()) {
                Assert.IsTrue(method.IsGranted());
            }
        }
    }

    class SecuredObject
    {
        [PrincipalPermission(SecurityAction.Assert, Authenticated = true, Role = "Administrator")]
        public void ForAdministratorOnly() {

        }
    }
}
