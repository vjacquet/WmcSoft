using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Security
{
    public abstract class SecurableBase : ISecurable
    {
        private readonly AccessControl _accessControl;

        protected SecurableBase() {
            _accessControl = new AccessControl();
        }
        #region ISecurable Members

        public AccessControl AccessControl {
            get { return _accessControl; }
        }

        public abstract PermissionSet SupportedPermissions {
            get;
        }

        #endregion
    }
}
