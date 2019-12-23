using System.Linq;
using Xunit;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Numerics
{
    public class MatrixTests
    {
        [Fact]
        public void CanConvert()
        {
            var expected = new double[,] {
                {1, 2},
                {3, 4},
                {5, 6},
            };
            var m = new Matrix(expected);
            var actual = (double[,])m;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckIdentity()
        {
            var actual = Matrix.Identity(3);
            var expected = new Matrix(new double[,] {
                {1, 0, 0},
                {0, 1, 0},
                {0, 0, 1},
            });
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanTranspose()
        {
            var m = new Matrix(new double[,] {
                {1, 2},
                {3, 4},
                {5, 6},
            });

            var expected = new Matrix(new double[,] {
                {1, 3, 5},
                {2, 4, 6},
            });
            var actual = m.Transpose();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanGetColumn()
        {
            var m = new Matrix(new double[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            });

            var expected = new double[] { 2, 5, 8 };
            var actual = m.Column(1).ToArray(3);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanGetRow()
        {
            var m = new Matrix(new double[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            });

            var expected = new double[] { 4, 5, 6 };
            var actual = m.Row(1).ToArray(3);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddSquareMatrices()
        {
            var m = new Matrix(new double[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            });

            var expected = new Matrix(new double[,] {
                { 2,  4,  6},
                { 8, 10, 12},
                {14, 16, 18},
            });
            var actual = m + m;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanSubtractSquareMatrices()
        {
            var m = new Matrix(new double[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            });

            var expected = new Matrix(3);
            var actual = m - m;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanMultiplySquareMatrices()
        {
            var x = new Matrix(new double[,] {
                {1, 2, 3},
                {0, 1, 2},
                {0, 0, 1},
            });
            var y = new Matrix(new double[,] {
                {1, 0, 0},
                {2, 1, 0},
                {3, 2, 1},
            });

            var expected = new Matrix(new double[,] {
                {14, 8, 3},
                { 8, 5, 2},
                { 3, 2, 1},
            });
            var actual = x * y;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanMultiplyMatrices()
        {
            var x = new Matrix(new double[,] {
                {1, 0, 0},
                {2, 1, 0},
            });
            var y = new Matrix(new double[,] {
                {1, 2},
                {0, 1},
                {0, 0},
            });

            var expected = new Matrix(new double[,] {
                {1, 2},
                {2, 5},
            });
            var actual = x * y;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanPowerMatrice()
        {
            var m = Matrix.Identity(4) * 2;
            var actual = Matrix.Power(m, 5);

            var expected = Matrix.Identity(4) * (2 << 4);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanInverse3x3()
        {
            var x = new Matrix(new double[,] {
                {1, 2, 3},
                {0, 1, 4},
                {5, 6, 0},
            });
            var expected = new Matrix(new double[,] {
                {-24,  18,  5},
                { 20, -15, -4},
                { -5,   4,  1},
            });
            var actual = x.Inverse();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanInverse4x4()
        {
            var x = new Matrix(new double[,] {
                {-1,  0,  1,  1},
                { 1, -2,  1, -1},
                { 1,  0, -1,  1},
                { 1,  0,  1, -1},
            });
            var expected = new Matrix(new double[,] {
                { 0.0d,  0.0d,  0.5d,  0.5d},
                { 0.0d, -0.5d,  0.0d,  0.5d},
                { 0.5d,  0.0d,  0.0d,  0.5d},
                { 0.5d,  0.0d,  0.5d,  0.0d},
            });
            var actual = x.Inverse();
            Assert.Equal(expected, actual);
        }
    }
}
