using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class OrdinalTests
    {
        [TestMethod]
        public void CheckInt32Ordinal() {
            var ordinal = new Int32Ordinal();

            ContractAssert.Ordinal(ordinal, 2, 5, 3);
        }

        [TestMethod]
        public void ChechYearOrdinal() {
            YearOrdinal ordinal;

            var expected = new DateTime(2017, 02, 28);
            var actual = ordinal.Advance(new DateTime(2016, 02, 29), 1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckSequenceOrdinal() {
            var ordinal = new SequenceOrdinal<char>('a', 'e', 'i', 'o', 'u', 'y');
            ContractAssert.Ordinal(ordinal, 'a', 'u', 4);
        }
    }
}
