using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Business.PartyModel
{
    public static class BusinessExtensions
    {
        public static R AddRole<R>(this Party party) where R : PartyRole {
            R role = (R)Activator.CreateInstance(typeof(R), party);
            return role;
        }

        public static R EnsureRole<R>(this Party party) where R : PartyRole {
            R role = party.Roles.OfType<R>().FirstOrDefault();
            if (role == null)
                role = party.AddRole<R>();
            return role;
        }

        public static Collaboration CollaborateOn(this Organization organization, JointProject project) {
            var collaborator = new Collaborator(organization);
            var relationship = new Collaboration(collaborator, project);
            return relationship;
        }

        public static Employment Employs(this Organization organization, Person person, Employee role) {
            var employer = organization.EnsureRole<Employer>();
            var relationship = new Employment(role, employer);
            return relationship;
        }

        public static ReportsTo ReportsTo(this Person subordinate, Person supervisor) {
            return new ReportsTo(supervisor.EnsureRole<Supervisor>(), subordinate.EnsureRole<Subordinate>());
        }

        public static MemberOfProject WorksAs<R>(this Person person, EmployingProject project) where R : RoleInProject {
            return new MemberOfProject(project, person.EnsureRole<R>());
        }
    }
}
