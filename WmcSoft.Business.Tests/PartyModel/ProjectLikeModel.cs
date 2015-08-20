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
    public class Division : OrganizationUnit
    {
    }

    public class Department : OrganizationUnit
    {
    }

    [PartyRoleConstraint(typeof(Project))]
    public class JointProject : SimplePartyRole
    {
        public JointProject(Party party)
            : base(party) {
        }
    }

    [PartyRoleConstraint(typeof(Project))]
    public class EmployingProject : SimplePartyRole
    {
        public EmployingProject(Party party)
            : base(party) {
        }
    }

    public class Collaborator : SimplePartyRole
    {
        public Collaborator(Party party)
            : base(party) {
        }
    }

    public class Employer : SimplePartyRole
    {
        public Employer(Party party)
            : base(party) {
        }
    }

    [PartyRoleConstraint(typeof(Person))]
    public class Employee : SimplePartyRole
    {
        public Employee(Party party)
            : base(party) {
        }
    }

    public class Director : Employee
    {
        public Director(Party party)
            : base(party) {
        }
    }

    public class Project : Organization
    {
        public Project(string name) {
            OrganizationName = new OrganizationName(name);
        }
    }

    [PartyRelationshipConstraint(typeof(Employee), typeof(Employer))]
    public class Employment : SimplePartyRelationship
    {
        public Employment(Employee client, Employer supplier)
            : base(client, supplier) {
        }

        public Employee Employee { get { return (Employee)this.Client; } }
        public Employer Employer { get { return (Employer)this.Supplier; } }
    }

    [PartyRelationshipConstraint(typeof(Collaborator), typeof(JointProject))]
    public class Collaboration : SimplePartyRelationship
    {
        public Collaboration(Collaborator client, JointProject supplier)
            : base(client, supplier) {
        }

        public Collaborator Collaborator { get { return (Collaborator)this.Client; } }
        public JointProject JointProject { get { return (JointProject)this.Supplier; } }
    }

    [PartyRoleConstraint(typeof(Person))]
    public class Subordinate : SimplePartyRole
    {
        public Subordinate(Party party)
            : base(party) {
        }
    }

    [PartyRoleConstraint(typeof(Person))]
    public class Supervisor : SimplePartyRole
    {
        public Supervisor(Party party)
            : base(party) {
        }
    }

    [PartyRelationshipConstraint(typeof(Supervisor), typeof(Subordinate))]
    public class ReportsTo : SimplePartyRelationship
    {
        public ReportsTo(Supervisor client, Subordinate supplier)
            : base(client, supplier) {
        }

        public Supervisor Supervisor { get { return (Supervisor)this.Client; } }
        public Subordinate Subordinate { get { return (Subordinate)this.Supplier; } }
    }

    [PartyRelationshipConstraint(typeof(EmployingProject), typeof(RoleInProject))]
    public class MemberOfProject : SimplePartyRelationship
    {
        public MemberOfProject(PartyRole client, PartyRole supplier)
            : base(client, supplier) {
        }

        public EmployingProject EmployingProject { get { return (EmployingProject)this.Client; } }
        public RoleInProject Subordinate { get { return (RoleInProject)this.Supplier; } }
    }

    public abstract class RoleInProject : SimplePartyRole
    {
        public RoleInProject(Party party)
            : base(party) {
        }
    }

    public class Developer : RoleInProject
    {
        public Developer(Party party)
            : base(party) {
        }
    }

    public class ProjectManager : RoleInProject
    {
        public ProjectManager(Party party)
            : base(party) {
        }
    }

    public class Architect : RoleInProject
    {
        public Architect(Party party)
            : base(party) {
        }
    }

    public class TechnicalConsultant : RoleInProject
    {
        public TechnicalConsultant(Party party)
            : base(party) {
        }
    }
}
