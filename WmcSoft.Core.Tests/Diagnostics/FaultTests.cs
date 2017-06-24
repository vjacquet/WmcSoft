using Xunit;
using System;

namespace WmcSoft.Diagnostics
{
    public class FaultTests
    {
        [Fact]
        public void CanUseConditionalOrExpression()
        {
            Fault x = Fault.Empty;
            Fault y = new ArgumentException();

            if (x || y) {
                // ok
            } else {
                Assert.True(false, "Fail");
            }
        }

        [Fact]
        public void CanUseConditionalAndExpressions()
        {
            Fault x = Fault.Empty;
            Fault y = new ArgumentException();

            if (x && y) {
                Assert.True(false, "Fail");
            }
        }

        [Fact]
        public void CanCreateSimpleExpression()
        {
            var fault = new Fault(new ArgumentNullException());
            Assert.IsAssignableFrom<ArgumentException>(fault.Exception);
        }

        [Fact]
        public void CanCreateAggregateExpression()
        {
            var fault = new Fault(new ArgumentNullException(), new InvalidOperationException());
            Assert.IsType<AggregateException>(fault.Exception);
        }
    }
}