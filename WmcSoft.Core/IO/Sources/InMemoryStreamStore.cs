using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WmcSoft.IO.Sources
{
    public class InMemoryStreamStore : StreamStore
    {
        class InMemoryStorageEntry : StorageEntry
        {
            private readonly byte[] _data;

            public InMemoryStorageEntry(byte[] data, string name, int length, byte[] hash, DateTime since, DateTime? until = null)
                : base(name, length, hash, since, until)
            {
                _data = data;
            }

            public override Stream GetStream()
            {
                return new MemoryStream(_data, false);
            }

            public void Close(DateTime until)
            {
                CloseEntry(until);
            }
        }

        class ValidSinceUtcComparer : IComparer<InMemoryStorageEntry>
        {
            public static readonly ValidSinceUtcComparer Default = new ValidSinceUtcComparer();

            public int Compare(InMemoryStorageEntry x, InMemoryStorageEntry y)
            {
                return x.ValidSinceUtc.CompareTo(y.ValidSinceUtc);
            }
        }

        class InMemoryStorageEntryFinder : InMemoryStorageEntry
        {
            public InMemoryStorageEntryFinder(DateTime since) : base(null, null, 0, null, since, null)
            {
            }
        }

        private readonly Dictionary<string, List<InMemoryStorageEntry>> _store = new Dictionary<string, List<InMemoryStorageEntry>>();

        public InMemoryStreamStore(IDateTimeSource source) : base(source)
        {
        }

        public override IEnumerable<StorageEntry> GetHistory(string name)
        {
            List<InMemoryStorageEntry> entries;
            if (_store.TryGetValue(name, out entries))
                return UnguardedBackwards(entries);
            return Enumerable.Empty<StorageEntry>();
        }

        private static IEnumerable<StorageEntry> UnguardedBackwards(List<InMemoryStorageEntry> entries)
        {
            for (int i = entries.Count - 1; i >= 0; i--) {
                yield return entries[i];
            }
        }

        public override IEnumerable<StorageEntry> GetInventory(DateTime asOfUtc)
        {
            var finder = new InMemoryStorageEntryFinder(asOfUtc);
            foreach (var entries in _store.Values) {
                var found = entries.BinarySearch(finder, ValidSinceUtcComparer.Default);
                if (found >= 0)
                    yield return entries[found];
                else if (found != -1)
                    yield return entries[~found - 1];
            }
        }

        public override StorageEntry Find(string name, DateTime asOfUtc)
        {
            if (_store.TryGetValue(name, out var entries)) {
                var finder = new InMemoryStorageEntryFinder(asOfUtc);
                var found = entries.BinarySearch(finder, ValidSinceUtcComparer.Default);
                if (found >= 0)
                    return entries[found];
                else if (found != -1)
                    return entries[~found - 1];
            }
            return null;
        }

        private List<InMemoryStorageEntry> EnsureStorage(string name)
        {
            if (!_store.TryGetValue(name, out var entries)) {
                entries = new List<InMemoryStorageEntry>();
                _store.Add(name, entries);
            }
            return entries;
        }

        protected override void Store(StorageEntry latest, StorageEntry metadata, Stream stream)
        {
            var name = metadata.Name;
            var entries = EnsureStorage(name);
            var version = entries.Count;
            var data = ReadAll(stream);
            var entry = new InMemoryStorageEntry(data, name, metadata.Length, metadata.Hash, metadata.ValidSinceUtc);
            entries.Add(entry);
            if (version > 0) {
                entries[version - 1].Close(metadata.ValidSinceUtc);
            }
        }
    }
}
