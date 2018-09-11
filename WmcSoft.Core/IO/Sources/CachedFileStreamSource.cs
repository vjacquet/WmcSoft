using System;
using System.IO;
using log4net;

namespace DevFi.Tools.IO.Sources
{
    /// <summary>
    /// Implements a <see cref="IStreamSource"/> that updates a local cache when a <see cref="ITimestampStreamSource"/> is more recent.
    /// </summary>
    /// <remarks>The stream is read from the local cache.</remarks>
    public class CachedFileStreamSource : ITimestampStreamSource
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IStreamSource));

        private readonly FileStreamSource _cache;
        private readonly ITimestampStreamSource _underlying;

        public CachedFileStreamSource(ITimestampStreamSource source, string cachePath)
        {
            _underlying = source;
            _cache = new FileStreamSource(cachePath);
        }

        public override string ToString()
        {
            return $"{GetType()} for {_underlying}";
        }

        public DateTime? LastWriteTimeUtc
        {
            get
            {
                var remote = _underlying.LastWriteTimeUtc;
                var local = _cache.LastWriteTimeUtc;
                var result = Nullable.Compare(remote, local);
                if (result > 0)
                    return remote;
                return local;
            }
        }

        public Stream Open()
        {
            var remote = _underlying.LastWriteTimeUtc;
            var local = _cache.LastWriteTimeUtc;
            var result = Nullable.Compare(remote, local);
            if (result > 0)
            {
                Logger.Info($"Caching `{_cache.Path}` with {_underlying}.");

                var path = _cache.Path;
                var temp = Path.ChangeExtension(path, ".tmp");
                using (var source = _underlying.Open())
                using (var target = File.Create(temp))
                {
                    byte[] array = new byte[80*1024];
                    int count;
                    while ((count = source.Read(array, 0, array.Length)) != 0)
                    {
                        target.Write(array, 0, count);
                    }
                }
                if (!File.Exists(path))
                    File.Move(temp, path);
                else
                    File.Replace(temp, path, Path.ChangeExtension(path, ".bak"));
            }
            return _cache.Open();
        }
    }
}
