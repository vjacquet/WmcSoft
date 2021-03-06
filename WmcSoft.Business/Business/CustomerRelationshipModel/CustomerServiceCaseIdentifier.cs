﻿#region Licence

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

namespace WmcSoft.Business.CustomerRelationshipModel
{
    /// <summary>
    /// Represents a unique identifier for a customer service case.
    /// </summary>
    [Serializable]
    public sealed class CustomerServiceCaseIdentifier : IUniqueIdentifier<Guid>
    {
        #region Fields

        private readonly Guid _id;

        #endregion

        #region Lifecycle

        public CustomerServiceCaseIdentifier()
            : this(Guid.NewGuid()) {
        }

        public CustomerServiceCaseIdentifier(Guid identifier) {
            _id = identifier;
        }

        #endregion

        #region Membres de IUniqueIdentifier

        public Guid Id
        {
            get { return _id; }
        }

        #endregion

        #region IEquatable<Guid> Members

        public bool Equals(Guid other) {
            return _id == other;
        }

        public override int GetHashCode() {
            return _id.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(CustomerServiceCaseIdentifier))
                return false;
            return base.Equals((CustomerServiceCaseIdentifier)obj);
        }

        #endregion
    }
}
