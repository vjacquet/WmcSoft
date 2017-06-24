using System;
using System.IO;
using Xunit;

namespace WmcSoft.IO
{
    public class StreamSourceTests
    {
        [Fact]
        public void CheckFileStreamSourceDoesNotThrowOnMissingFile()
        {
            var expected = "lorem.ipsum";
            var actual = new FileStreamSource(expected);
            Assert.True(Path.IsPathRooted(actual.Path));
            Assert.False(File.Exists(actual.Path));
            Assert.Equal(expected, Path.GetFileName(actual.Path));
        }

        [Fact]
        public void CheckFileStreamSourceThrowsOnInvalidfileName()
        {
            Assert.Throws<NotSupportedException>(() => new FileStreamSource("bad:filename.txt"));
        }
    }
}
