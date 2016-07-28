using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Diagnostics
{
    [TestClass]
    public class TraceIndentTests
    {
        [TestMethod]
        public void CanIndentTrace() {
            var indent = Trace.IndentLevel;
            using (new TraceIndent()) {
                Assert.IsTrue(indent < Trace.IndentLevel);
            }
            Assert.AreEqual(indent, Trace.IndentLevel);
        }
    }
}
