using Xunit;

using static WmcSoft.AI.NeuralNetwork.Helpers;

namespace WmcSoft.AI.NeuralNetwork
{
    public class HelpersTests
    {
        [Fact]
        public void CheckCount()
        {
            var values = new[] { 1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d };
            Assert.Equal(4, Count(values, 5.5d));
            Assert.Equal(3, Count(values, 7d));
        }
    }
}
