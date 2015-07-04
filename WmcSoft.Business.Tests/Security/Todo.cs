using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Security
{
    public class Todo : SecurableBase
    {
        static readonly PermissionSet _permissions = Permissions.Read | Permissions.Write | Permissions.Delegate;

        public ISecurable Customer { get; set; }

        public override PermissionSet SupportedPermissions {
            get { return _permissions; }
        }
    }
}
