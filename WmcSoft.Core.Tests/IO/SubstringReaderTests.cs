using Xunit;

namespace WmcSoft.IO
{
    public class SubstringReaderTests
    {
        [Fact]
        public void CanPeek()
        {
            var data = "abcdefghijklmnopqrstuvwxyz";
            using (var reader = new SubstringReader(data, 1, 5)) {
                Assert.Equal('b', reader.Peek());
                Assert.Equal('b', reader.Read());
                Assert.Equal('c', reader.Peek());
            }
        }

        [Fact]
        public void CanReadToEnd()
        {
            var data = "abcdefghijklmnopqrstuvwxyz";
            using (var reader = new SubstringReader(data, 1, 5)) {
                const string expected = "bcdef";
                var actual = reader.ReadToEnd();
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void CanReadLines()
        {
            var data = "abcde\r\nfghij\r\nklmno\r\npqrs\r\ntuvxy";
            using (var reader = new SubstringReader(data, 0, 16)) {
                Assert.Equal("abcde", reader.ReadLine());
                Assert.Equal("fghij", reader.ReadLine());
                Assert.Equal("kl", reader.ReadLine());
                Assert.Null(reader.ReadLine());
            }
        }

        [Fact]
        public void CanReadLinesWhenLastLineIsAnEmptyLine()
        {
            var data = "abcde\r\nfghij\r\nklmno\r\npqrs\r\ntuvxy";
            using (var reader = new SubstringReader(data, 0, 14)) {
                Assert.Equal("abcde", reader.ReadLine());
                Assert.Equal("fghij", reader.ReadLine());
                Assert.Null(reader.ReadLine());
            }
        }
    }
}
