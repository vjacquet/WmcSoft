using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Security
{
    public class Command : SecurableBase
    {
        static readonly PermissionSet _permissions = Permissions.Execute | Permissions.Abort;

        public override PermissionSet SupportedPermissions {
            get { return _permissions; }
        }
    }
}
