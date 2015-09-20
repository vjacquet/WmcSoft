﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Business.PartyModel;
using WmcSoft.Business.PartyModel.InMemory;

namespace WmcSoft.Business.PartyModel
{
    [TestClass]
    public class PartyStoreTests
    {
        [TestMethod]
        public void CanAddParty() {
            var mgr = new MyPartyManager(new PartyStore<Party, Guid>());
            var person = new Person("Vincent JACQUET");
            var id = mgr.AddPartyAsync(person).Result;
            Assert.AreEqual(id, person.Id);
        }
    }

    class MyPartyManager : PartyManager<PartyStore<Party, Guid>, Party>
    {
        public MyPartyManager(PartyStore<Party, Guid> store) : base(store) {
        }
    }
}
