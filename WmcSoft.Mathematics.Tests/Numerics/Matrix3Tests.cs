using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Numerics.Tests
{
    [TestClass]
    public class Matrix3Tests
    {
        [TestMethod]
        public void CanConvert() {
            var expected = new double[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            };
            var m = new Matrix3(expected);
            var actual = (double[,])m;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIdentity() {
            var actual = Matrix3.Identity;
            var expected = new Matrix3(new double[,] {
                {1, 0, 0},
                {0, 1, 0},
                {0, 0, 1},
            });
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanTranspose() {
            var m = new Matrix3(new double[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            });

            var expected = new Matrix3(new double[,] {
                {1, 4, 7},
                {2, 5, 8},
                {3, 6, 9},
            });
            var actual = m.Transpose();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanGetColumn() {
            var m = new Matrix3(new double[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            });

            var expected = new double[] { 2, 5, 8 };
            var actual = m.Column(1).ToArray(3);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanGetRow() {
            var m = new Matrix3(new double[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            });

            var expected = new double[] { 4, 5, 6 };
            var actual = m.Row(1).ToArray(3);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanAddSquareMatrices() {
            var m = new Matrix3(new double[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            });

            var expected = new Matrix3(new double[,] {
                { 2,  4,  6},
                { 8, 10, 12},
                {14, 16, 18},
            });
            var actual = m + m;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanSubtractSquareMatrices() {
            var m = new Matrix3(new double[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            });

            var expected = Matrix3.Zero;
            var actual = m - m;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanMultiplySquareMatrices() {
            var x = new Matrix3(new double[,] {
                {1, 2, 3},
                {0, 1, 2},
                {0, 0, 1},
            });
            var y = new Matrix3(new double[,] {
                {1, 0, 0},
                {2, 1, 0},
                {3, 2, 1},
            });

            var expected = new Matrix3(new double[,] {
                {14, 8, 3},
                { 8, 5, 2},
                { 3, 2, 1},
            });
            var actual = x * y;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanMultiplyMatrices() {
            var x = new Matrix3(new double[,] {
                {1, 0, 0},
                {2, 1, 0},
                {3, 2, 1},
            });
            var y = new Matrix3(new double[,] {
                {1, 2, 3},
                {0, 1, 2},
                {0, 0, 1},
            });

            var expected = new Matrix3(new double[,] {
                {1, 2, 3},
                {2, 5, 8},
                {3, 8, 14},
            });
            var actual = x * y;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanComputeDet() {
            var x = new Matrix3(new double[,] {
                {1, 2, 3},
                {0, 1, 4},
                {5, 6, 0},
            });
            var expected = 1d;
            var actual = x.Det();
            Assert.AreEqual(expected, actual);
        }
    }
}