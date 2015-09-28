using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.IO
{
    [TestClass]
    public class SubstringReaderTests
    {
        [TestMethod]
        public void CanPeek() {
            var data = "abcdefghijklmnopqrstuvwxyz";
            using (var reader = new SubstringReader(data, 1, 5)) {
                Assert.AreEqual('b', reader.Peek());
                Assert.AreEqual('b', reader.Read());
                Assert.AreEqual('c', reader.Peek());
            }
        }

        [TestMethod]
        public void CanReadToEnd() {
            var data = "abcdefghijklmnopqrstuvwxyz";
            using (var reader = new SubstringReader(data, 1, 5)) {
                const string expected = "bcdef";
                var actual = reader.ReadToEnd();
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void CanReadLines() {
            var data = "abcde\r\nfghij\r\nklmno\r\npqrs\r\ntuvxy";
            using (var reader = new SubstringReader(data, 0, 16)) {
                Assert.AreEqual("abcde", reader.ReadLine());
                Assert.AreEqual("fghij", reader.ReadLine());
                Assert.AreEqual("kl", reader.ReadLine());
                Assert.AreEqual(null, reader.ReadLine());
            }
        }

        [TestMethod]
        public void CanReadLinesWhenLastLineIsAnEmptyLine() {
            var data = "abcde\r\nfghij\r\nklmno\r\npqrs\r\ntuvxy";
            using (var reader = new SubstringReader(data, 0, 14)) {
                Assert.AreEqual("abcde", reader.ReadLine());
                Assert.AreEqual("fghij", reader.ReadLine());
                Assert.AreEqual(null, reader.ReadLine());
            }
        }
    }
}
