using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Tests
{
    [TestClass]
    public class GeneratorsTests
    {
        [TestMethod]
        public void CheckUnarySeries() {
            var series = Generators.Series(x => x + 2, 0).GetEnumerator();
            Assert.AreEqual(0, series.Read());
            Assert.AreEqual(2, series.Read());
            Assert.AreEqual(4, series.Read());
            Assert.AreEqual(6, series.Read());
        }

        [TestMethod]
        public void CheckBinarySeries() {
            var series = Generators.Series((x, y) => x + y, 0, 1).GetEnumerator();
            Assert.AreEqual(0, series.Read());
            Assert.AreEqual(1, series.Read());
            Assert.AreEqual(1, series.Read());
            Assert.AreEqual(2, series.Read());
            Assert.AreEqual(3, series.Read());
            Assert.AreEqual(5, series.Read());
            Assert.AreEqual(8, series.Read());
            Assert.AreEqual(13, series.Read());
            Assert.AreEqual(21, series.Read());
        }

    }
}
