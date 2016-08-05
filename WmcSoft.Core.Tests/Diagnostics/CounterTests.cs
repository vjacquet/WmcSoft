using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Diagnostics
{
    [TestClass]
    public class CounterTests
    {
        [TestMethod]
        public void CanIncrementCounter() {
            var counter = new Counter("a");

            Assert.AreEqual(0, counter.Tally);
            counter.Increment();
            counter.Increment();
            Assert.AreEqual(2, counter.Tally);
        }

        [TestMethod]
        public void CanResetCounter() {
            var counter = new Counter("a");

            counter.Increment();
            counter.Increment();

            var expected = counter.Tally;
            var actual = counter.Reset();
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(0, counter.Tally);
        }
    }
}
