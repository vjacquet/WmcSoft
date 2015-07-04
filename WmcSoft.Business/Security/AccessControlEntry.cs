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
using System.Diagnostics;

namespace WmcSoft.Security
{
    /// <summary>
    /// Represents an entry in a grant or deny access control list.
    /// </summary>
    [DebuggerDisplay("{_permission.Name,nq} - {_principal.Name,nq}")]
    public class AccessControlEntry : IEquatable<AccessControlEntry>
    {
        #region Fields

        private readonly Permission _permission;
        private readonly Principal _principal;

        #endregion

        #region Lifecycle

        public AccessControlEntry(Permission permission, Principal principal) {
            if (permission == null)
                throw new ArgumentNullException("permission");
            if (principal == null)
                throw new ArgumentNullException("principal");

            _permission = permission;
            _principal = principal;
        }

        #endregion

        #region Properties

        public Permission Permission {
            get { return _permission; }
        }

        public Principal Principal {
            get { return _principal; }
        }

        #endregion

        #region Methods

        public bool Match(Principal principal) {
            return _principal.Match(principal);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj) {
            return Equals(obj as AccessControlEntry);
        }

        public override int GetHashCode() {
            return _permission.GetHashCode() * 397 ^ _principal.GetHashCode();
        }

        #endregion

        #region IEquatable<AccessControlEntry> Membres

        public bool Equals(AccessControlEntry other) {
            if (other == null)
                return false;
            return _principal.Equals(other._principal)
                && _permission.Equals(other._permission);
        }

        #endregion
    }
}
