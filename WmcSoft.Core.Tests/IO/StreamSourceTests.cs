using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.IO
{
    [TestClass]
    public class StreamSourceTests
    {
        [TestMethod]
        public void CheckFileStreamSourceDoesNotThrowOnMissingFile() {
            var expected = "lorem.ipsum";
            var actual = new FileStreamSource(expected);
            Assert.IsTrue(Path.IsPathRooted(actual.Path));
            Assert.IsFalse(File.Exists(actual.Path));
            Assert.AreEqual(expected, Path.GetFileName(actual.Path));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void CheckFileStreamSourceThrowsOnInvalidfileName() {
            var actual = new FileStreamSource("bad:filename.txt");
        }
    }
}
