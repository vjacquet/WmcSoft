using System;
using System.Collections.Generic;
using Xunit;

namespace WmcSoft.Runtime
{
    public class ObjectWalkerTests
    {
        class ComplexObject
        {
            readonly object[] _items;

            public ComplexObject(params object[] items)
            {
                _items = items;
                Age = 32;
            }

            public string Name { get; set; }
            public object this[int index] {
                get {
                    return _items[index];
                }
            }

            public int Age { get; set; }
        }

        [Fact]
        public void CanWalkSimpleObject()
        {
            var root = "expected";
            var expected = new HashSet<object>();
            expected.Add(root);

            var walker = new ObjectWalker(root);
            var actual = new HashSet<object>(walker);

            Assert.True(expected.SetEquals(actual));
        }

        [Fact]
        public void CanWalkCollection()
        {
            var root = new object[] { "one", "two", 3 };
            var expected = new HashSet<object>();
            for (int i = 0; i != root.Length; i++)
                expected.Add(root[i]);

            var walker = new ObjectWalker(root);
            var actual = new HashSet<object>(walker);

            Assert.True(expected.SetEquals(actual));
        }

        [Fact]
        public void CanWalkSequentially()
        {
            var root = new object[] { "one", new object[] { "two", "three", "four" }, new object[] { "five" }, "six" };
            var expected = new List<object>() {
                "one",
                "two",
                "three",
                "four",
                "five",
                "six"
            };

            var walker = new ObjectWalker(root);
            var actual = new List<object>(walker);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanWalkComplexObject()
        {
            var root = new ComplexObject("one", "two", 3);
            root.Name = "name";
            var expected = new HashSet<object>();
            expected.Add(root);
            expected.Add("one");
            expected.Add("two");
            expected.Add(3);
            expected.Add("name");
            expected.Add(32);

            var walker = new ObjectWalker(root);
            var actual = new HashSet<object>(walker);

            Assert.True(expected.SetEquals(actual));
        }
    }
}
