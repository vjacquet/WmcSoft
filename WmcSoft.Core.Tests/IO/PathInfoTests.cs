using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.IO
{
    [TestClass]
    public class PathInfoTests
    {
        [TestMethod]
        public void CheckUnrootedPathIsNull() {
            PathInfo p = "path/to/file";
            var root = p.Root;
            Assert.IsNull(root);
        }

        [TestMethod]
        public void CheckRootedPathIsNotNull() {
            PathInfo p = @"c:\path\to\file";
            var root = p.Root;
            Assert.AreEqual(@"c:\", root);
        }

        [TestMethod]
        public void CheckFileName() {
            PathInfo p = "path/to/file";
            var fileName = p.FileName;
            Assert.AreEqual("file", fileName);
        }

        [TestMethod]
        public void CheckExtension() {
            PathInfo p = "path/to/file.ext";
            var extension = p.Extension;
            Assert.AreEqual(".ext", extension);
        }

        [TestMethod]
        public void CheckExtensionlessExtensionIsNull() {
            PathInfo p = "path/to/file";
            var extension = p.Extension;
            Assert.IsNull(extension);
        }

        [TestMethod]
        public void CheckCombineOperator() {
            PathInfo p = @"c:\path";
            Assert.AreEqual(@"c:\path\to\file", p / "to" / "file");
        }
    }
}
