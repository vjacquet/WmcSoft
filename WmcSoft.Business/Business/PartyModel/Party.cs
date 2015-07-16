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
using System.Collections.ObjectModel;
using System.Security.Principal;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Represents an identifiable, addressable unit that have a legal status
    /// and that normally has autonomous control over (at least some of) its
    /// actions.
    /// </summary>
    public abstract class Party
    {
        #region Fields

        readonly PartyIdentifier _identifier;
        readonly List<AddressProperties> _addresses;
        readonly List<RegisteredIdentifier> _registeredIdentifiers;
        readonly List<PartyAuthentication> _partyAuthentications;
        readonly HashSet<PartyRole> _roles;
        WeightedPreferenceCollection _preferences;

        #endregion

        #region Lifecycle

        protected Party()
            : this(new PartyIdentifier()) {
        }

        protected Party(PartyIdentifier identifier) {
            _addresses = new List<AddressProperties>();
            _registeredIdentifiers = new List<RegisteredIdentifier>();
            _partyAuthentications = new List<PartyAuthentication>();
            _identifier = identifier;
            _roles = new HashSet<PartyRole>(PartyRole.UniqueIdentifierComparer);
        }

        #endregion

        public PartyIdentifier Identifier {
            get { return _identifier; }
        }

        public ICollection<RegisteredIdentifier> RegisteredIdentifiers {
            get { return _registeredIdentifiers; }
        }

        public ICollection<PartyAuthentication> PartyAuthentications {
            get { return _partyAuthentications; }
        }

        public virtual string Name {
            get {
                // crawl the hierarchy of types to find a name
                var type = GetType();
                while (type != typeof(Party)) {
                    var name = RM.GetName(type);
                    if (!String.IsNullOrEmpty(name))
                        return name;
                }
                return null;
            }
        }

        public virtual string Description {
            get {
                var type = GetType();
                while (type != typeof(Party)) {
                    // crawl the hierarchy of types to find a name
                    // then get the associated description
                    var name = RM.GetName(type);
                    if (!String.IsNullOrEmpty(name))
                        return RM.GetDescription(type);
                }
                return null;
            }
        }

        public ICollection<AddressProperties> Addresses {
            get { return _addresses; }
        }

        public ICollection<PartyRole> Roles {
            get { return _roles; }
        }

        public WeightedPreferenceCollection Preferences {
            get {
                if (_preferences == null) {
                    lock (_identifier) {
                        if (_preferences == null) {
                            _preferences = new WeightedPreferenceCollection();
                        }
                    }
                }
                return _preferences;
            }
        }
    }
}
