using System;
using Xunit;

namespace WmcSoft.Diagnostics
{
    public class CounterTests
    {
        [Fact]
        public void CanIncrementCounter()
        {
            var counter = new Counter("a");

            Assert.Equal(0, counter.Tally);
            counter.Increment();
            counter.Increment();
            Assert.Equal(2, counter.Tally);
        }

        [Fact]
        public void CanResetCounter()
        {
            var counter = new Counter("a");

            counter.Increment();
            counter.Increment();

            var expected = counter.Tally;
            var actual = counter.Reset();
            Assert.Equal(expected, actual);
            Assert.Equal(0, counter.Tally);
        }
    }
}
