using System;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.IO;

using Cloner = System.Runtime.Serialization.Formatters.Binary.BinaryFormatter;

namespace WmcSoft
{
    [TestClass]
    public class SymbolTests
    {
        [TestMethod]
        public void CanEqualsSymbols() {
            Symbol<int> x = "symbol1";
            Symbol<int> y = "symbol" + 1;

            Assert.AreEqual(x, y);
        }

        [TestMethod]
        public void CannotCompareDifferentSymbols() {
            Symbol<int> x = "symbol";
            Symbol<long> y = "symbol";

            Assert.AreNotEqual(x, y);
            Assert.AreEqual((string)x, (string)y);
        }

        [TestMethod]
        public void CanRoundTripSymbol() {
            using (var ms = new MemoryStream()) {
                Symbol<int> x = "symbol";

                var f = new Cloner(null, new StreamingContext(StreamingContextStates.Clone));
                f.Serialize(ms, x);
                ms.Rewind();
                var y = (Symbol<int>)f.Deserialize(ms);

                Assert.AreEqual(x, y);
            }
        }
    }
}
