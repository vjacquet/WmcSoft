using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic.Accumulators
{
    [TestClass]
    public class KahanAdderTests
    {
        [TestMethod]
        public void CheckKahanAdder()
        {
            var values = new[] { 10000.0d, 3.14159d, 2.71828d };
            var adder = new KahanAdder();
            var actual = 0d;
            for (int i = 0; i < values.Length; i++) {
                actual = adder.Accumulate(actual, values[i]);
            }

            var expected = 10005.85987d;
            Assert.AreEqual(expected, actual);

            var naive = values.Sum();
            Assert.AreNotEqual(naive, actual);
        }
    }
}
