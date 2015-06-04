using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class SingleItemListTests
    {
        [TestMethod]
        public void CheckIndexer() {
            var list = new SingleItemList<int>(5);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(5, list[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CheckInsert() {
            var list = new SingleItemList<int>(5);
            list.Insert(1, 3);
        }

        [TestMethod]
        public void CheckInsertOnEmpty() {
            var list = new SingleItemList<int>();
            list.Insert(0, 3);
        }
    }
}
