using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Specialized
{
    [TestClass]
    public class FenwickTreeTests
    {
        [TestMethod]
        public void CanAddToFenwickTree()
        {
            var tree = new FenwickTree(4);
            tree.Add(0, 2);
            tree.Add(1, 4);
            tree.Add(2, 8);
            tree.Add(3, 16);

            Assert.AreEqual(tree[0], 2);
            Assert.AreEqual(tree[1], 4);
            Assert.AreEqual(tree[2], 8);
            Assert.AreEqual(tree[3], 16);

            Assert.AreEqual(30, tree.Tally());
        }

        [TestMethod]
        public void CanInitializeFenwickTree()
        {
            var tree = new FenwickTree(2, 4, 8, 16);

            Assert.AreEqual(tree[0], 2);
            Assert.AreEqual(tree[1], 4);
            Assert.AreEqual(tree[2], 8);
            Assert.AreEqual(tree[3], 16);

            Assert.AreEqual(30, tree.Tally());
        }
    }
}
