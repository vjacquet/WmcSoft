using System;
using Xunit;

namespace WmcSoft
{
    public class UriExtensionsTests
    {
        [Theory]
        [InlineData("http://example.com", "http://example.com/?a=b")]
        [InlineData("http://example.com/", "http://example.com/?a=b")]
        [InlineData("http://example.com/path/to/file", "http://example.com/path/to/file?a=b")]
        [InlineData("http://example.com/path/to/file#fragment", "http://example.com/path/to/file?a=b#fragment")]
        [InlineData("http://example.com/?name=value", "http://example.com/?name=value&a=b")]
        public void CanAppendParameterToEmptyQuery(string uri, string result)
        {
            var expected = new Uri(result);
            var actual = new UriBuilder(uri)
                .AppendToQuery("a", "b")
                .Uri;
            Assert.Equal(expected, actual);
        }
    }
}
