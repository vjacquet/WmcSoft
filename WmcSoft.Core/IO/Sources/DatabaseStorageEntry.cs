using System;
using System.IO;

namespace DevFi.Tools.IO.Sources
{
    [Obsolete("Use SqlConnectionStreamStore instead.", false)]
    public class DatabaseStorageEntry : StorageEntry
    {
        public byte[] RawStorage { get; }

        public DatabaseStorageEntry(string name, int length, byte[] hash, byte[] rawStorage, DateTime since, DateTime? until = null)
            : base(name, length, hash, since, until)
        {
            RawStorage = rawStorage;
        }

        public override Stream Open()
        {
            return new MemoryStream(RawStorage);
        }
   }
}
