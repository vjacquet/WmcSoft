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

using System.DirectoryServices;
using System.Security.Principal;

namespace WmcSoft.DirectoryServices
{
    public static class DirectoryServicesExtensions
    {
        #region Flags manipulation

        public static void ResetFlagValue(this PropertyValueCollection self, int flags) {
            var value = (int)self.Value;
            self.Value = value & ~flags;
        }
        public static void SetFlagValue(this PropertyValueCollection self, int flags) {
            var value = (int)self.Value;
            self.Value = value | flags;
        }
        public static bool HasFlagValue(this PropertyValueCollection self, int flags) {
            var value = (int)self.Value;
            return (value & flags) == flags;
        }

        #endregion

        #region GetNTAccount

        public static string GetNTAccount(this DirectoryEntry member) {
            var collection = member.Properties;
            var objectSid = collection["objectSid"];
            var value = objectSid.Value;
            var identifier = new SecurityIdentifier((byte[])value, 0);
            return identifier.Translate(typeof(NTAccount)).ToString();
        }

        #endregion
    }
}
