using Xunit;

namespace WmcSoft.Arithmetics
{
    public class InterpolationTests
    {
        [Fact]
        public void CheckLinearInterpolation()
        {
            double[] x = { 1d, 2d, 3d };
            double[] y = { 2d, 4d, 6d };
            var i = new LinearInterpolation(x, y);
            var actual = i.Interpolate(1.5d);
            Assert.Equal(3d, actual);
        }

        [Fact]
        public void CheckLinearInterpolationWhenExact()
        {
            double[] x = { 1d, 2d, 3d };
            double[] y = { 2d, 4d, 6d };
            var i = new LinearInterpolation(x, y);
            var actual = i.Interpolate(2d);
            Assert.Equal(4d, actual);
        }

        [Fact]
        public void CheckLinearInterpolationWhenSmaller()
        {
            double[] x = { 1d, 2d, 3d };
            double[] y = { 2d, 4d, 6d };
            var i = new LinearInterpolation(x, y);
            var actual = i.Interpolate(0d);
            Assert.Equal(0d, actual);
        }

        [Fact]
        public void CheckLinearInterpolationWhenBigger()
        {
            double[] x = { 1d, 2d, 3d };
            double[] y = { 2d, 4d, 6d };
            var i = new LinearInterpolation(x, y);
            var actual = i.Interpolate(4d);
            Assert.Equal(8d, actual);
        }
    }
}