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
        public void CheckSequenceOrdinal() {
            var ordinal = new SequenceOrdinal<char>('a', 'e', 'i', 'o', 'u', 'y');
            ContractAssert.Ordinal(ordinal, 'a', 'u', 4);
        }
    }
}
