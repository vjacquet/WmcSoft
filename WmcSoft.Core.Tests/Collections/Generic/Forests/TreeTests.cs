using System;
using System.Linq;
using Xunit;

namespace WmcSoft.Collections.Generic.Forests
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

            tree.Root.Add(1);
            Assert.Equal(2, tree.Height);
            Assert.NotNull(tree.Root.FirstChild);
            Assert.Equal(1, tree.Root.FirstChild.Value);
        }

        [Fact]
        public void CanCreateComplexTree()
        {
            var tree = new Tree<int>();
            Assert.Equal(0, tree.Height);

            tree.Root = new TreeNode<int>(1
                , new TreeNode<int>(2
                    , new TreeNode<int>(3)
                    , new TreeNode<int>(4))
                , new TreeNode<int>(5));
            Assert.Equal(3, tree.Height);
            Assert.Equal(5, tree.Weight);
        }

        [Fact]
        public void CanTraverseComplexTree()
        {
            var tree = new Tree<char>();
            Assert.Equal(0, tree.Height);

            tree.Root = new TreeNode<char>('F'
                , new TreeNode<char>('B'
                    , new TreeNode<char>('A')
                    , new TreeNode<char>('D'
                        , new TreeNode<char>('C')
                        , new TreeNode<char>('E')
                    )
                )
                , new TreeNode<char>('G'
                    , new TreeNode<char>('I'
                        , new TreeNode<char>('H')
                    )
                )
            );
            Assert.Equal(4, tree.Height);
            Assert.Equal(9, tree.Weight);
            Assert.Equal("FBADCEGIH", tree.Enumerate(DepthFirst.PreOrder));
            Assert.Equal("ACEDBHIGF", tree.Enumerate(DepthFirst.PostOrder));
        }

        [Fact]
        public void EnumerateOnNullNodeThrowsNullReferenceException()
        {
            TreeNode<int> node = null;
            Assert.Throws<NullReferenceException>(() => node.Enumerate(DepthFirst.PreOrder));
        }

        [Fact]
        public void CanAppendChildren()
        {
            var tree = new Tree<int>();
            tree.Root = new TreeNode<int>(5);
            tree.Root.Add(1);
            tree.Root.Add(2);
            tree.Root.Add(3);
            tree.Root.Add(4);

            var expected = new[] { 1, 2, 3, 4 };
            var actual = tree.Root.Children.Select(n => n.Value).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRemoveChild()
        {
            var tree = new Tree<int>();
            tree.Root = new TreeNode<int>(5);
            tree.Root.Add(1);
            tree.Root.Add(2);
            tree.Root.Add(3);
            tree.Root.Add(4);

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
            tree.Root.Add(1);
            tree.Root.Add(2);
            tree.Root.Add(3);
            tree.Root.Add(4);

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
            tree.Root.Add(1);
            tree.Root.Add(2);
            tree.Root.Add(3);
            tree.Root.Add(4);

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
