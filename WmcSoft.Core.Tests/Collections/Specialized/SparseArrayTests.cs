using System;
using Xunit;

namespace WmcSoft.Collections.Specialized
{
    public class SparseArrayTests
    {
        [Fact]
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
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckSpareArray()
        {
            var array = new SparseArray<string>(100, "?") {
                [24] = "24",
                [16] = "16",
                [33] = "33",
                [64] = "64",
                [48] = "48"
            };
            Assert.Equal(100, array.Count);
            Assert.Equal("33", array[33]);
            Assert.Equal("?", array[27]);
            Assert.Equal("?", array[99]);
        }

        [Fact]
        public void CheckIndexOf()
        {
            var array = new SparseArray<string>(100, "?") {
                [24] = "24",
                [16] = "16",
                [33] = "33",
                [64] = "64",
                [48] = "48"
            };
            Assert.Equal(24, array.IndexOf("24"));
            Assert.Equal(16, array.IndexOf("16"));
            Assert.Equal(64, array.IndexOf("64"));
            Assert.Equal(0, array.IndexOf("?"));
            array[0] = "0";
            array[1] = "1";
            array[2] = "2";
            Assert.Equal(3, array.IndexOf("?"));
            Assert.Equal(-1, array.IndexOf("missing"));
        }

        [Fact]
        public void CheckIndexOutOfBounds()
        {
            var array = new SparseArray<string>(100, "?") {
                [24] = "24",
                [16] = "16",
                [33] = "33",
                [64] = "64",
                [48] = "48"
            };
            Assert.Equal("?", array[99]);
            Assert.Throws<ArgumentOutOfRangeException>(() => array[100]);
        }
    }
}