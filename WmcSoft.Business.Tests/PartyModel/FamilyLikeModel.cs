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


namespace WmcSoft.Business.PartyModel
{
    public class Spouse : PartyRole
    {
        public Spouse(Party party)
            : base(party) {
        }

        public override string Name {
            get { return "Spouse"; }
        }
        public override string Description {
            get { return "A partner in a marriage"; }
        }
    }

    [FemaleConstraint]
    public class Wife : Spouse
    {
        public Wife(Party party)
            : base(party) {
        }

        public override string Name {
            get { return "Wife"; }
        }
        public override string Description {
            get { return "A female partner in a marriage"; }
        }
    }


    [MaleConstraint]
    public class Husband : Spouse
    {
        public Husband(Party party)
            : base(party) {
        }

        public override string Name {
            get { return "Husband"; }
        }
        public override string Description {
            get { return "A male partner in a marriage"; }
        }
    }

    public class MaleConstraintAttribute : PartyRoleConstraintAttribute
    {
        public MaleConstraintAttribute()
            : base(typeof(Person)) {

        }
        public override bool CanPlayRole(Party party) {
            if (!base.CanPlayRole(party))
                return false;
            var person = (Person)party;
            return person.Gender == Gender.Male;
        }
    }

    public class FemaleConstraintAttribute : PartyRoleConstraintAttribute
    {
        public FemaleConstraintAttribute()
            : base(typeof(Person)) {

        }
        public override bool CanPlayRole(Party party) {
            if (!base.CanPlayRole(party))
                return false;
            var person = (Person)party;
            return person.Gender == Gender.Female;
        }
    }

    [PartyRelationshipConstraint(typeof(Husband), typeof(Wife))]
    public class TraditionalMarriage : PartyRelationship
    {
        public TraditionalMarriage(PartyRole client, PartyRole supplier)
            : base(client, supplier) {
        }

        public override string Name {
            get { return "Marriage"; }
        }
        public override string Description {
            get { return "The state of being married"; }
        }

        public Husband Husband { get { return (Husband)this.Client; } }
        public Wife Wife { get { return (Wife)this.Supplier; } }
    }
}
