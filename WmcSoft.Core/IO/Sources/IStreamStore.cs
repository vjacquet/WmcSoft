using System;
using System.Collections.Generic;
using System.IO;

namespace DevFi.Tools.IO.Sources
{
    /// <summary>
    /// Defines a service for loading and storing stream while maintaining versions.
    /// </summary>
    /// <remarks>The stream name must not contain invalid file name chars.</remarks>
    public interface IStreamStore
    {
        /// <summary>
        /// Gets a readable stream for the given name.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="asOfUtc">The as of date of the file, in UTC.</param>
        /// <returns>The stream; or <c>null</c> if it does not exists.</returns>
        Stream Load(string name, DateTime asOfUtc);

        /// <summary>
        /// Store the given stream.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="stream">The stream.</param>
        /// <returns><c>true</c> if the stream was store; otherwise, <c>false</c>.</returns>
        /// <remarks>If the stream is not seekable, it is read only once. The stream is not closed.</remarks>
        bool Store(string name, Stream stream);

        /// <summary>
        /// Finds the entry for the given name valid at the given type.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="asOfUtc">The as of date of the file, in UTC.</param>
        StorageEntry Find(string name, DateTime asOfUtc);

        /// <summary>
        /// Gets all storage entries at the given date.
        /// </summary>
        /// <param name="asOfUtc">The requested date, in UTC.</param>
        /// <returns>The storage entries</returns>
        IEnumerable<StorageEntry> GetInventory(DateTime asOfUtc);

        /// <summary>
        /// Gets all versions of a given stream.
        /// </summary>
        /// <param name="asOfUtc">The name of the stream.</param>
        /// <returns>The storage entries of the given stream, sorted by <see cref="StorageEntry.ValidSince"/> in descending order.</returns>
        IEnumerable<StorageEntry> GetHistory(string name);

        /// <summary>
        /// Computes the hash if the given <paramref name="readable"/> stream source.
        /// </summary>
        /// <param name="readable">The readable stream source</param>
        /// <returns>The has code.</returns>
        /// <remarks>It can be any readable stream source.</remarks>
        byte[] ComputeHash(IStreamSource readable);
    }

    /// <summary>
    /// Extensions for the stream store.
    /// This is a static class.
    /// </summary>
    public static class StreamStoreExtensions
    {
        /// <summary>
        /// Uploads the given stream to the store when it is different that the up to date version.
        /// </summary>
        /// <param name="store">The stream store.</param>
        /// <param name="name">The name of the stream.</param>
        /// <param name="readable">The readble source.</param>
        /// <returns><c>true</c> if the file was uploaded, false otherwise.</returns>
        public static bool Upload(this IStreamStore store, string name, IStreamSource readable)
        {
            using (var stream = readable.Open())
            {
                return store.Store(name, stream);
            }
        }

        /// <summary>
        /// Downloads the given stream from the store if it is more recent that the current writable source.
        /// </summary>
        /// <param name="store">The stream store.</param>
        /// <param name="name">The name of the stream.</param>
        /// <param name="asOf">The date of the requested version.</param>
        /// <param name="writable">The timestamp writable source.</param>
        /// <returns><c>true</c> if the file was uploaded, false otherwise.</returns>
        public static bool Download(this IStreamStore store, string name, DateTime asOf, ITimestampStreamSource writable)
        {
            var now = asOf;
            var entry = store.Find(name, now);
            if (entry != null && entry.ValidSinceUtc > writable.LastWriteTimeUtc.GetValueOrDefault())
            {
                using (var input = entry.Open())
                using (var output = writable.Open())
                {
                    const int BufferSize = 4096;
                    var buffer = new byte[BufferSize];
                    var read = 0;
                    while ((read = input.Read(buffer, 0, BufferSize)) > 0)
                        output.Write(buffer, 0, read);
                }
                return entry.ValidSinceUtc <= writable.LastWriteTimeUtc.GetValueOrDefault();
            }
            return false;
        }

        /// <summary>
        /// Downloads the given stream from the store if it is more recent that the current writable source.
        /// </summary>
        /// <param name="store">The stream store.</param>
        /// <param name="name">The name of the stream.</param>
        /// <param name="writable">The timestamp writable source.</param>
        /// <returns><c>true</c> if the file was uploaded, false otherwise.</returns>
        public static bool Download(this IStreamStore store, string name, ITimestampStreamSource writable)
        {
            return Download(store, name, DateTime.UtcNow, writable);
        }
    }
}
