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

using TKey = System.Guid;

namespace WmcSoft.Business.PartyModel
{

    /// <summary>
    /// Represents a unique identifier for a PartyRole.
    /// </summary>
    public struct PartyRoleIdentifier : IUniqueIdentifier<TKey>
    {
        #region Fields

        readonly TKey _id;

        #endregion

        #region Lifecycle

        public PartyRoleIdentifier(TKey id) {
            _id = id;
        }

        #endregion

        #region Operators & conversions 

        public static explicit operator PartyRoleIdentifier(PartyRole partyRole) {
            return partyRole.Id;
        }

        public static implicit operator PartyRoleIdentifier(TKey id) {
            return new PartyRoleIdentifier(id);
        }

        public static implicit operator TKey(PartyRoleIdentifier id) {
            return id.Id;
        }

        #endregion

        #region IUniqueIdentifier<string> Membres

        public TKey Id {
            get { return _id; }
        }

        #endregion

        #region IEquatable<Guid> Members

        public bool Equals(Guid other) {
            return _id == other;
        }

        #endregion

        #region Overidables

        public override string ToString() {
            return _id.ToString();
        }

        public override int GetHashCode() {
            return _id.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(PartyRoleIdentifier))
                return false;
            return base.Equals((PartyRoleIdentifier)obj);
        }

        #endregion
    }
}