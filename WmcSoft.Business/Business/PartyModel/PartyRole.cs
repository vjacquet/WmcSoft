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
using System.Linq;
using WmcSoft.Business.RuleModel;

using TKey = System.Guid;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Captures the semantics of the role played by a <see cref="Party"/> in a particular
    /// <see cref="PartyRelationship"/>.
    /// </summary>
    public abstract class PartyRole : DomainObject<TKey>
    {
        #region Fields

        private readonly List<AssignedResponsability> _assignedResponsabilities;

        #endregion

        #region Lifecycle

        protected PartyRole(Party party) {
            CheckConstraints(party);

            Party = party;
            party.Roles.Add(this);

            Relationships = new List<PartyRelationship>();
            _assignedResponsabilities = new List<AssignedResponsability>();
        }

        private void CheckConstraints(Party party) {
            var constraints = GetType().GetAttributes<PartyRoleConstraintAttribute>();
            using (var enumerator = constraints.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    // if I have constraints, at least one should fit.
                    do {
                        if (enumerator.Current.CanPlayRole(party))
                            return;
                    }
                    while (enumerator.MoveNext());

                    throw new InvalidOperationException();
                }
                // no constraints, so ok
            }
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

        public virtual Party Party { get; private set; }
        public virtual ICollection<PartyRelationship> Relationships { get; internal set; }

        public virtual RuleSet RequirementsForRole {
            get { return RuleSet.Empty; }
        }

        public ICollection<AssignedResponsability> AssignedResponsabilities {
            get {
                return _assignedResponsabilities;
            }
        }

        #endregion

        #region Management of responsabilities

        public static IEnumerable<Responsability> GetMandatoryResponsabilitiesOf<R>() where R : PartyRole {
            return from a in typeof(R).GetAttributes<ResponsabilityAttribute>()
                   where a.Policy == RegistrationPolicy.Mandatory
                   select a.Responsability;
        }

        public static IEnumerable<Responsability> GetOptionalResponsabilitiesOf<R>() where R : PartyRole {
            return from a in typeof(R).GetAttributes<ResponsabilityAttribute>()
                   where a.Policy == RegistrationPolicy.Optional
                   select a.Responsability;
        }

        #endregion

        #region Management of contraints

        public static IEnumerable<PartyRoleConstraintAttribute> GetConstraintsOf<R>() where R : PartyRole {
            return typeof(R).GetAttributes<PartyRoleConstraintAttribute>();
        }

        #endregion
    }
}
