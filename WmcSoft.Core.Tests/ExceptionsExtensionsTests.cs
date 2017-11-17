using System;
using System.Collections.Generic;
using Xunit;

namespace WmcSoft
{
    public class ExceptionsExtensionsTests
    {
        [Fact]
        public void ComposeNoExceptionReturnsNull()
        {
            var exceptions = new List<Exception>();
            var actual = exceptions.Compose();
            Assert.Null(actual);
        }

        [Fact]
        public void ComposeOneExceptionReturnsIt()
        {
            var exceptions = new List<Exception> {
                new ArgumentException()
            };
            var actual = exceptions.Compose();
            Assert.NotNull(actual);
            Assert.Equal(exceptions[0], actual);
        }

        [Fact]
        public void ComposeSomeExceptionsReturnsAggregateException()
        {
            var exceptions = new List<Exception> {
                new ArgumentException(),
                new InvalidOperationException()
            };
            var actual = exceptions.Compose();
            Assert.NotNull(actual);
            Assert.IsType<AggregateException>(actual);
            Assert.Equal(exceptions, ((AggregateException)actual).InnerExceptions);
        }
    }
}
