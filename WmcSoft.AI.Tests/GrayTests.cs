using System;
using Xunit;

namespace WmcSoft.AI
{
    public class GrayTests
    {
        [Theory]
        [InlineData(0, "00000000")]
        [InlineData(1, "00000001")]
        [InlineData(2, "00000011")]
        [InlineData(3, "00000010")]
        [InlineData(4, "00000110")]
        [InlineData(5, "00000111")]
        [InlineData(6, "00000101")]
        [InlineData(7, "00000100")]
        public void CheckGray8(byte n, string expected)
        {
            var actual = new Gray8(n);
            Assert.Equal(expected, actual.ToString());
        }
    }
}