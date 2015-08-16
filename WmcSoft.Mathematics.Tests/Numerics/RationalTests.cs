﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Numerics.Tests
{
    [TestClass]
    public class RationalTests
    {
        [TestMethod]
        public void CanAdd() {
            var x = new Rational(1, 2);
            var y = new Rational(1, 3);

            Assert.AreEqual(new Rational(5, 6), x + y);
        }

        [TestMethod]
        public void CanSubtract() {
            var x = new Rational(1, 2);
            var y = new Rational(1, 3);

            Assert.AreEqual(new Rational(1, 6), x - y);
        }

        [TestMethod]
        public void CanMultiply() {
            var x = new Rational(1, 2);
            var y = new Rational(1, 3);

            Assert.AreEqual(new Rational(1, 6), x * y);
        }

        [TestMethod]
        public void CanDivide() {
            var x = new Rational(1, 2);
            var y = new Rational(1, 3);

            Assert.AreEqual(new Rational(3, 2), x / y);
        }

        [TestMethod]
        public void CanCastToInt() {
            var q = new Rational(4, 2);

            Assert.AreEqual(2, (int)q);
        }

        [TestMethod]
        public void CanSimplify() {
            var x = new Rational(2, 6);
            var y = new Rational(1, 6);

            Assert.AreEqual(new Rational(1, 3), x.Simplify());
        }
    }
}
