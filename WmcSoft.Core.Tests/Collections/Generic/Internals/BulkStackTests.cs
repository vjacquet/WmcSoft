using System;
using Xunit;

namespace WmcSoft.Collections.Generic.Internals
{
    public class BulkStackTests
    {
        [Fact]
        public void CanPush()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var stack = new BulkStack<int>();
            foreach (var item in data)
                stack.Push(item);

            var length = data.Length;
            for (int i = length - 1; i >= 0; i--) {
                var expected = data[i];
                var actual = stack.Pop();
                Assert.Equal(expected, actual);
            }
            Assert.True(stack.IsEmpty());
        }

        [Fact]
        public void CanBulkPush()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var stack = new BulkStack<int>();
            stack.BulkPush(data.Length, (array, index) => Array.Copy(data, 0, array, index, data.Length));

            var length = data.Length;
            for (int i = length - 1; i >= 0; i--) {
                var expected = data[i];
                var actual = stack.Pop();
                Assert.Equal(expected, actual);
            }
            Assert.True(stack.IsEmpty());
        }

        [Fact]
        public void CanPushItems()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var stack = new BulkStack<int>();
            stack.Push(data);

            var length = data.Length;
            for (int i = length - 1; i >= 0; i--) {
                var expected = data[i];
                var actual = stack.Pop();
                Assert.Equal(expected, actual);
            }
            Assert.True(stack.IsEmpty());
        }

        [Fact]
        public void CanBulkPushAfterAFewPushes()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var stack = new BulkStack<int>();
            stack.Push(-20);
            stack.BulkPush(data.Length, (array, index) => Array.Copy(data, 0, array, index, data.Length));
            stack.Push(20);

            Assert.Equal(20, stack.Pop());

            var length = data.Length;
            for (int i = length - 1; i >= 0; i--) {
                var expected = data[i];
                var actual = stack.Pop();
                Assert.Equal(expected, actual);
            }

            Assert.Equal(-20, stack.Pop());
            Assert.True(stack.IsEmpty());
        }
    }
}