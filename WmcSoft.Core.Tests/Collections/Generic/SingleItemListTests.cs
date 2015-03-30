using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic.Tests
{
    [TestClass]
    public class SingleItemListTests
    {
        [TestMethod]
        public void CheckIndexer() {
            var list = new List<int>();
            var i = list[1];
        }
    }
}
