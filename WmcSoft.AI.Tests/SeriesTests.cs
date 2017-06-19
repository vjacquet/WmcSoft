using System;
using System.Linq;
using Xunit;

namespace WmcSoft.AI
{
    public class SeriesTests
    {
        [Fact]
        public void CheckDetrendWithEndPoints()
        {
            double[] series = { 1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d };
            var actual = Series.Detrend<EndPoints>(series);
            Assert.True(actual.All(x => x == 0d));
        }

        [Fact]
        public void CheckDetrendWithLeastSquare()
        {
            double[] series = { 1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d };
            var actual = Series.Detrend<LeastSquare>(series);
            Assert.True(actual.All(x => x == 0d));
        }
    }
}
