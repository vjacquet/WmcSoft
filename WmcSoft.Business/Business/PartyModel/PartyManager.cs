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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WmcSoft.Business.PartyModel
{
    public class PartyManager<TStore, TParty>
        where TParty : class
        where TStore : IPartyStore<TParty>
    {
        public PartyManager(TStore store) {
            Store = store;
        }

        internal TStore Store { get; }

        public Task<PartyIdentifier> AddPartyAsync(TParty party, CancellationToken cancellationToken = default(CancellationToken)) {
            return Store.AddPartyAsync(party, cancellationToken);
        }
        public Task<bool> DeletePartyAsync(PartyIdentifier partyId, CancellationToken cancellationToken = default(CancellationToken)) {
            return Store.DeletePartyAsync(partyId, cancellationToken);
        }
        public Task<TParty> GetPartyAsync(PartyIdentifier partyId, CancellationToken cancellationToken = default(CancellationToken)) {
            return Store.GetPartyAsync(partyId, cancellationToken);
        }
        public Task<IEnumerable<TParty>> FindPartiesByName(string name, CancellationToken cancellationToken = default(CancellationToken)) {
            return Store.FindPartiesByName(name, cancellationToken);
        }
        public Task<IEnumerable<TParty>> FindPartiesByRegisteredIdentifier(RegisteredIdentifier registeredId, CancellationToken cancellationToken = default(CancellationToken)) {
            return Store.FindPartiesByRegisteredIdentifier(registeredId, cancellationToken);
        }
    }

    public interface IPartyStore<TParty> : IDisposable
        where TParty : class
    {
        Task<PartyIdentifier> AddPartyAsync(TParty party, CancellationToken cancellationToken);
        Task<bool> DeletePartyAsync(PartyIdentifier partyId, CancellationToken cancellationToken);
        Task<TParty> GetPartyAsync(PartyIdentifier partyId, CancellationToken cancellationToken);
        Task<IEnumerable<TParty>> FindPartiesByName(string name, CancellationToken cancellationToken);
        Task<IEnumerable<TParty>> FindPartiesByRegisteredIdentifier(RegisteredIdentifier registeredId, CancellationToken cancellationToken);
    }

    public interface IPartyRoleStore<TParty> : IPartyStore<TParty>
        where TParty : class
    {
        Task<TParty> GetPartyByPartyRoleIdentifierAsync(PartyRoleIdentifier partyRoleId, CancellationToken cancellationToken);
    }

    // TODO: should have one for party roles too.
    public interface IPartyPreferenceStore<TParty> : IPartyStore<TParty>
        where TParty : class
    {
        Task<WeightedPreferenceCollection> GetPartyPreferencesAsync(PartyIdentifier partyId, CancellationToken cancellationToken);
    }

    public interface IPartyAuthenticationStore<TParty> : IPartyStore<TParty>
        where TParty : class
    {
        Task<List<PartyAuthentication>> GetPartyAuthenticationsAsync(PartyIdentifier partyId, CancellationToken cancellationToken);
    }

    public interface IQueryablePartyStore<TParty> : IPartyStore<TParty>
        where TParty : class
    {
        IQueryable<TParty> QueryParties();
    }

    public static class PartyManagerExtensions
    {
        public static Task<TParty> GetPartyByPartyRoleIdentifierAsync<TStore, TParty>(this PartyManager<TStore, TParty> partyManager, PartyRoleIdentifier partyRoleId, CancellationToken cancellationToken = default(CancellationToken))
            where TParty : class
            where TStore : IPartyRoleStore<TParty> {
            return partyManager.Store.GetPartyByPartyRoleIdentifierAsync(partyRoleId, cancellationToken);
        }
    }
}
