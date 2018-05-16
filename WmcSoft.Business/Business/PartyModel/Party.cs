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

using TKey = System.Guid;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Represents an identifiable, addressable unit that have a legal status
    /// and that normally has autonomous control over (at least some of) its
    /// actions.
    /// </summary>
    public abstract class Party : DomainObject<TKey>, IFormattable
    {
        #region Fields

        private readonly List<AddressProperties> _addressProperties;

        #endregion

        #region Lifecycle

        protected Party()
        {
            _addressProperties = new List<AddressProperties>();
            Addresses = ManyToMany<Party, AddressBase>.Adapt(this, _addressProperties);
            RegisteredIdentifiers = new HashSet<RegisteredIdentifier>();
            Roles = new HashSet<PartyRole>(UniqueIdentifierComparer);
        }

        protected Party(TKey identifier) : this()
        {
            Id = identifier;
        }

        #endregion

        #region Traits

        public virtual string Name {
            get { return RM.GetInheritedName(GetType()); }
        }

        public virtual string Description {
            get { return RM.GetInheritedDescription(GetType()); }
        }

        #endregion

        #region Properties

        public ICollection<RegisteredIdentifier> RegisteredIdentifiers { get; }

        public ICollection<AddressProperties> AddressProperties { get { return _addressProperties; } }
        public ICollection<AddressBase> Addresses { get; }

        public ICollection<PartyRole> Roles { get; }

        #endregion

        #region IFormattable members

        public abstract string ToString(string format, IFormatProvider formatProvider);

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public sealed override string ToString()
        {
            return ToString(null, null) ?? base.ToString();
        }

        #endregion
    }
}
