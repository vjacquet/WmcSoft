﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Numerics.Tests
{
    [TestClass]
    public class Vector3Tests
    {
        [TestMethod]
        public void CheckAdd() {
            var u = new Vector3(1, 2, 3);
            var v = new Vector3(1, 2, 3);
            var expected = new Vector3(2, 4, 6);
            Assert.AreEqual(expected, u + v);
        }

        [TestMethod]
        public void CheckDotProduct() {
            var u = new Vector3(1, 2, 3);
            var v = new Vector3(1, 2, 3);
            var expected = 14d;
            Assert.AreEqual(expected, Vector3.DotProduct(u, v));
        }

        [TestMethod]
        public void CheckConvert() {
            var v = new Vector3(1, 2, 3);
            var expected = new double[] { 1, 2, 3};
            var actual = (double[])v;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckToString() {
            var v = new Vector3(1, 2, 3);
            var expected = "[1  2  3]";
            var actual = v.ToString("N0", null);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanEnumerate() {
            var v = new Vector3(1, 2, 3);
            var expected = new double[] { 1, 2, 3 };
            var actual = v.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanMultiplyWithMatrix() {
            var v = new Vector3(1, 2, 3);
            var m = new Matrix3(new double[,] {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9},
            });
            var actual = m * v;
            var expected = new Vector3(14, 32, 50);
            Assert.AreEqual(expected, actual);
        }
    }
}