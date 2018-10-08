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
using System.Diagnostics;
using System.IO;

namespace WmcSoft.IO.Sources
{
    /// <summary>
    /// Implements a <see cref="ITimestampStreamSource"/> that updates a local cache when a <see cref="ITimestampStreamSource"/> is more recent,
    /// then returns the stream from the local cache.
    /// </summary>
    public class CachedFileStreamSource : ITimestampStreamSource
    {
        private readonly FileStreamSource cache;
        private readonly ITimestampStreamSource underlying;

        public CachedFileStreamSource(ITimestampStreamSource source, string cachePath)
        {
            underlying = source;
            cache = new FileStreamSource(cachePath);
        }

        public override string ToString()
        {
            return $"{GetType()} for {underlying}";
        }

        /// <inheritdoc />
        public bool SupportTimestamp => true;

        /// <inheritdoc />
        public DateTime? Timestamp {
            get {
                var remote = underlying.Timestamp;
                var local = cache.Timestamp;
                var result = Nullable.Compare(remote, local);
                if (result > 0)
                    return remote;
                return local;
            }
        }

        /// <summary>
        /// Gets the stream from the source.
        /// </summary>
        /// <returns>A stream.</returns>
        public Stream GetStream()
        {
            var remote = underlying.Timestamp;
            var local = cache.Timestamp;
            var result = Nullable.Compare(remote, local);
            if (result > 0) {
                Trace.TraceInformation($"Upgrading cache `{cache.Path}` with {underlying}.");

                var path = cache.Path;
                var temp = Path.ChangeExtension(path, ".tmp");
                using (var source = underlying.GetStream())
                using (var target = File.Create(temp)) {
                    byte[] array = new byte[80 * 1024];
                    int count;
                    while ((count = source.Read(array, 0, array.Length)) != 0) {
                        target.Write(array, 0, count);
                    }
                }
                File.SetLastWriteTimeUtc(temp, remote.GetValueOrDefault());
                if (!File.Exists(path))
                    File.Move(temp, path);
                else
                    File.Replace(temp, path, Path.ChangeExtension(path, ".bak"));
            }
            return cache.GetStream();
        }
    }
}
