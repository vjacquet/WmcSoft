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
using System.ComponentModel;
using System.Linq;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Captures the semantics of the role played by a Party in a particular
    /// PartyRelationship.
    /// </summary>
    public abstract class PartyRole
    {
        #region Comparers

        class UniqueIdentifierEqualityComparer : IEqualityComparer<PartyRole>
        {

            public UniqueIdentifierEqualityComparer() {
            }

            #region IEqualityComparer<PartyRole> Membres

            bool IEqualityComparer<PartyRole>.Equals(PartyRole x, PartyRole y) {
                if (x == null)
                    return y == null;
                if (x == null)
                    return y == null;
                return x.Identifier.Id.Equals(y.Identifier.Id);
            }

            int IEqualityComparer<PartyRole>.GetHashCode(PartyRole obj) {
                if (obj == null)
                    return 0;
                return obj.Identifier.Id.GetHashCode();
            }

            #endregion
        }
        public readonly static IEqualityComparer<PartyRole> UniqueIdentifierComparer = new UniqueIdentifierEqualityComparer();

        #endregion

        #region Fields

        private readonly Party _party;
        private readonly PartyRoleIdentifier _identifier;
        internal readonly Collection<PartyRelationship> _relationships;
        //WeightedPreferenceCollection _preferences;

        #endregion

        #region Lifecycle

        protected PartyRole(Party party) {
            CheckConstraints(party);

            _party = party;
            _identifier = new PartyRoleIdentifier();
            _party.Roles.Add(this);

            _relationships = new Collection<PartyRelationship>();
        }

        private void CheckConstraints(Party party) {
            var constraints = GetAttributes<PartyRoleConstraintAttribute>(GetType()).GetEnumerator();
            if (constraints.MoveNext()) {
                // if I have constraints, at least one should fit.
                do {
                    if (constraints.Current.CanPlayRole(party))
                        return;
                }
                while (constraints.MoveNext());

                throw new InvalidOperationException();
            }
            // no constraints, so ok
        }

        #endregion

        #region Properties

        public abstract string Name { get; }

        public abstract string Description { get; }

        public Party Party {
            get { return _party; }
        }

        public PartyRoleIdentifier Identifier {
            get { return _identifier; }
        }

        public ICollection<PartyRelationship> Relationships {
            get {
                return _relationships;
            }
        }

        //public WeightedPreferenceCollection Preferences {
        //    get {
        //        if (_preferences == null) {
        //            lock (_identifier) {
        //                if (_preferences == null) {
        //                    _preferences = new WeightedPreferenceCollection();
        //                }
        //            }
        //        }
        //        return _preferences;
        //    }
        //}

        #endregion

        #region Management of responsabilities

        public static IEnumerable<Responsability> GetMandatoryResponsabilitiesOf<R>() where R : PartyRole {
            return GetAttributes<ResponsabilityAttribute>(typeof(R))
                .Where(a => a.Policy == RegistrationPolicy.Mandatory)
                .Select(a => a.Responsability);
        }

        public static IEnumerable<Responsability> GetOptionalResponsabilitiesOf<R>() where R : PartyRole {
            return GetAttributes<ResponsabilityAttribute>(typeof(R))
                .Where(a => a.Policy == RegistrationPolicy.Optional)
                .Select(a => a.Responsability);
        }

        #endregion

        #region Management of contraints

        public static IEnumerable<PartyRoleConstraintAttribute> GetConstraintsOf<R>() where R : PartyRole {
            return GetAttributes<PartyRoleConstraintAttribute>(typeof(R));
        }

        #endregion

        #region Helpers

        protected static IEnumerable<A> GetAttributes<A>(Type type) where A : Attribute {
            return TypeDescriptor.GetAttributes(type).OfType<A>();
        }

        #endregion
    }
}
