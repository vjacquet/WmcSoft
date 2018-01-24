using Xunit;

namespace WmcSoft.Statistics
{
    public class AccumulatorTests
    {
        [Fact]
        public void CanGetPropertiesWhenEmpty()
        {
            var a = new SamplesAccumulator();
            Assert.Equal(0d, a.Mean);
            Assert.Equal(0d, a.Variance);
            Assert.Equal(0d, a.Sigma);
        }

        [Fact]
        public void CanGetPropertiesWhenAddedOne()
        {
            var a = new SamplesAccumulator(1d);
            Assert.Equal(1d, a.Mean);
            Assert.Equal(0d, a.Variance);
            Assert.Equal(0d, a.Sigma);
        }

        [Fact]
        public void CheckStraightforwardAccumulator()
        {
            var a = new StraightforwardAccumulator(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Equal(5.5d, a.Mean);
            Assert.Equal(8.25d, a.Variance);
            Assert.Equal(2.872281323d, a.Sigma, 8);
        }

        [Fact]
        public void CheckPopulationAccumulator()
        {
            var a = new PopulationAccumulator(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Equal(5.5d, a.Mean);
            Assert.Equal(8.25d, a.Variance);
            Assert.Equal(2.872281323d, a.Sigma, 8);
        }

        [Fact]
        public void CheckSamplesAccumulator()
        {
            var a = new SamplesAccumulator(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.Equal(5.5d, a.Mean);
            Assert.Equal(9.166666667d, a.Variance, 8);
            Assert.Equal(3.027650354d, a.Sigma, 8);
        }

        [Fact]
        public void StraightforwardAccumulatorIsUnstable()
        {
            var a1 = new StraightforwardAccumulator(100_000_000d, 100_000_001d, 100_000_002d);
            var a2 = new PopulationAccumulator(100_000_000d, 100_000_001d, 100_000_002d);
            Assert.NotEqual(a1.Variance, a2.Variance);
        }
    }
}
