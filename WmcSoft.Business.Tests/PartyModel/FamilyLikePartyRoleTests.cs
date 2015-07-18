using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Business.PartyModel
{
    [TestClass]
    public class FamilyLikePartyRoleTests
    {
        Person CreatePerson(string name) {
            return new Person {
                PersonName = new PersonName {
                    PreferredName = name
                }
            };
        }

        [TestMethod]
        public void CanMarryMaleAndFemale() {
            var naomi = CreatePerson("naomi");
            naomi.Gender = Gender.Female;
            var richard = CreatePerson("richard");
            richard.Gender = Gender.Male;

            var marriage = new TraditionalMarriage(richard.AddRole<Husband>(), naomi.AddRole<Wife>());
            Assert.AreEqual(richard, marriage.Client.Party);
            Assert.AreEqual(naomi, marriage.Supplier.Party);
        }
    }
}
