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

            Assert.Equal(2, tree[0]);
            Assert.Equal(4, tree[1]);
            Assert.Equal(8, tree[2]);
            Assert.Equal(16, tree[3]);

            Assert.Equal(30, tree.Tally());
        }

        [Fact]
        public void CanInitializeFenwickTree()
        {
            var tree = new FenwickTree(2, 4, 8, 16);

            Assert.Equal(2, tree[0]);
            Assert.Equal(4, tree[1]);
            Assert.Equal(8, tree[2]);
            Assert.Equal(16, tree[3]);

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
