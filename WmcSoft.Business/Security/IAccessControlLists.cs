using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Security
{
    public interface IAccessControlLists
    {
        ISet<AccessControlEntry> GrantedPermissions { get; }
        ISet<AccessControlEntry> DeniedPermissions { get; }

        void Grant(IEnumerable<Permission> permissions, params Principal[] principals);
        void Deny(IEnumerable<Permission> permissions, params Principal[] principals);
        void Revoke(IEnumerable<Permission> permissions, params Principal[] principals);

        bool Verify(Principal principal, Permission permission);
        IEnumerable<Permission> Verify(Principal principal, IEnumerable<Permission> permissions);
    }
}
