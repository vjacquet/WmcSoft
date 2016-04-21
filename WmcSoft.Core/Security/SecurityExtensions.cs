#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace WmcSoft.Security
{
    public static class SecurityExtensions
    {
        static bool InvalidIdentity(PrincipalPermissionAttribute attribute, IIdentity identity) {
            return String.IsNullOrWhiteSpace(attribute.Name) || String.Equals(attribute.Name, identity.Name);
        }

        static bool InvalidRole(PrincipalPermissionAttribute attribute, IPrincipal principal) {
            return principal.IsInRole(attribute.Role);
        }

        public static bool IsGranted(this MemberInfo self, IPrincipal principal) {
            try {
#pragma warning disable CS0642 // Possible mistaken empty statement

                var identity = principal.Identity;
                var permissions = new List<IPermission>();
                foreach (var attribute in self.GetCustomAttributes<SecurityAttribute>(true)) {
                    // special case to avoid exception in trivial cases
                    var principalPermissionAttribute = attribute as PrincipalPermissionAttribute;
                    if (principalPermissionAttribute == null)
                        ;
                    else if (principalPermissionAttribute.Authenticated && !identity.IsAuthenticated)
                        return false;
                    else if (InvalidIdentity(principalPermissionAttribute, identity) || InvalidRole(principalPermissionAttribute, principal))
                        return false;

                    permissions.Add(attribute.CreatePermission());
                }

                using (principal.Impersonate()) {
                    permissions.ForEach(p => p.Demand());
                }

                return true;

#pragma warning restore CS0642 // Possible mistaken empty statement
            }
            catch (SecurityException) {
                return false;
            }
        }

        public static bool IsGranted(this MemberInfo self) {
            return IsGranted(self, Thread.CurrentPrincipal);
        }

        public static bool IsGranted(this IPrincipal principal, MemberInfo self) {
            return IsGranted(self, principal);
        }

        public static IDisposable Impersonate(this IPrincipal principal) {
            var current = Thread.CurrentPrincipal;
            Thread.CurrentPrincipal = principal;
            return new Disposer(() => Thread.CurrentPrincipal = current);
        }
    }
}
