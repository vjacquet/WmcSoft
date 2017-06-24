using Xunit;

namespace WmcSoft.Collections.Specialized
{
    public class FenwickTreeTests
    {
        [Fact]
        public void CanAddToFenwickTree()
        {
            var tree = new FenwickTree(4);
            tree.Add(0, 2);
            tree.Add(1, 4);
            tree.Add(2, 8);
            tree.Add(3, 16);

            Assert.Equal(tree[0], 2);
            Assert.Equal(tree[1], 4);
            Assert.Equal(tree[2], 8);
            Assert.Equal(tree[3], 16);

            Assert.Equal(30, tree.Tally());
        }

        [Fact]
        public void CanInitializeFenwickTree()
        {
            var tree = new FenwickTree(2, 4, 8, 16);

            Assert.Equal(tree[0], 2);
            Assert.Equal(tree[1], 4);
            Assert.Equal(tree[2], 8);
            Assert.Equal(tree[3], 16);

            Assert.Equal(30, tree.Tally());
        }

        [Fact]
        public void CanSumFenwickTree()
        {
            var tree = new FenwickTree(2, 4, 8, 16);
            Assert.Equal(6, tree.Sum(2));
        }

        [Fact]
        public void CanRangeFenwickTree()
        {
            var tree = new FenwickTree(2, 4, 8, 16);
            Assert.Equal(12, tree.Range(1, 3));
        }
    }
}
