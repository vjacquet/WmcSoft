﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections.Generic;

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
            var actual = m.Transpose();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanGetColumn() {
            var m = new Matrix(new double[,] {
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
            var m = new Matrix(new double[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            });

            var expected = new double[] { 4, 5, 6 };
            var actual = m.Row(1).ToArray(3);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
