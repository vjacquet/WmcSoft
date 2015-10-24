using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Reflection
{
    [TestClass]
    public class TypeExtensionsTests
    {
        [TestMethod]
        public void AssertIsOverriden() {
            var m = typeof(Uri).GetMethod("GetHashCode", BindingFlags.Public | BindingFlags.Instance);
            var actual = m.DeclaringType != typeof(Object);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CheckEnumerateDefinitions() {
            var m = typeof(Uri).GetMethod("GetHashCode", BindingFlags.Public | BindingFlags.Instance);
            var expected = new Type[] { typeof(Uri), typeof(object) };
            var actual = m.EnumerateDefinitions().Select(d => d.DeclaringType).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
