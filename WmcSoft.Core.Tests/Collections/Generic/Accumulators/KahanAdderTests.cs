using System.Linq;
using Xunit;

namespace WmcSoft.Collections.Generic.Accumulators
{
    public class KahanAdderTests
    {
        [Fact]
        public void CheckKahanAdder()
        {
            var values = new[] { 10000.0d, 3.14159d, 2.71828d };
            var adder = new KahanAdder();
            var actual = 0d;
            for (int i = 0; i < values.Length; i++) {
                actual = adder.Accumulate(actual, values[i]);
            }

            var expected = 10005.85987d;
            Assert.Equal(expected, actual);

            var naive = values.Sum();
            Assert.NotEqual(naive, actual);
        }
    }
}