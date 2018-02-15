using System.IO;
using Xunit;

namespace WmcSoft.IO
{
    public class PathInfoTests
    {
        [Theory]
        [InlineData(@"c:\path\to\file.ext", @"c:\path\to\file-suffix.ext")]
        [InlineData(@"path\to\file.ext", @"path\to\file-suffix.ext")]
        [InlineData(@"file.ext", @"file-suffix.ext")]
        [InlineData(@".ext", @"-suffix.ext")]
        public void CanAddSuffixToFileName(string path, string expected)
        {
            var pi = new PathInfo(path);
            Assert.Equal(expected, (string)pi.AppendBeforeExtension("-suffix"));
        }

        [Theory]
        [InlineData(@"c:\path\to\file.ext", @"c:\path\to", "file", ".ext")]
        [InlineData(@"unrooted-path\to\file.ext", @"unrooted-path\to", "file", ".ext")]
        [InlineData(@"file.ext", @"", "file", ".ext")]
        [InlineData(@"c:\path\to\.ext", @"c:\path\to", "", ".ext")]
        public void CanGetPathParts(string path, string directory, string fileName, string extension)
        {
            var pi = new PathInfo(path);
            Assert.Equal(directory, pi.DirectoryName);
            Assert.Equal(fileName, pi.FileNameWithoutExtension);
            Assert.Equal(extension, pi.Extension);
        }

        [Fact]
        public void CheckUnrootedPathIsNull()
        {
            PathInfo p = "path/to/file";
            var root = p.Root;
            Assert.Null(root);
        }

        [Fact]
        public void CheckRootedPathIsNotNull()
        {
            PathInfo p = @"c:\path\to\file";
            var root = p.Root;
            Assert.Equal(@"c:\", root);
        }

        [Fact]
        public void CheckFileName()
        {
            PathInfo p = "path/to/file";
            var fileName = p.FileName;
            Assert.Equal("file", fileName);
        }

        [Fact]
        public void CheckExtension()
        {
            PathInfo p = "path/to/file.ext";
            var extension = p.Extension;
            Assert.Equal(".ext", extension);
        }

        [Fact]
        public void CheckExtensionlessExtensionIsNull()
        {
            PathInfo p = "path/to/file";
            var extension = p.Extension;
            Assert.Null(extension);
        }

        [Fact]
        public void CheckCombineOperator()
        {
            PathInfo p = @"c:\path";
            Assert.Equal(@"c:\path\to\file", p / "to" / "file");
        }
    }
}
