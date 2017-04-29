using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Text.RegularExpressions;

namespace WmcSoft
{
    [TestClass]
    public class RegexExtensionsTests
    {
        [TestMethod]
        public void CanGetGroupValue()
        {
            var regex = new Regex(@"^(?<n>\d+)");
            var m = regex.Match("42");
            var actual = m.GetGroupValue<int>("n");
            Assert.AreEqual(42, actual);
        }

        [TestMethod]
        public void CanGetGroupNullableValue()
        {
            var regex = new Regex(@"^(?<n>\d+)");
            var m = regex.Match("42");
            var actual = m.GetNullableGroupValue<int>("n");
            Assert.AreEqual(42, actual);
        }

        [TestMethod]
        public void CheckMissingGroupValueIsNullOnNullable()
        {
            var regex = new Regex(@"^(?<n>\d+)");
            var m = regex.Match("abc");
            var actual = m.GetNullableGroupValue<int>("n");
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void CheckMissingGroupValueIsNull()
        {
            var regex = new Regex(@"^(?<n>\d+)");
            var m = regex.Match("abc");
            var actual = m.GetGroupValue<int>("n");
            Assert.Fail();
        }

        [TestMethod]
        public void CheckMissingGroupValueIsDefaultValue()
        {
            var regex = new Regex(@"^(?<n>\d+)");
            var m = regex.Match("abc");
            var actual = m.GetGroupValueOrDefault("n", 4);
            Assert.AreEqual(4, actual);
        }
    }
}
