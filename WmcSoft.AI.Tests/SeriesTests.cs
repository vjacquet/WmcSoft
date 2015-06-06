using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.AI
{
    [TestClass]
    public class SeriesTests
    {
        [TestMethod]
        public void CheckDetrendWithEndPoints() {
            double[] series = { 1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d };
            var actual = Series.Detrend<EndPoints>(series);
            Assert.IsTrue(actual.All(x => x == 0d));
        }

        [TestMethod]
        public void CheckDetrendWithLeastSquare() {
            double[] series = { 1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d };
            var actual = Series.Detrend<LeastSquare>(series);
            Assert.IsTrue(actual.All(x => x == 0d));
        }
    }
}
