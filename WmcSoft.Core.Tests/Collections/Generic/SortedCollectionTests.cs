using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class SortedCollectionTests
    {
        [TestMethod]
        public void CanAdd() {
            var collection = new SortedCollection<char>();
            collection.AddRange(new []{'b', 'a', 'c'});
            var expected = "abc";
            var actual = String.Concat(collection);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanAddWithCollision() {
            var collection = new SortedCollection<string>(StringComparer.InvariantCultureIgnoreCase);
            collection.AddRange(new[] { "b", "a", "c", "B" });
            var expected = "abBc";
            var actual = String.Concat(collection);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanRemove() {
            var collection = new SortedCollection<char>();
            collection.AddRange(new[] { 'b', 'a', 'c' });
            Assert.IsTrue(collection.Remove('a'));
            var expected = "bc";
            var actual = String.Concat(collection);
            Assert.AreEqual(expected, actual);
            Assert.IsFalse(collection.Remove('a'));
        }

    }
}
