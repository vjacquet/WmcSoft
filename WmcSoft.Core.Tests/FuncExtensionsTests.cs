using System;
using Xunit;
using WmcSoft.Diagnostics;
using System.Collections.Generic;

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

        [Fact]
        public void CanCallOnce()
        {
            var stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);

            Func<int> pop = () => stack.Pop();
            var once = pop.Once();
            Assert.Equal(2, once());
            Assert.Equal(2, once());
            Assert.NotEmpty(stack);
            Assert.Equal(1, stack.Pop());
            Assert.Empty(stack);
        }

        [Fact]
        public void CanCallBefore()
        {
            var stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);

            Func<int> pop = () => stack.Pop();
            var before = pop.Before(2);
            Assert.Equal(2, before());
            Assert.NotEmpty(stack);
            Assert.Equal(1, before());
            Assert.Empty(stack);
            Assert.Equal(1, before());
        }
    }
}
