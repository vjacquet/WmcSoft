using System;
using System.IO;
using System.Linq;
using Xunit;

namespace WmcSoft.IO.Sources
{
    public abstract class StreamStoreTests
    {
        #region WritableMemoryStream class

        class WritableMemoryStream : ITimestampStreamSource
        {
            private readonly IDateTimeSource _source;

            public DateTime? LastWriteTimeUtc { get; private set; }
            public byte[] StoredData { get; private set; }

            public WritableMemoryStream(IDateTimeSource source)
            {
                _source = source;
            }

            public WritableMemoryStream(IDateTimeSource source, byte[] data)
            {
                _source = source;
                LastWriteTimeUtc = source.Now();
                StoredData = data;
            }


            public Stream Open()
            {
                var ms = new MemoryStream();
                return new ClosableStream(ms, onClosed: (sender, e) =>
                {
                    StoredData = ms.ToArray();
                    LastWriteTimeUtc = _source.Now();
                });
            }
        }

        #endregion

        #region Helpers

        protected static byte[] Sample(int seed, int length)
        {
            var random = new Random(seed);
            var buffer = new byte[length];
            random.NextBytes(buffer);
            return buffer;
        }

        #endregion

        protected abstract StreamStore CreateEmptyStore(IDateTimeSource dateTimeSource);

        [Fact]
        public void InventoryIsEmptyWhenTheStoreIsEmpty()
        {
            var clock = new DateTimeSourceStub(2018, 01, 22);
            var store = CreateEmptyStore(clock);
            clock.Advance(TimeSpan.FromHours(1));
            Assert.Empty(store.GetInventory(clock.Now()).ToList());
        }

        [Fact]
        public void HistoryIsEmptyWhenTheStoreIsEmpty()
        {
            var clock = new DateTimeSourceStub(2018, 01, 22);
            var store = CreateEmptyStore(clock);
            Assert.Empty(store.GetHistory("file.ext").ToList());
        }

        [Fact]
        public void StoringAFileMakesItVisibleInTheStore()
        {
            var clock = new DateTimeSourceStub(2018, 01, 22);
            var store = CreateEmptyStore(clock);

            var asOf = clock.Now();
            var data = Sample(1789, 421);
            using (var ms = new MemoryStream(data))
            {
                Assert.True(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromHours(1));
            var inventory = store.GetInventory(clock.Now()).ToList();
            Assert.NotEmpty(inventory);
            var entry = inventory.Single();
            Assert.AreEqual("file.ext", entry.Name);
            Assert.AreEqual(asOf, entry.ValidSinceUtc);

            var entries = store.GetHistory("file.ext").ToList();
            Assert.AreEqual(1, entries.Count);
        }

        [Fact]
        public void StoringAFileTwiceDoesNotCreateANewEntry()
        {
            var clock = new DateTimeSourceStub(2018, 01, 22);
            var store = CreateEmptyStore(clock);

            var asOf = clock.Now();
            var data = Sample(1789, 421);
            using (var ms = new MemoryStream(data))
            {
                Assert.IsTrue(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromHours(1));
            using (var ms = new MemoryStream(data))
            {
                Assert.IsFalse(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromHours(1));
            var inventory = store.GetInventory(clock.Now()).ToList();
            Assert.IsNotEmpty(inventory);
            var entry = inventory.Single();
            Assert.AreEqual("file.ext", entry.Name);
            Assert.AreEqual(asOf, entry.ValidSinceUtc);

            var entries = store.GetHistory("file.ext").ToList();
            Assert.AreEqual(1, entries.Count);
        }

        [Fact]
        public void StoringADifferentFileWithTheSameNameCreatesANewEntry()
        {
            var clock = new DateTimeSourceStub(2018, 01, 22);
            var store = CreateEmptyStore(clock);

            var asOf1 = clock.Now();
            var data = Sample(1789, 421);
            using (var ms = new MemoryStream(data))
            {
                Assert.IsTrue(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromHours(1));
            var asOf2 = clock.Now();
            using (var ms = new MemoryStream(data, 100, 100))
            {
                Assert.IsTrue(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromHours(1));
            var inventory = store.GetInventory(clock.Now()).ToList();
            Assert.IsNotEmpty(inventory);
            var entry = inventory.Single();
            Assert.AreEqual("file.ext", entry.Name);
            Assert.AreEqual(asOf2, entry.ValidSinceUtc);

            var entries = store.GetHistory("file.ext").ToList();
            Assert.AreEqual(2, entries.Count);
            Assert.AreEqual(asOf2, entries[0].ValidSinceUtc);
            Assert.AreEqual(asOf1, entries[1].ValidSinceUtc);
            Assert.AreEqual(asOf2, entries[1].ValidUntilUtc);
        }

        [Fact]
        public void CanDownloadFileFromStore()
        {
            var clock = new DateTimeSourceStub(2018, 01, 22);
            var store = CreateEmptyStore(clock);

            var data = Sample(1789, 421);
            using (var ms = new MemoryStream(data))
            {
                Assert.IsTrue(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromMinutes(5));
            var asOf = clock.Now();
            var writable = new WritableMemoryStream(clock);
            Assert.IsTrue(store.Download("file.ext", writable));
            Assert.AreEqual(asOf, writable.LastWriteTimeUtc);
            Assert.IsTrue(data.SequenceEqual(writable.StoredData));
        }

        [Fact]
        public void DoNotDownloadFileFromStoreWhenStreamIsOlder()
        {
            var clock = new DateTimeSourceStub(2018, 01, 22);
            var store = CreateEmptyStore(clock);

            var data = Sample(1789, 421);
            using (var ms = new MemoryStream(data))
            {
                Assert.IsTrue(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromMinutes(5));
            var asOf = clock.Now();
            var writable = new WritableMemoryStream(clock, new byte[0]);
            Assert.IsFalse(store.Download("file.ext", writable));
            Assert.AreEqual(asOf, writable.LastWriteTimeUtc);
        }
    }
}
