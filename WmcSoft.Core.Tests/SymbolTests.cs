using System;
using System.IO;
using System.Runtime.Serialization;
using Xunit;
using WmcSoft.IO;

using Cloner = System.Runtime.Serialization.Formatters.Binary.BinaryFormatter;

namespace WmcSoft
{
    public class SymbolTests
    {
        [Fact]
        public void CanEqualsSymbols()
        {
            Symbol<int> x = "symbol1";
            Symbol<int> y = "symbol" + 1;

            Assert.Equal(x, y);
        }

        [Fact]
        public void CannotCompareDifferentSymbols()
        {
            Symbol<int> x = "symbol";
            Symbol<long> y = "symbol";

            Assert.NotEqual<object>(x, y);
            Assert.NotEqual<object>(y, x);
            Assert.Equal((string)x, (string)y);
        }

        [Fact]
        public void CanRoundTripSymbol()
        {
            using (var ms = new MemoryStream()) {
                Symbol<int> x = "symbol";

                var f = new Cloner(null, new StreamingContext(StreamingContextStates.Clone));
                f.Serialize(ms, x);
                ms.Rewind();
                var y = (Symbol<int>)f.Deserialize(ms);

                Assert.Equal(x, y);
            }
        }
    }
}