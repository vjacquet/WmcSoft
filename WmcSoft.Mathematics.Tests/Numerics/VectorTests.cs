﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Numerics.Tests
{
    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void CheckAdd() {
            var u = new Vector(1, 2, 3);
            var v = new Vector(1, 2, 3);
            var expected = new Vector(2, 4, 6);
            Assert.AreEqual(expected, u + v);
        }

        [TestMethod]
        public void CheckDotProduct() {
            var u = new Vector(1, 2, 3);
            var v = new Vector(1, 2, 3);
            var expected = 14d;
            Assert.AreEqual(expected, Vector.DotProduct(u, v));
        }

        [TestMethod]
        public void CheckConvert() {
            var v = new Vector(1, 2, 3);
            var expected = new double[] { 1, 2, 3};
            var actual = (double[])v;
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
