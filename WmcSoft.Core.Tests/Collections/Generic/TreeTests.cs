using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class TreeTests
    {
        [TestMethod]
        public void CanCreateTree() {
            var tree = new Tree<int>();
            Assert.AreEqual(0, tree.Height);

            tree.Root = new TreeNode<int>(5);
            Assert.AreEqual(1, tree.Height);

            tree.Root.Append(1);
            Assert.AreEqual(2, tree.Height);
            Assert.IsNotNull(tree.Root.FirstChild);
            Assert.AreEqual(1, tree.Root.FirstChild.Value);
        }
    }
}
