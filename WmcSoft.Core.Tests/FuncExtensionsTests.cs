using System;
using Xunit;
using WmcSoft.Diagnostics;

namespace WmcSoft
{
    public class FuncExtensionsTests
    {
        static int Fault(int i)
        {
            if (i == 3)
                throw new ArgumentOutOfRangeException("i");
            return i * i;
        }

        [Fact]
        public void CanApplyWithException()
        {
            Func<int, int> fault = Fault;
            try {
                fault.ApplyEach(1, 2, 3, 4, 5);
            } catch (ArgumentOutOfRangeException e) {
                Assert.Equal(2, e.GetCapturedEntry("i"));
                Assert.Equal(3, e.GetCapturedEntry("arg"));
            }
        }
    }
}
