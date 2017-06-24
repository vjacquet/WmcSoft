using Xunit;

namespace WmcSoft.IO
{
    public class PathInfoTests
    {
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
