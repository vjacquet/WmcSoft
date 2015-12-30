using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Arithmetics
{
    [TestClass]
    public class IntegralCalculusTests
    {
        [TestMethod]
        public void CanIntegrateLinearFunctionWithMidOrdinateRule() {
            var rule = new MidOrdinateRule(100);
            var f = new GenericFunction<double>(x => 1d / x);
            var actual = f.Integrate(rule, 1, 2);
            var expected = 0.6931440556283d;
            Assert.AreEqual(expected, actual, 0.0000000000001d);
        }

        [TestMethod]
        public void CanIntegrateLinearFunctionWithTrapezoidalRule() {
            var rule = new TrapezoidalRule(100);
            var f = new GenericFunction<double>(x => 1d / x);
            var actual = f.Integrate(rule, 1, 2);
            var expected = 0.6931534304818d;
            Assert.AreEqual(expected, actual, 0.0000000000001d);
        }

        [TestMethod]
        public void CanIntegrateLinearFunctionWithSimpsonRule() {
            var rule = new SimpsonRule(100);
            var f = new GenericFunction<double>(x => 1d / x);
            var actual = f.Integrate(rule, 1, 2);
            var expected = 0.6931471805795d;
            Assert.AreEqual(expected, actual, 0.0000000000001d);
        }
    }
}
