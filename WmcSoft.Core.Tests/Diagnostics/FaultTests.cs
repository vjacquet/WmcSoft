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

        [TestMethod]
        public void CanCreateSimpleExpression()
        {
            var fault = new Fault(new ArgumentNullException());
            Assert.IsInstanceOfType(fault.Exception, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void CanCreateAggregateExpression()
        {
            var fault = new Fault(new ArgumentNullException(), new InvalidOperationException());
            Assert.IsInstanceOfType(fault.Exception, typeof(AggregateException));
        }
    }
}
