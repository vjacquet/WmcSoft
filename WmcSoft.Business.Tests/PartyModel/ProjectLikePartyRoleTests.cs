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

using Xunit;

namespace WmcSoft.Business.PartyModel
{
    public class ProjectLikePartyRoleTests
    {
        Person CreatePerson(string name)
        {
            return new Person {
                PersonName = new PersonName {
                    PreferredName = name
                }
            };
        }

        Company CreateCompany(string name)
        {
            return new Company {
                OrganizationName = new OrganizationName(name)
            };
        }

        [Fact]
        public void CanCreateComplexRelationship()
        {
            var jim = CreatePerson("jim");
            var ila = CreatePerson("ila");
            var richard = CreatePerson("richard");
            var ronald = CreatePerson("donald");

            ila.ReportsTo(jim);
            ronald.ReportsTo(jim);
            ronald.ReportsTo(richard);

            var project = new Project("archetype");
            var employingProject = new EmployingProject(project);
            jim.WorksAs<ProjectManager>(employingProject);
            jim.WorksAs<Architect>(employingProject);
            ila.WorksAs<Developer>(employingProject);
            ronald.WorksAs<TechnicalConsultant>(employingProject);

            var jointProject = new JointProject(project);

            var clearViewTraining = CreateCompany("clearViewTraining");
            var interactiveObjects = CreateCompany("interactiveObjects");

            Assert.True(clearViewTraining.CanCollaborateOn(jointProject));
            Assert.False(jim.CanCollaborateOn(jointProject));
            Assert.True(interactiveObjects.CanCollaborateOn(jointProject));

            clearViewTraining.CollaborateOn(jointProject);
            interactiveObjects.CollaborateOn(jointProject);

            clearViewTraining.Employs(jim, new Director(jim));
            clearViewTraining.Employs(ila, new Director(ila));
            interactiveObjects.Employs(richard, new Director(richard));
            interactiveObjects.Employs(ronald, new Employee(ronald));

            Assert.True(jim.HasRole<ProjectManager>());
            Assert.True(jim.HasRole<Employee>());
            Assert.True(jim.HasRole<Director>());
            Assert.False(ronald.HasRole<Director>());

            Assert.True(jim.HasRole<ProjectManager>(project));
        }
    }
}