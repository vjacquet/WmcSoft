using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WmcSoft.Business.PartyModel.InMemory
{
    public class PartyStore<TParty, TKey> : IPartyStore<TParty>
          where TKey : IEquatable<TKey>
          where TParty : Party
    {
        readonly ConcurrentDictionary<TKey, TParty> _context = new ConcurrentDictionary<TKey, TParty>();
        readonly Func<PartyIdentifier> _uniqueIdGenerator;

        public PartyStore(Func<PartyIdentifier> uniqueIdGenerator)
        {
            _uniqueIdGenerator = uniqueIdGenerator;
        }

        public Task<PartyIdentifier> AddPartyAsync(TParty party, CancellationToken cancellationToken)
        {
            var id = _uniqueIdGenerator();
            var key = ConvertPartyIdentifierToKey(id);
            _context.TryAdd(key, party);
            party.Id = id;
            return Task.FromResult(id);
        }

        private TKey ConvertPartyIdentifierToKey(PartyIdentifier id)
        {
            return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id.ToString());
        }

        public Task<bool> DeletePartyAsync(PartyIdentifier partyId, CancellationToken cancellationToken)
        {
            var key = ConvertPartyIdentifierToKey(partyId);
            var result = _context.TryRemove(key, out TParty party);
            return Task.FromResult(result);
        }

        public Task<IEnumerable<TParty>> FindPartiesByName(string name, CancellationToken cancellationToken)
        {
            var result = new List<Party>();
            foreach (var kv in _context) {
                var p = kv.Value;
                if (string.Equals(name, p.Name, StringComparison.CurrentCultureIgnoreCase))
                    result.Add(p);
            }
            return Task.FromResult((IEnumerable<TParty>)result);
        }

        public Task<IEnumerable<TParty>> FindPartiesByRegisteredIdentifier(RegisteredIdentifier registeredId, CancellationToken cancellationToken)
        {
            var result = new List<Party>();
            foreach (var kv in _context) {
                var p = kv.Value;
                if (p.RegisteredIdentifiers.Any(i => i.Match(registeredId)))
                    result.Add(p);
            }
            return Task.FromResult((IEnumerable<TParty>)result);
        }

        public Task<TParty> GetPartyAsync(PartyIdentifier partyId, CancellationToken cancellationToken)
        {
            var key = ConvertPartyIdentifierToKey(partyId);
            if (!_context.TryGetValue(key, out TParty party))
                return Task.FromException<TParty>(new ArgumentOutOfRangeException("partyId"));
            return Task.FromResult(party);
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PartyStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}