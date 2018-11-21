using System.Diagnostics;
using Xunit;

namespace WmcSoft.Diagnostics
{
    public class TraceIndentTests
    {
        [Fact]
        public void CanIndentTrace()
        {
            var indent = Trace.IndentLevel;
            using (new TraceIndent()) {
                Assert.True(indent < Trace.IndentLevel);
            }
            Assert.Equal(indent, Trace.IndentLevel);
        }
    }
}
