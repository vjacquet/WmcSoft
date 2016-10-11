using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic.Internals
{
    [TestClass]
    public class BulkStackTests
    {
        [TestMethod]
        public void CanPush() {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var stack = new BulkStack<int>();
            foreach (var item in data)
                stack.Push(item);

            var length = data.Length;
            for (int i = length - 1; i >= 0; i--) {
                var expected = data[i];
                var actual = stack.Pop();
                Assert.AreEqual(expected, actual);
            }
            Assert.IsTrue(stack.IsEmpty());
        }

        [TestMethod]
        public void CanBulkPush() {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var stack = new BulkStack<int>();
            stack.BulkPush(data.Length, (array, index) => Array.Copy(data, 0, array, index, data.Length));

            var length = data.Length;
            for (int i = length - 1; i >= 0; i--) {
                var expected = data[i];
                var actual = stack.Pop();
                Assert.AreEqual(expected, actual);
            }
            Assert.IsTrue(stack.IsEmpty());
        }

        [TestMethod]
        public void CanBulkPushAfterAFewPushes() {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var stack = new BulkStack<int>();
            stack.Push(-20);
            stack.BulkPush(data.Length, (array, index) => Array.Copy(data, 0, array, index, data.Length));
            stack.Push(20);

            Assert.AreEqual(20, stack.Pop());

            var length = data.Length;
            for (int i = length - 1; i >= 0; i--) {
                var expected = data[i];
                var actual = stack.Pop();
                Assert.AreEqual(expected, actual);
            }

            Assert.AreEqual(-20, stack.Pop());

            Assert.IsTrue(stack.IsEmpty());
        }
    }
}
