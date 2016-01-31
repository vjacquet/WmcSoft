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
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace WmcSoft.Security
{
    public static class SecurityExtensions
    {
        public static bool IsGranted(this MemberInfo self) {
            try {
                var principal = Thread.CurrentPrincipal;
                foreach (var attribute in self.GetCustomAttributes<SecurityAttribute>(true)) {
                    // special case to avoid exception in trivial cases
                    var principalPermissionAttribute = attribute as PrincipalPermissionAttribute;
                    if (principalPermissionAttribute != null) {
                        if ((principalPermissionAttribute.Authenticated && !principal.Identity.IsAuthenticated)
                            || (!String.IsNullOrWhiteSpace(principalPermissionAttribute.Name) && !String.Equals(principalPermissionAttribute.Name, principal.Identity.Name))
                            || ((!String.IsNullOrWhiteSpace(principalPermissionAttribute.Role) && !principal.IsInRole(principalPermissionAttribute.Role)))) {
                            return false;
                        }
                    }
                    attribute.CreatePermission().Demand();
                }
                return true;
            }
            catch (SecurityException) {
                return false;
            }
        }
    }
}
