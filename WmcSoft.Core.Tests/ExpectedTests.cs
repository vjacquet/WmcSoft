using System;
using Xunit;
using WmcSoft.Diagnostics;

namespace WmcSoft
{
    public class ExpectedTests
    {
        static Expected<T> Success<T>(T value)
        {
            return value;
        }
        static Expected<T> Failed<T>(Exception exception)
        {
            return exception;
        }

        [Fact]
        public void CheckSuccessOperation()
        {
            var result = Success(5);
            Assert.Equal(6, (int)result + 1);
        }

        [Fact]
        public void CheckFailedOperation()
        {
            var result = Failed<int>(new ArgumentOutOfRangeException());
            Assert.Throws<ArgumentOutOfRangeException>(() => (int)result + 1);
        }

        Expected<int> MethodWithParameters(string s, int n)
        {
            var exception = new ArgumentNullException();
            exception.CaptureContext(new { s, n })
                .CaptureCaller();
            return exception;
        }

        [Fact]
        public void CheckCapture()
        {
            var r = MethodWithParameters("beer", 1664);
            Assert.True(r.IsFaulted);
            Assert.Equal("beer", r.Exception.GetCapturedEntry("s"));
            Assert.Equal(1664, r.Exception.GetCapturedEntry("n"));
            Assert.NotNull(r.Exception.GetCapturedEntry("caller"));
        }

        [Fact]
        public void CheckDataKeyConverterWithPrefixes()
        {
            var converter = DataKeyConverter.Basic.WithPrefix("own.").WithPrefix("my.");
            var actual = converter.ConvertTo("key");
            Assert.Equal("my.own.key", actual);
        }
    }
}
