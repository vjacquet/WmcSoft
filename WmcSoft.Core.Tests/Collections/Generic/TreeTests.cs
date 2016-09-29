using System;
using System.Linq;
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

        [TestMethod]
        public void CanAppendChildren() {
            var tree = new Tree<int>();
            tree.Root = new TreeNode<int>(5);
            tree.Root.Append(1);
            tree.Root.Append(2);
            tree.Root.Append(3);
            tree.Root.Append(4);

            var expected = new[] { 1, 2, 3, 4 };
            var actual = tree.Root.Children.Select(n => n.Value).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanPrependChildren() {
            var tree = new Tree<int>();
            tree.Root = new TreeNode<int>(5);
            tree.Root.Preprend(1);
            tree.Root.Preprend(2);
            tree.Root.Preprend(3);
            tree.Root.Preprend(4);

            var expected = new[] { 4, 3, 2, 1 };
            var actual = tree.Root.Children.Select(n => n.Value).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
