using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic.Tests
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        public void CheckQuorum() {
            var list = new[] { 1, 2, 3, 4, 5, 6, 7 };
            Assert.IsTrue(list.Quorum(3, i => (i & 1) == 1));
        }

        [TestMethod]
        public void CheckElectedOrDefaultWithPredicate() {
            string[] array;

            array = new[] { "A", "B", null, "", "A", "C", "B", "A" };
            Assert.AreEqual("A", array.ElectedOrDefault(s => !String.IsNullOrEmpty(s)));

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B", "B" };
            Assert.AreEqual("B", array.ElectedOrDefault(s => !String.IsNullOrEmpty(s)));

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B" };
            Assert.AreEqual("A", array.ElectedOrDefault(s => !String.IsNullOrEmpty(s)));
        }

        [TestMethod]
        public void CheckElectedOrDefaultWithoutPredicate() {
            string[] array;

            array = new[] { "A", "B", null, "", "A", "C", "B", "A" };
            Assert.AreEqual("A", array.ElectedOrDefault());

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B", "B" };
            Assert.AreEqual("B", array.ElectedOrDefault());

            array = new[] { "A", "B", null, "", "A", "C", "B", "A", "B" };
            Assert.AreEqual("A", array.ElectedOrDefault());
        }
    }
}
