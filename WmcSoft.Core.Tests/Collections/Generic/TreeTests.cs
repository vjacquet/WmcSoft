using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class TreeTests
    {
        [Fact]
        public void CanCreateTree()
        {
            var tree = new Tree<int>();
            Assert.Equal(0, tree.Height);

            tree.Root = new TreeNode<int>(5);
            Assert.Equal(1, tree.Height);

            tree.Root.Append(1);
            Assert.Equal(2, tree.Height);
            Assert.NotNull(tree.Root.FirstChild);
            Assert.Equal(1, tree.Root.FirstChild.Value);
        }

        [Fact]
        public void CanAppendChildren()
        {
            var tree = new Tree<int>();
            tree.Root = new TreeNode<int>(5);
            tree.Root.Append(1);
            tree.Root.Append(2);
            tree.Root.Append(3);
            tree.Root.Append(4);

            var expected = new[] { 1, 2, 3, 4 };
            var actual = tree.Root.Children.Select(n => n.Value).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanPrependChildren()
        {
            var tree = new Tree<int>();
            tree.Root = new TreeNode<int>(5);
            tree.Root.Preprend(1);
            tree.Root.Preprend(2);
            tree.Root.Preprend(3);
            tree.Root.Preprend(4);

            var expected = new[] { 4, 3, 2, 1 };
            var actual = tree.Root.Children.Select(n => n.Value).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRemoveChild()
        {
            var tree = new Tree<int>();
            tree.Root = new TreeNode<int>(5);
            tree.Root.Append(1);
            tree.Root.Append(2);
            tree.Root.Append(3);
            tree.Root.Append(4);

            Assert.False(tree.Root.Remove(5));
            Assert.True(tree.Root.Remove(2));

            var expected = new[] { 1, 3, 4 };
            var actual = tree.Root.Children.Select(n => n.Value).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRemoveFirstChild()
        {
            var tree = new Tree<int>();
            tree.Root = new TreeNode<int>(5);
            tree.Root.Append(1);
            tree.Root.Append(2);
            tree.Root.Append(3);
            tree.Root.Append(4);

            Assert.True(tree.Root.Remove(1));

            var expected = new[] { 2, 3, 4 };
            var actual = tree.Root.Children.Select(n => n.Value).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRemoveChildren()
        {
            var tree = new Tree<int>();
            tree.Root = new TreeNode<int>(5);
            tree.Root.Append(1);
            tree.Root.Append(2);
            tree.Root.Append(3);
            tree.Root.Append(4);

            Assert.True(tree.Root.Remove(2));
            Assert.True(tree.Root.Remove(4));
            Assert.True(tree.Root.Remove(3));
            Assert.True(tree.Root.Remove(1));

            var expected = new int[0];
            var actual = tree.Root.Children.Select(n => n.Value).ToArray();
            Assert.Equal(expected, actual);
        }
    }
}
