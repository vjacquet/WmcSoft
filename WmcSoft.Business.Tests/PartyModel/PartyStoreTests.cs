using System;
using Xunit;
using WmcSoft.Business.PartyModel.InMemory;

namespace WmcSoft.Business.PartyModel
{
    public class PartyStoreTests
    {
        [Fact]
        public void CanAddParty()
        {
            var store = new PartyStore<Party, Guid>(() => Guid.NewGuid());
            var mgr = new MyPartyManager(store);
            var person = new Person("Vincent JACQUET");
            var id = (Guid)mgr.AddPartyAsync(person).Result;
            Assert.Equal(id, person.Id);
        }
    }

    class MyPartyManager : PartyManager<PartyStore<Party, Guid>, Party>
    {
        public MyPartyManager(PartyStore<Party, Guid> store) : base(store)
        {
        }
    }
}