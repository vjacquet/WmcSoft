using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class OrdinalExtensionsTests
    {
        [TestMethod]
        public void CanCollate() {
            var sequence = new[] { 1, 2, 5, 7, 8, 9 };
            Int32Ordinal ordinal;

            var actual = sequence.Collate(ordinal, Tuple.Create).ToArray();
            var expected = new[] { Tuple.Create(1, 2), Tuple.Create(5, 5), Tuple.Create(7, 9) };

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void CanCollateWithSingleElementAtEnd() {
            var sequence = new[] { 1, 2, 5, 7, 8, 9, 11 };
            Int32Ordinal ordinal;

            var actual = sequence.Collate(ordinal, Tuple.Create).ToArray();
            var expected = new[] { Tuple.Create(1, 2), Tuple.Create(5, 5), Tuple.Create(7, 9), Tuple.Create(11, 11) };

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void CanCollateOneElement() {
            var sequence = new[] { 1 };
            Int32Ordinal ordinal;

            var actual = sequence.Collate(ordinal, Tuple.Create).ToArray();
            var expected = new[] { Tuple.Create(1, 1) };

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void CanCollateOneRange() {
            var sequence = new[] { 1, 2, 3 };
            Int32Ordinal ordinal;

            var actual = sequence.Collate(ordinal, Tuple.Create).ToArray();
            var expected = new[] { Tuple.Create(1, 3) };

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void ChechYearOrdinal() {
            YearOrdinal ordinal;

            var expected = new DateTime(2017, 02, 28);
            var actual = ordinal.Advance(new DateTime(2016, 02, 29), 1);
            Assert.AreEqual(expected, actual);
        }
    }
}
