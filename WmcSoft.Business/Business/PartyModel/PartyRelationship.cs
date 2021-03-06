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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WmcSoft.Business.RuleModel;

using TKey = System.Guid;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Captures the fact that there is a semantic relationship between two parties 
    /// in which each Party plays a specific role.
    /// </summary>
    [RM.DisplayName(nameof(PartyRelationship))]
    [RM.Description(nameof(PartyRelationship))]
    public abstract class PartyRelationship : DomainObject<TKey>
    {
        #region Fields

        private PartyRole _client;
        private PartyRole _supplier;

        #endregion

        #region Lifecycle

        protected PartyRelationship(PartyRole client, PartyRole supplier) {
            CheckConstraints(client, supplier);

            _client = client;
            _client.Relationships.Add(this);

            _supplier = supplier;
            _supplier.Relationships.Add(this);
        }

        private void CheckConstraints(PartyRole client, PartyRole supplier) {
            using (var constraints = GetConstraints(GetType()).GetEnumerator()) {
                if (constraints.MoveNext()) {
                    // if I have constraints, at least one should fit.
                    do {
                        if (constraints.Current.CanFormRelationship(client, supplier))
                            return;
                    }
                    while (constraints.MoveNext());

                    throw new InvalidOperationException();
                }
            }
            // no constraints, so ok
        }

        #endregion

        #region Properties

        public PartyRole Client {
            get {
                return _client;
            }
            set {
                if (_client != value) {
                    _client.Relationships.Remove(this);
                    _client = value;
                    _client.Relationships.Add(this);
                }
            }
        }
        public PartyRole Supplier {
            get {
                return _supplier;
            }
            set {
                if (_supplier != value) {
                    _supplier.Relationships.Remove(this);
                    _supplier = value;
                    _supplier.Relationships.Add(this);
                }
            }
        }

        public virtual RuleSet RequirementsForRole {
            get { return RuleSet.Empty; }
        }

        #endregion

        #region Management of contraints

        protected static IEnumerable<PartyRelationshipConstraintAttribute> GetConstraints(Type type) {
            return TypeDescriptor.GetAttributes(type).OfType<PartyRelationshipConstraintAttribute>();
        }

        public static IEnumerable<PartyRelationshipConstraintAttribute> GetConstraintsOf<R>() where R : PartyRelationship {
            return GetConstraints(typeof(R));
        }

        #endregion
    }
}
