using System;
using Xunit;

namespace WmcSoft
{
    public class ConvertibleExtensionsTests
    {
        [Fact]
        public void CanConvertTo()
        {
            var expected = 5m;
            var n = 5;
            var actual = n.ConvertTo<decimal>();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanConvertNullableTo()
        {
            decimal? expected = 5m;
            int? n = 5;
            var actual = n.ConvertTo<decimal?>();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CannotConvertNullToNonNullable()
        {
            int? n = null;
            Assert.Throws<InvalidCastException>(() => n.ConvertTo<decimal>());
        }
    }
}