using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Specialized
{
    [TestClass]
    public class SparseArrayTests
    {
        [TestMethod]
        public void CheckExtent()
        {
            var array = new SparseArray<string>(100, "?") {
                [24] = "24",
                [16] = "16",
                [33] = "33",
                [64] = "64",
                [48] = "48"
            };
            var actual = array.Extent;
            var expected = Pair.Create(16, 64);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckSpareArray()
        {
            var array = new SparseArray<string>(100, "?") {
                [24] = "24",
                [16] = "16",
                [33] = "33",
                [64] = "64",
                [48] = "48"
            };
            Assert.AreEqual(100, array.Count);
            Assert.AreEqual("33", array[33]);
            Assert.AreEqual("?", array[27]);
            Assert.AreEqual("?", array[99]);
        }

        [TestMethod]
        public void CheckIndexOf()
        {
            var array = new SparseArray<string>(100, "?") {
                [24] = "24",
                [16] = "16",
                [33] = "33",
                [64] = "64",
                [48] = "48"
            };
            Assert.AreEqual(24, array.IndexOf("24"));
            Assert.AreEqual(16, array.IndexOf("16"));
            Assert.AreEqual(64, array.IndexOf("64"));
            Assert.AreEqual(0, array.IndexOf("?"));
            array[0] = "0";
            array[1] = "1";
            array[2] = "2";
            Assert.AreEqual(3, array.IndexOf("?"));
            Assert.AreEqual(-1, array.IndexOf("missing"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CheckIndexOutOfBounds()
        {
            var array = new SparseArray<string>(100, "?") {
                [24] = "24",
                [16] = "16",
                [33] = "33",
                [64] = "64",
                [48] = "48"
            };
            Assert.AreEqual("?", array[99]);
            Assert.AreEqual("?", array[100]);

            Assert.Inconclusive();
        }
    }
}
