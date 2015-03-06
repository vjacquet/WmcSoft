using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Numerics.Tests
{
    [TestClass]
    public class MatrixTests
    {
        [TestMethod]
        public void CanTranspose() {
            var m = new Matrix(new double[,] {
                {1, 2},
                {3, 4},
                {5, 6},
            });

            var expected = new Matrix(new double[,] {
                {1, 3, 5},
                {2, 4, 6},
            });

            Assert.AreEqual(expected, m.Transpose());
        }
    }
}
