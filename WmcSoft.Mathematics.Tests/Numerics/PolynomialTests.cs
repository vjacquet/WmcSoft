using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Numerics.Tests
{
    [TestClass]
    public class PolynomialTests
    {
        [TestMethod]
        public void CanConstructPolynomial() {
            var actual = new Polynomial(1, 0, 2, 4);
            var expected = "x^3 + 2x + 4";
            Assert.AreEqual(expected, actual.ToString());
        }

        [TestMethod]
        public void CanConstructPolynomialWithLeadingZeros() {
            var actual = new Polynomial(0, 0, 1, 0, 2, 4);
            var expected = "x^3 + 2x + 4";
            Assert.AreEqual(expected, actual.ToString());
        }

        [TestMethod]
        public void CanAddPolynomials() {
            var x = new Polynomial(1, 0, 2, 4);
            var y = new Polynomial(2, 0);
            var actual = x + y;
            var expected = "x^3 + 4x + 4";
            Assert.AreEqual(expected, actual.ToString());
        }

        [TestMethod]
        public void CanMultiplyPolynomials() {
            var x = new Polynomial(1, 2, 3, 4, 5);
            var y = new Polynomial(1, -2, 1);
            var actual = x * y;
            var expected = "x^6 - 6x + 5";
            Assert.AreEqual(expected, actual.ToString());
        }

    }
}
