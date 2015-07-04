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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Security
{
    class AccessControlLists : IAccessControlLists
    {
        #region Fields

        private readonly HashSet<AccessControlEntry> _grant;
        private readonly HashSet<AccessControlEntry> _deny;

        #endregion

        #region Lifecycle

        public AccessControlLists() {
            _grant = new HashSet<AccessControlEntry>();
            _deny = new HashSet<AccessControlEntry>();
        }

        public AccessControlLists(IAccessControlLists other) {
            _grant = new HashSet<AccessControlEntry>(other.GrantedPermissions);
            _deny = new HashSet<AccessControlEntry>(other.DeniedPermissions);
        }

        #endregion

        #region IAccessControlList Membres

        public ISet<AccessControlEntry> GrantedPermissions {
            get { return _grant; }
        }

        public ISet<AccessControlEntry> DeniedPermissions {
            get { return _deny; }
        }

        public void Grant(IEnumerable<Permission> permissions, params Principal[] principals) {
            foreach (var p in permissions) {
                for (int i = 0; i < principals.Length; i++) {
                    var t = new AccessControlEntry(p, principals[i]);
                    _deny.Remove(t);
                    _grant.Add(t);
                }
            }
        }

        public void Deny(IEnumerable<Permission> permissions, params Principal[] principals) {
            foreach (var p in permissions) {
                for (int i = 0; i < principals.Length; i++) {
                    var t = new AccessControlEntry(p, principals[i]);
                    _grant.Remove(t);
                    _deny.Add(new AccessControlEntry(p, principals[i]));
                }
            }
        }

        public void Revoke(IEnumerable<Permission> permissions, params Principal[] principals) {
            foreach (var p in permissions) {
                for (int i = 0; i < principals.Length; i++) {
                    var t = new AccessControlEntry(p, principals[i]);
                    _grant.Remove(t);
                    _deny.Remove(t);
                }
            }
        }

        public bool Verify(Principal principal, Permission permission) {
            var acl = new HashSet<Permission>(_grant.Where(t => t.Match(principal)).Select(t => t.Permission));
            acl.ExceptWith(_deny.Where(t => t.Match(principal)).Select(t => t.Permission));
            return acl.Contains(permission);
        }

        public IEnumerable<Permission> Verify(Principal principal, IEnumerable<Permission> permissions) {
            var acl = new HashSet<Permission>(_grant.Where(t => t.Match(principal)).Select(t => t.Permission));
            acl.ExceptWith(_deny.Where(t => t.Match(principal)).Select(t => t.Permission));
            var result = acl.Intersect(permissions);
            return result;
        }

        #endregion
    }
}
