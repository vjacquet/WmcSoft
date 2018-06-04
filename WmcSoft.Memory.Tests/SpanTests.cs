using System;
using Xunit;

namespace WmcSoft.Memory
{
    public class SpanTests
    {
        [Fact]
        public void CanCreateSpanFromStrings()
        {
            var span = "This is a string".AsSpan();
            Assert.False(span.IsEmpty);
        }
    }
}
