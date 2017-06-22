using Xunit;

namespace WmcSoft.Arithmetics
{
    public class IntegralCalculusTests
    {
        [Fact]
        public void CanIntegrateLinearFunctionWithMidOrdinateRule() {
            var rule = new MidOrdinateRule(100);
            var f = new GenericFunction<double>(x => 1d / x);
            var actual = f.Integrate(rule, 1, 2);
            var expected = 0.6931440556283d;
            Assert.Equal(expected, actual, 13);
        }

        [Fact]
        public void CanIntegrateLinearFunctionWithTrapezoidalRule() {
            var rule = new TrapezoidalRule(100);
            var f = new GenericFunction<double>(x => 1d / x);
            var actual = f.Integrate(rule, 1, 2);
            var expected = 0.6931534304818d;
            Assert.Equal(expected, actual, 13);
        }

        [Fact]
        public void CanIntegrateLinearFunctionWithSimpsonRule() {
            var rule = new SimpsonRule(100);
            var f = new GenericFunction<double>(x => 1d / x);
            var actual = f.Integrate(rule, 1, 2);
            var expected = 0.6931471805795d;
            Assert.Equal(expected, actual, 13);
        }

        [Fact]
        public void CanIntegrateLinearFunctionWithBooleRule() {
            var rule = new BooleRule();
            var f = new GenericFunction<double>(x => 1d / x);
            var actual = f.Integrate(rule, 1, 2);
            var expected = System.Math.Log(2);
            Assert.Equal(expected, actual, 3);
        }
    }
}
