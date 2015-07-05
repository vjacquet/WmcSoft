#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Security;

namespace WmcSoft.Security
{
    public sealed class AccessControl
    {
        IAccessControlLists _acls;
        bool _shared;

        public AccessControl() {
            _acls = new AccessControlLists();
        }

        #region IAccessControlList Membres

        public bool IsUnique {
            get { return !_shared; }
        }

        public void Grant(IEnumerable<Permission> permissions, params Principal[] principals) {
            _acls.Grant(permissions, principals);
        }

        public void Deny(IEnumerable<Permission> permissions, params Principal[] principals) {
            _acls.Deny(permissions, principals);
        }

        public void Revoke(IEnumerable<Permission> permissions, params Principal[] principals) {
            _acls.Revoke(permissions, principals);
        }

        public IEnumerable<Permission> Verify(Principal principal, IEnumerable<Permission> permissions) {
            return _acls.Verify(principal, permissions);
        }

        #endregion

        public void Assert(Principal principal, Permission permission) {
            if (!_acls.Verify(principal, permission))
                throw new SecurityException();
        }

        public bool this[Principal principal, Permission permission] {
            get {
                return _acls.Verify(principal, permission);
            }
        }

        internal void MakeUnique() {
            if (_shared) {
                _acls = new AccessControlLists(_acls);
                _shared = false;
            }
        }

        internal void MergeWith(AccessControl acls) {
            _acls.GrantedPermissions.UnionWith(acls._acls.GrantedPermissions);
            _acls.DeniedPermissions.UnionWith(acls._acls.DeniedPermissions);
            acls._acls = _acls;
            _shared = true;
            acls._shared = true;
        }
    }
}
