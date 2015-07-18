using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Business.PartyModel
{
    [TestClass]
    public class ProjectLikePartyRoleTests
    {
        Person CreatePerson(string name) {
            return new Person {
                PersonName = new PersonName {
                    PreferredName = name
                }
            };
        }

        Company CreateCompany(string name) {
            return new Company {
                OrganizationName = new OrganizationName(name)
            };
        }

        [TestMethod]
        public void CanCreateComplexRelationship() {
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

            clearViewTraining.CollaborateOn(jointProject);
            interactiveObjects.CollaborateOn(jointProject);

            clearViewTraining.Employs(jim, new Director(jim));
            clearViewTraining.Employs(ila, new Director(ila));
            interactiveObjects.Employs(richard, new Director(richard));
            interactiveObjects.Employs(ronald, new Employee(ronald));
        }
    }
}
