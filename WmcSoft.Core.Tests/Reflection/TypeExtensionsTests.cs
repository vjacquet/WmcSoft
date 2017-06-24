using System;
using System.Linq;
using System.Reflection;
using Xunit;
using System.ComponentModel;

namespace WmcSoft.Reflection
{
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

        [Fact]
        public void AssertIsOverriden()
        {
            var m = typeof(Uri).GetMethod("GetHashCode", BindingFlags.Public | BindingFlags.Instance);
            var actual = m.DeclaringType != typeof(Object);
            Assert.True(actual);
        }

        [Fact]
        public void CheckEnumerateDefinitions()
        {
            var m = typeof(Uri).GetMethod("GetHashCode", BindingFlags.Public | BindingFlags.Instance);
            var expected = new Type[] { typeof(Uri), typeof(object) };
            var actual = m.EnumerateDefinitions().Select(d => d.DeclaringType).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckGetInheritedDescription()
        {
            var expected = "The B class";
            var actual = typeof(C).GetDescription(true);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckGetDescription()
        {
            var expected = "";
            var actual = typeof(C).GetDescription(false);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckGetInheritedDisplayName()
        {
            var expected = "b";
            var actual = typeof(C).GetDisplayName(true);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckGetDisplayName()
        {
            var expected = "C";
            var actual = typeof(C).GetDisplayName(false);
            Assert.Equal(expected, actual);
        }
    }
}