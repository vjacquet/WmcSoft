#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace WmcSoft.IO.Sources
{
    /// <summary>
    /// Abstract base class of a service for loading and storing stream while maintaining versions.
    /// </summary>
    /// <remarks>The stream name must not contain invalid file name chars.</remarks>
    public abstract class StreamStore
    {
        class NewStorageEntry : StorageEntry
        {
            public NewStorageEntry(string name, int length, byte[] hash, DateTime since)
                : base(name, length, hash, since)
            {
            }

            protected override void CloseEntry(DateTime asOf)
            {
            }

            public override Stream GetStream()
            {
                throw new InvalidOperationException();
            }
        }

        protected StreamStore()
        {
        }

        /// <summary>
        /// Creates a new instance of the <seealso cref="HashAlgorithm"/> used to check the stream data.
        /// </summary>
        /// <returns>A new instance of the <see cref="HashAlgorithm"/>.</returns>
        protected virtual HashAlgorithm CreateHashAlgorithm()
        {
            return SHA256.Create();
        }

        /// <summary>
        /// Computes the hash if the given <paramref name="readable"/> stream source.
        /// </summary>
        /// <param name="readable">The readable stream source</param>
        /// <returns>The has code.</returns>
        /// <remarks>It can be any readable stream source.</remarks>
        public byte[] ComputeHash(IStreamSource readable)
        {
            using (var stream = readable.GetStream())
            using (var algorithm = CreateHashAlgorithm()) {
                return algorithm.ComputeHash(stream);
            }
        }

        /// <summary>
        /// Finds the current entry for the given name valid at the given type.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        public StorageEntry Find(string name)
        {
            return Find(name, DateTime.UtcNow);
        }

        protected virtual DateTime MaxDate => DateTime.MaxValue;

        /// <summary>
        /// Finds the entry for the given name valid at the given type.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="asOfUtc">The as of date of the file, in UTC.</param>
        public abstract StorageEntry Find(string name, DateTime asOfUtc);

        /// <summary>
        /// Gets all versions of a given stream.
        /// </summary>
        /// <param name="asOfUtc">The name of the stream.</param>
        /// <returns>The storage entries of the given stream, sorted by <see cref="StorageEntry.ValidSince"/> in descending order.</returns>
        public abstract IEnumerable<StorageEntry> GetHistory(string name);

        /// <summary>
        /// Gets all storage entries at the given date.
        /// </summary>
        /// <param name="asOfUtc">The requested date, in UTC.</param>
        /// <returns>The storage entries</returns>
        public abstract IEnumerable<StorageEntry> GetInventory(DateTime asOfUtc);

        /// <summary>
        /// Gets a readable stream for the given name.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="asOfUtc">The as of date of the file, in UTC.</param>
        /// <returns>The stream; or <c>null</c> if it does not exists.</returns>
        public virtual Stream Load(string name, DateTime asOfUtc)
        {
            var found = Find(name, asOfUtc);
            if (found != null)
                return found.GetStream();
            return null;
        }

        /// <summary>
        /// Store the given stream.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="stream">The stream.</param>
        /// <returns><c>true</c> if the stream was store; otherwise, <c>false</c>.</returns>
        /// <remarks>If the stream is not seekable, it is read only once. The stream is not closed.</remarks>
        public virtual bool Store(string name, Stream stream)
        {
            var capacity = stream.CanSeek ? (int)stream.Length : BufferSize;

            using (var ms = new MemoryStream(capacity))
            using (var algorithm = CreateHashAlgorithm())
            using (var cs = new CryptoStream(ms, algorithm, CryptoStreamMode.Write)) {
                var buffer = new byte[BufferSize];
                var read = (int)Copy(stream, cs);
                cs.FlushFinalBlock();
                ms.Seek(0, SeekOrigin.Begin);

                var hash = algorithm.Hash;
                var found = Find(name, MaxDate);
                if (found != null && found.Hash.SequenceEqual(hash))
                    return false;

                var entry = new NewStorageEntry(name, read, hash, Now());
                Store(found, entry, ms);

                return true;
            }
        }

        protected abstract void Store(StorageEntry latest, StorageEntry metadata, Stream stream);

        /// <summary>
        /// Uploads the given stream to the store when it is different that the up to date version.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="readable">The readble source.</param>
        /// <returns><c>true</c> if the file was uploaded, false otherwise.</returns>
        public bool Upload(string name, IStreamSource readable)
        {
            using (var stream = readable.GetStream()) {
                return Store(name, stream);
            }
        }

        /// <summary>
        /// Downloads the given stream from the store if it is more recent that the current writable source.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="asOf">The date of the requested version.</param>
        /// <param name="writable">The timestamp writable source.</param>
        /// <returns><c>true</c> if the file was uploaded, false otherwise.</returns>
        public bool Download(string name, DateTime asOf, ITimestampStreamSource writable)
        {
            var now = asOf;
            var entry = Find(name, now);
            if (entry != null && entry.ValidSinceUtc > writable.Timestamp.GetValueOrDefault()) {
                using (var input = entry.GetStream())
                using (var output = writable.GetStream()) {
                    const int BufferSize = 4096;
                    var buffer = new byte[BufferSize];
                    var read = 0;
                    while ((read = input.Read(buffer, 0, BufferSize)) > 0)
                        output.Write(buffer, 0, read);
                }
                return entry.ValidSinceUtc <= writable.Timestamp.GetValueOrDefault();
            }
            return false;
        }

        /// <summary>
        /// Downloads the given stream from the store if it is more recent that the current writable source.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="writable">The timestamp writable source.</param>
        /// <returns><c>true</c> if the file was uploaded, false otherwise.</returns>
        public bool Download(string name, ITimestampStreamSource writable)
        {
            return Download(name, DateTime.UtcNow, writable);
        }

        #region Helpers 

        protected byte[] ReadAll(Stream stream)
        {
            if (stream is MemoryStream) {
                return ((MemoryStream)stream).ToArray();
            } else if (stream.CanSeek) {
                var count = (int)stream.Length;
                var buffer = new byte[count];
                stream.Read(buffer, 0, count);
                return buffer;
            } else {
                var ms = new MemoryStream();
                Copy(stream, ms);
                ms.Seek(0, SeekOrigin.Begin);
                return ms.ToArray();
            }
        }

        private const int BufferSize = 81920; // https://msdn.microsoft.com/en-us/library/dd783870(v=vs.110).aspx

        protected static long Copy(Stream from, Stream to)
        {
            long size = 0L;
            var buffer = new byte[BufferSize];
            var read = 0;
            while ((read = from.Read(buffer, 0, BufferSize)) > 0) {
                to.Write(buffer, 0, read);
                size += read;
            }
            return size;
        }

        protected DateTime Now()
        {
            return DateTime.UtcNow;
        }

        #endregion
    }
}
