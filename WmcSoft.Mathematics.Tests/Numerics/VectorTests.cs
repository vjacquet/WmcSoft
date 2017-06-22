using System.Linq;
using Xunit;

namespace WmcSoft.Numerics.Tests
{
    public class VectorTests
    {
        [Fact]
        public void CheckAdd()
        {
            var u = new Vector(1, 2, 3);
            var v = new Vector(1, 2, 3);
            var expected = new Vector(2, 4, 6);
            Assert.Equal(expected, u + v);
        }

        [Fact]
        public void CheckOperatorEqual()
        {
            var u = new Vector(1, 2, 3);
            var v = new Vector(1, 2, 3);
            Assert.True(u == v);
        }

        [Fact]
        public void CheckDotProduct()
        {
            var u = new Vector(1, 2, 3);
            var v = new Vector(1, 2, 3);
            var expected = 14d;
            Assert.Equal(expected, Vector.DotProduct(u, v));
        }

        [Fact]
        public void CheckConvert()
        {
            var v = new Vector(1, 2, 3);
            var expected = new double[] { 1, 2, 3 };
            var actual = (double[])v;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckToString()
        {
            var v = new Vector(1, 2, 3);
            var expected = "[1  2  3]";
            var actual = v.ToString("N0", null);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEnumerate()
        {
            var v = new Vector(1, 2, 3);
            var expected = new double[] { 1, 2, 3 };
            var actual = v.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanMultiplyWithMatrix()
        {
            var v = new Vector(1, 2, 3);
            var m = new Matrix(new double[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            });
            var actual = m * v;
            var expected = new Vector(14, 32, 50);
            Assert.Equal(expected, actual);
        }
    }
}