using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.IO
{
    [TestClass]
    public class StreamExtensionsTests
    {
        public class Progress : IProgress<long>
        {
            readonly object _syncRoot = new Object();
            readonly List<long> _notifications = new List<long>();

            #region IProgress<long> Members

            public void Report(long value) {
                lock (_syncRoot) {
                    _notifications.Add(value);
                }
            }

            #endregion

            public List<long> Notifications { get { return _notifications; } }
        }

        [TestMethod]
        public void CheckCopyToWithProgress() {
            var buffer = new byte[1024];
            var random = new Random(1664);
            random.NextBytes(buffer);

            var progress = new Progress();

            using (var source = new MemoryStream(buffer))
            using (var destination = new MemoryStream()) {
                source.CopyToAsync(destination, 256, progress).Wait();

                CollectionAssert.AreEqual(buffer, destination.GetBuffer());
            }

            var expected = new[] { 256L, 512L, 768L, 1024L };
            var actual = progress.Notifications;
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
