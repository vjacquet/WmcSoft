using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Security
{
    public interface ISecurable
    {
        AccessControl AccessControl { get; }

        PermissionSet SupportedPermissions { get; }
    }

    public static class SecurableExtensions
    {
        public static S Grant<S>(this S securable, IEnumerable<Permission> permissions, params Principal[] principals)
            where S : ISecurable {
            securable.AccessControl.Grant(permissions, principals);
            return securable;
        }
        public static S Deny<S>(this S securable, IEnumerable<Permission> permissions, params Principal[] principals)
            where S : ISecurable {
            securable.AccessControl.Deny(permissions, principals);
            return securable;
        }
        public static S Revoke<S>(this S securable, IEnumerable<Permission> permissions, params Principal[] principals)
            where S : ISecurable {
            securable.AccessControl.Revoke(permissions, principals);
            return securable;
        }
    }
}
