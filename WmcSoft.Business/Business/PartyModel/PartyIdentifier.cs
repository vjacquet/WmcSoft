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
    /// Represents a unique identifier for a Party.
    /// </summary>
    [Serializable]
    public class PartyIdentifier : IUniqueIdentifier<TKey>
    {
        #region Fields

        readonly TKey _id;

        #endregion

        #region Lifecycle

        public PartyIdentifier(TKey id) {
            _id = id;
        }

        #endregion

        #region Operators & conversions 

        public static explicit operator PartyIdentifier(Party party) {
            return party.Id;
        }

        public static implicit operator PartyIdentifier(TKey id) {
            return new PartyIdentifier(id);
        }

        public static implicit operator TKey(PartyIdentifier id) {
            return id.Id;
        }

        #endregion

        #region Membres de IUniqueIdentifier

        public TKey Id {
            get { return _id; }
        }

        #endregion

        #region IEquatable<TKey> Members

        public bool Equals(TKey other) {
            return Id.Equals(other);
        }

        #endregion

        #region Overidables

        public override string ToString() {
            return _id.ToString();
        }

        #endregion
    }
}
