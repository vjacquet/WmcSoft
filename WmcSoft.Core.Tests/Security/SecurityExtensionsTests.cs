using System.Security.Permissions;
using System.Security.Principal;
using Xunit;

namespace WmcSoft.Security
{
    public class SecurityExtensionsTests
    {
        [Fact]
        public void CheckIsGranted()
        {
            var method = typeof(SecuredObject).GetMethod("ForAdministratorOnly");

            Assert.False(method.IsGranted());

            var admin = new GenericPrincipal(new GenericIdentity("admin"), new string[] { "Administrator" });
            Assert.True(admin.IsGranted(method));
        }

        [Fact]
        public void CheckImpersonate()
        {
            var method = typeof(SecuredObject).GetMethod("ForAdministratorOnly");
            var admin = new GenericPrincipal(new GenericIdentity("admin"), new string[] { "Administrator" });
            using (admin.Impersonate()) {
                Assert.True(method.IsGranted());

                var attr = new PrincipalPermissionAttribute(SecurityAction.Assert) {
                    Authenticated = true,
                    Role = "Administrator"
                };
                attr.CreatePermission().Demand();
            }
        }
    }

    class SecuredObject
    {
        [PrincipalPermission(SecurityAction.Assert, Authenticated = true, Role = "Administrator")]
        public void ForAdministratorOnly()
        {

        }
    }
}
