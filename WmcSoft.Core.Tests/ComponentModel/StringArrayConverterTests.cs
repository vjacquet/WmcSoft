using Xunit;

namespace WmcSoft.ComponentModel
{
    public class StringArrayConverterTests
    {
        [Fact]
        public void CanConvertFromString()
        {
            var converter = new StringArrayConverter();
            Assert.True(converter.CanConvertFrom(typeof(string)));

            var text = "a,b,c";
            Assert.Equal(new[] { "a", "b", "c" }, converter.ConvertFromInvariantString(text));
        }

        [Fact]
        public void CanConvertToString()
        {
            var converter = new StringArrayConverter();
            Assert.True(converter.CanConvertTo(typeof(string)));

            var array = new[] { "a", "b", "c" };
            Assert.Equal("a,b,c", converter.ConvertToInvariantString(array));
        }
    }
}
