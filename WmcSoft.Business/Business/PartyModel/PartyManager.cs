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
using System.Threading;
using System.Threading.Tasks;

namespace WmcSoft.Business.PartyModel
{
    public abstract class PartyManager
    {
        public abstract void AddParty(Party party);
        public abstract bool DeleteParty(PartyIdentifier partyId);
        public abstract Party GetParty(PartyIdentifier partyId);
        public abstract Party GetPartyByPartyRoleIdentifier(PartyRoleIdentifier partyRoleId);
        public abstract IEnumerable<Party> FindPartiesByName(string name);
        public abstract IEnumerable<Party> FindPartiesByRegisteredIdentifier(RegisteredIdentifier registeredId);
    }

#if FUTURE
    // TODO: should we try MicroSoft approach like in the UserManager ?

    public class PartyManager<TStore, TParty, TIdentifier>
        where TParty : class
        where TStore : IPartyStore<TParty, TIdentifier>
    {
        public Task AddPartyAsync(TParty party, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
        public abstract Task<bool> DeletePartyAsync(TIdentifier partyId, CancellationToken cancellationToken);
        public abstract Task<TParty> GetPartyAsync(TIdentifier partyId, CancellationToken cancellationToken);
        public abstract Task<TParty> GetPartyByPartyRoleIdentifierAsync(PartyRoleIdentifier partyRoleId, CancellationToken cancellationToken);
        public abstract Task<IEnumerable<TParty>> FindPartiesByName(string name, CancellationToken cancellationToken);
        public abstract Task<IEnumerable<TParty>> FindPartiesByRegisteredIdentifier(RegisteredIdentifier registeredId, CancellationToken cancellationToken);
    }

    public interface IPartyStore<TParty, TIdentifier> : IDisposable 
        where TParty : class
    {
    }

#endif
}
