using System.IO;
using Xunit;

namespace WmcSoft.IO
{
    public class PathInfoTests
    {
        static string AddFileNameSuffix(string path, string suffix)
        {
            var directory = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path) + suffix + Path.GetExtension(path);
            return Path.Combine(directory, fileName);
        }

        [Theory]
        [InlineData(@"c:\path\to\file.ext", @"c:\path\to\file-suffix.ext")]
        [InlineData(@"path\to\file.ext", @"path\to\file-suffix.ext")]
        [InlineData(@"file.ext", @"file-suffix.ext")]
        [InlineData(@".ext", @"-suffix.ext")]
        public void CanAddSuffixToFileName(string path, string expected)
        {
            Assert.Equal(expected, AddFileNameSuffix(path, "-suffix"));
        }

        [Theory]
        [InlineData(@"c:\path\to\file.ext", @"c:\path\to", "file", ".ext")]
        [InlineData(@"unrooted-path\to\file.ext", @"unrooted-path\to", "file", ".ext")]
        [InlineData(@"file.ext", @"", "file", ".ext")]
        [InlineData(@"c:\path\to\.ext", @"c:\path\to", "", ".ext")]
        public void CanGetPathParts(string path, string directory, string fileName, string extension)
        {
            Assert.Equal(directory, Path.GetDirectoryName(path));
            Assert.Equal(fileName, Path.GetFileNameWithoutExtension(path));
            Assert.Equal(extension, Path.GetExtension(path));
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
