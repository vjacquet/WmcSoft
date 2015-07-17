using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Business.PartyModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlowGroup.Business.Tests.PartyModel
{
    [TestClass]
    public class PartyTests
    {
        [TestMethod]
        public void CanCreateCompany() {
            var company = new Company("MyLittleCompany");

            Assert.IsInstanceOfType(company, typeof(Party));
            Assert.IsNotNull(company.OrganizationName);
            Assert.AreEqual(OrganizationalNameUse.LegalName, company.OrganizationName.Use);
            Assert.AreEqual("MyLittleCompany", company.OrganizationName.ToString());
        }
    }
}
