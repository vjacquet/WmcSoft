using System;
using System.Linq;
using Xunit;
using WmcSoft.Collections;

namespace WmcSoft.Text
{
    public class NumericalComparerTests
    {
        [Fact]
        public void CanCompareNumerics()
        {
            var random = new Random(1664);
            var expected = Array.ConvertAll(Enumerable.Range(1, 1000).ToArray(), x => x.ToString());
            var data = (string[])expected.Clone();
            data.Shuffle(random);

            Assert.NotEqual(expected, data);

            Array.Sort(data, StringComparer.CurrentCulture);
            Assert.NotEqual(expected, data);

            Array.Sort(data, new NumericalComparer());
            Assert.Equal(expected, data);
        }
    }
}