using System;
using Xunit;

namespace WmcSoft.AI
{
    public class AutoassociativeFilterTests
    {
        void Identity(double[] input, int startIndex, double[] output)
        {
            for (int i = 0; i < output.Length; i++) {
                output[i] = input[startIndex + i];
            }
        }

        [Fact]
        public void CheckWithIdentityEvaluate()
        {
            var filter = new AutoassociativeFilter(Identity, 9);
            var input = new[] { 1d, 1d, 1d, 1d, 1d, 1d, 1d, 1d, 1d, 1d, 1d, 1d };
            var output = new double[input.Length];
            filter.Execute(input, output);
            Assert.Equal(input, output);
        }
    }
}
