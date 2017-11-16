using System;
using Xunit;
using System.Collections.Generic;

namespace WmcSoft
{
    public class ActionExtensionsTests
    {
        [Fact]
        public void CanCallOnce()
        {
            var stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);

            Action pop = () => stack.Pop();
            var once = pop.Once();
            once();
            Assert.NotEmpty(stack);
            Assert.Equal(1, stack.Peek());
        }

        [Fact]
        public void CanCallBefore()
        {
            var stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            Action pop = () => stack.Pop();
            var before = pop.Before(2);

            before();
            Assert.Equal(2, stack.Peek());
            before();
            Assert.Equal(1, stack.Peek());
            before();
            Assert.Equal(1, stack.Peek());
        }

        [Fact]
        public void CanCallAfter()
        {
            var stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            Action pop = () => stack.Pop();
            var after = pop.After(2);

            after();
            Assert.Equal(3, stack.Peek());
            after();
            Assert.Equal(3, stack.Peek());
            after();
            Assert.Equal(2, stack.Peek());
        }
    }
}
