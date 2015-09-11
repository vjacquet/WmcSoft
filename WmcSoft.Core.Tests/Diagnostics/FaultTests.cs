using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WmcSoft.Diagnostics
{
    [TestClass]
    public class FaultTests
    {
        [TestMethod]
        public void CanUseConditionalOrExpression() {
            Fault x = Fault.Empty;
            Fault y = new ArgumentException();

            if (x || y) {
                // ok
            } else {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CanUseConditionalAndExpressions() {
            Fault x = Fault.Empty;
            Fault y = new ArgumentException();

            if (x && y) {
                Assert.Fail();
            }
        }
    }
}
