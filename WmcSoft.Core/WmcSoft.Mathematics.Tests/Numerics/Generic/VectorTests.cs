using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Vect = WmcSoft.Numerics.Generic.Vector<int, WmcSoft.Arithmetics.Int32Arithmetics>;

namespace WmcSoft.Numerics.Generic.Tests
{
    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void CheckAdd() {
            var u = new Vect(1, 2, 3);
            var v = new Vect(1, 2, 3);
            var expected = new Vect(2, 4, 6);
            Assert.AreEqual(expected, u + v);
        }

        [TestMethod]
        public void CheckDotProduct() {
            var u = new Vect(1, 2, 3);
            var v = new Vect(1, 2, 3);
            var expected = 14d;
            Assert.AreEqual(expected, Vect.DotProduct(u, v));
        }
    }
}
