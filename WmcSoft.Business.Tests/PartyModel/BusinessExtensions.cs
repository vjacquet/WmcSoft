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
    public static class BusinessExtensions
    {
        public static bool CanCollaborateOn(this Party party, PartyRole role) {
            return party is Organization && role is JointProject;
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
