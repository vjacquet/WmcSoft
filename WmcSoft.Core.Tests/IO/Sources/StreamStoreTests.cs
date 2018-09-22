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

            public bool SupportTimestamp => true;
            public DateTime? Timestamp { get; private set; }
            public byte[] StoredData { get; private set; }

            public WritableMemoryStream(IDateTimeSource source)
            {
                _source = source;
            }

            public WritableMemoryStream(IDateTimeSource source, byte[] data)
            {
                _source = source;
                Timestamp = source.Now();
                StoredData = data;
            }


            public Stream GetStream()
            {
                var ms = new MemoryStream();
                return new ClosableStream(ms, onClosed: (sender, e) => {
                    StoredData = ms.ToArray();
                    Timestamp = _source.Now();
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
            using (var ms = new MemoryStream(data)) {
                Assert.True(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromHours(1));
            var inventory = store.GetInventory(clock.Now()).ToList();
            Assert.NotEmpty(inventory);
            var entry = inventory.Single();
            Assert.Equal("file.ext", entry.Name);
            Assert.Equal(asOf, entry.ValidSinceUtc);

            var entries = store.GetHistory("file.ext").ToList();
            Assert.Single(entries);
        }

        [Fact]
        public void StoringAFileTwiceDoesNotCreateANewEntry()
        {
            var clock = new DateTimeSourceStub(2018, 01, 22);
            var store = CreateEmptyStore(clock);

            var asOf = clock.Now();
            var data = Sample(1789, 421);
            using (var ms = new MemoryStream(data)) {
                Assert.True(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromHours(1));
            using (var ms = new MemoryStream(data)) {
                Assert.False(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromHours(1));
            var inventory = store.GetInventory(clock.Now()).ToList();
            Assert.NotEmpty(inventory);
            var entry = inventory.Single();
            Assert.Equal("file.ext", entry.Name);
            Assert.Equal(asOf, entry.ValidSinceUtc);

            var entries = store.GetHistory("file.ext").ToList();
            Assert.Single(entries);
        }

        [Fact]
        public void StoringADifferentFileWithTheSameNameCreatesANewEntry()
        {
            var clock = new DateTimeSourceStub(2018, 01, 22);
            var store = CreateEmptyStore(clock);

            var asOf1 = clock.Now();
            var data = Sample(1789, 421);
            using (var ms = new MemoryStream(data)) {
                Assert.True(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromHours(1));
            var asOf2 = clock.Now();
            using (var ms = new MemoryStream(data, 100, 100)) {
                Assert.True(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromHours(1));
            var inventory = store.GetInventory(clock.Now()).ToList();
            Assert.NotEmpty(inventory);
            var entry = inventory.Single();
            Assert.Equal("file.ext", entry.Name);
            Assert.Equal(asOf2, entry.ValidSinceUtc);

            var entries = store.GetHistory("file.ext").ToList();
            Assert.Equal(2, entries.Count);
            Assert.Equal(asOf2, entries[0].ValidSinceUtc);
            Assert.Equal(asOf1, entries[1].ValidSinceUtc);
            Assert.Equal(asOf2, entries[1].ValidUntilUtc);
        }

        [Fact]
        public void CanDownloadFileFromStore()
        {
            var clock = new DateTimeSourceStub(2018, 01, 22);
            var store = CreateEmptyStore(clock);

            var data = Sample(1789, 421);
            using (var ms = new MemoryStream(data)) {
                Assert.True(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromMinutes(5));
            var asOf = clock.Now();
            var writable = new WritableMemoryStream(clock);
            Assert.True(store.Download("file.ext", writable));
            Assert.Equal(asOf, writable.Timestamp);
            Assert.True(data.SequenceEqual(writable.StoredData));
        }

        [Fact]
        public void DoNotDownloadFileFromStoreWhenStreamIsOlder()
        {
            var clock = new DateTimeSourceStub(2018, 01, 22);
            var store = CreateEmptyStore(clock);

            var data = Sample(1789, 421);
            using (var ms = new MemoryStream(data)) {
                Assert.True(store.Store("file.ext", ms));
            }

            clock.Advance(TimeSpan.FromMinutes(5));
            var asOf = clock.Now();
            var writable = new WritableMemoryStream(clock, new byte[0]);
            Assert.False(store.Download("file.ext", writable));
            Assert.Equal(asOf, writable.Timestamp);
        }
    }
}
