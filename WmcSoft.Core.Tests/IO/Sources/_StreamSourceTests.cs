using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace DevFi.Tools.IO.Sources
{
    [TestFixture]
    public class StreamSourceTests
    {
        static string Read(IStreamSource source)
        {
            var stream = source.Open();
            if (stream == null)
                return null;
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        static string CreateTempFileName()
        {
            var path = Path.GetTempFileName();
            File.Delete(path);
            return Path.ChangeExtension(path, ".dat");
        }

        static string CreateTempFile(string contents, DateTime lastWriteTimeUtc)
        {
            var path = CreateTempFileName();

            File.WriteAllText(path, contents);
            File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
            return path;
        }

        class TimestampStreamSourceStub : ITimestampStreamSource
        {
            public DateTime? LastWriteTimeUtc { get; set; }
            public string Contents { get; set; }

            public Stream Open()
            {
                if (Contents == null)
                    return null;

                var ms = new MemoryStream();
                using (var writer = new StreamWriter(ms, Encoding.UTF8, 4096))
                {
                    writer.Write(Contents);
                    writer.Flush();

                    return new MemoryStream(ms.GetBuffer(), 0, (int)ms.Length);
                }
            }
        }

        class TimestampStreamSourceMock : ITimestampStreamSource
        {
            static string DefaultContents() => null;
            static DateTime? DefaultLastWriteTimeUtc() => null;

            private readonly Func<string> _contents;
            private readonly Func<DateTime?> _lastWriteTimeUtc;

            public TimestampStreamSourceMock(Func<string> contents = null, Func<DateTime?> lastWriteTimeUtc = null)
            {
                _contents = contents ?? DefaultContents;
                _lastWriteTimeUtc = lastWriteTimeUtc ?? DefaultLastWriteTimeUtc;
            }

            public DateTime? LastWriteTimeUtc => _lastWriteTimeUtc();

            public Stream Open()
            {
                var contents = _contents();
                if (contents == null)
                    return null;

                var ms = new MemoryStream();
                using (var writer = new StreamWriter(ms, Encoding.UTF8, 4096))
                {
                    writer.Write(contents);
                    writer.Flush();

                    return new MemoryStream(ms.GetBuffer(), 0, (int)ms.Length);
                }
            }
        }

        [Test]
        public void CanCacheSource()
        {
            var source = new TimestampStreamSourceStub
            {
                LastWriteTimeUtc = new DateTime(2017, 07, 14),
                Contents = "Remote"
            };

            var temp = CreateTempFileName();

            var caching = new CachedFileStreamSource(source, temp);
            Assert.AreEqual("Remote", Read(caching));
            Assert.IsTrue(File.Exists(temp));
            Assert.AreEqual("Remote", File.ReadAllText(temp));
            File.Delete(temp);
        }

        [Test]
        public void CanCacheSourceWhenMoreRecent()
        {
            var source = new TimestampStreamSourceStub
            {
                LastWriteTimeUtc = new DateTime(2017, 07, 14),
                Contents = "Remote"
            };

            var temp = CreateTempFile("Local", new DateTime(2017, 07, 1));
            var caching = new CachedFileStreamSource(source, temp);
            Assert.AreEqual("Remote", Read(caching));
            Assert.IsTrue(File.Exists(temp));
            Assert.AreEqual("Remote", File.ReadAllText(temp));
            File.Delete(temp);
        }

        [Test]
        public void CanPreserveCacheWhenMoreRecentThanSource()
        {
            var source = new TimestampStreamSourceStub
            {
                LastWriteTimeUtc = new DateTime(2017, 07, 14),
                Contents = "Remote"
            };

            var temp = CreateTempFile("Local", new DateTime(2017, 08, 1));
            var caching = new CachedFileStreamSource(source, temp);
            Assert.AreEqual("Local", Read(caching));
            Assert.IsTrue(File.Exists(temp));
            Assert.AreEqual("Local", File.ReadAllText(temp));
            File.Delete(temp);
        }

        [Test]
        public void CanBackupLocal()
        {
            var source = new TimestampStreamSourceStub
            {
                LastWriteTimeUtc = new DateTime(2017, 07, 14),
                Contents = "Remote"
            };

            var temp = CreateTempFile("Local", new DateTime(2017, 08, 1));
            var backup = Path.ChangeExtension(temp, ".bak");
            var caching = new CachedFileStreamSource(source, temp);

            Assert.AreEqual("Local", Read(caching));

            source.LastWriteTimeUtc = new DateTime(2017, 08, 14);
            source.Contents = "Updated";
            Assert.AreEqual("Updated", Read(caching));

            Assert.AreEqual("Updated", File.ReadAllText(temp));
            Assert.AreEqual("Local", File.ReadAllText(backup));

            File.Delete(temp);
            File.Delete(backup);
        }

        [Test]
        public void CanGetMostRecentLastWriteTimeUtc()
        {
            var source = new MostRecentStreamSource {
                new TimestampStreamSourceStub(),
                new TimestampStreamSourceStub { LastWriteTimeUtc = new DateTime(2016, 01, 01) },
                new TimestampStreamSourceStub(),
                new TimestampStreamSourceStub { LastWriteTimeUtc = new DateTime(2017, 01, 01) },
                new TimestampStreamSourceStub { LastWriteTimeUtc = new DateTime(2015, 01, 01) },
            };

            Assert.AreEqual(2017, source.LastWriteTimeUtc.Value.Year);
        }

        [Test]
        public void CanShieldTimestampStreamSource()
        {
            var source = new TimestampStreamSourceMock(contents: () => { throw new InvalidOperationException(); });
            var shield = source.Shield();
            Assert.IsNotNull(shield);
            Assert.IsInstanceOf<TimestampShieldingStreamSource>(shield);

            var s = shield.Open();
            Assert.IsNull(s);
            Assert.IsNotNull(shield.Error);
            Assert.IsInstanceOf<InvalidOperationException>(shield.Error);
        }
    }
}
