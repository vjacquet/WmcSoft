using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class IndexTests
    {
        [TestMethod]
        public void CanAddAndRemoveToIndex()
        {
            var index = new Index<int, string>();
            index.Add(1, "one");
            index.Add(1, "un");
            Assert.AreEqual(2, index.Count);

            index.Add(2, "two");

            Assert.AreEqual(3, index.Count);
            Assert.AreEqual("one un", index[1].JoinWith(' '));

            index.Remove(1, "one");
            Assert.AreEqual(2, index.Count);
            Assert.AreEqual("un", index[1].JoinWith(' '));

            index.Remove(1, "un");
            Assert.AreEqual("", index[1].JoinWith(' '));
        }

        [TestMethod]
        public void CanManipulateAsLookup()
        {
            var index = new Index<int, string> {
                { 1, "one" },
                { 1, "un" },
                { 2, "two" }
            };

            var lookup = index.AsLookup();
            Assert.AreEqual(3, lookup.Count);
            Assert.IsTrue(lookup.Contains(2));
            CollectionAssert.AreEqual(new[] { "one", "un" }, lookup[1].ToArray());
            CollectionAssert.AreEqual(new[] { "two" }, lookup[2].ToArray());
            CollectionAssert.AreEqual(new string[0], lookup[0].ToArray());
            var groups = lookup.ToList();

            Assert.AreEqual(1, groups[0].Key);
        }
    }
}
