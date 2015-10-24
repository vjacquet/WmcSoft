using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;

namespace WmcSoft.Reflection
{
    [TestClass]
    public class TypeExtensionsTests
    {
        #region Test classes

        [System.ComponentModel.Description("The A class")]
        [DisplayName("a")]
        public class A
        {
        }

        [System.ComponentModel.Description("The B class")]
        [DisplayName("b")]
        public class B : A
        {
        }

        public class C : B
        {

        }

        #endregion

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

        [TestMethod]
        public void CheckGetInheritedDescription() {
            var expected = "The B class";
            var actual = typeof(C).GetDescription(true);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckGetDescription() {
            var expected = "";
            var actual = typeof(C).GetDescription(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckGetInheritedDisplayName() {
            var expected = "b";
            var actual = typeof(C).GetDisplayName(true);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckGetDisplayName() {
            var expected = "C";
            var actual = typeof(C).GetDisplayName(false);
            Assert.AreEqual(expected, actual);
        }
    }
}
