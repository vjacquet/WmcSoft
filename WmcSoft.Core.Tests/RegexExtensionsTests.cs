using System;
using System.Text.RegularExpressions;
using Xunit;
using WmcSoft.Text.RegularExpressions;

namespace WmcSoft
{
    public class RegexExtensionsTests
    {
        [Fact]
        public void CanGetGroupValue()
        {
            var regex = new Regex(@"^(?<n>\d+)");
            var m = regex.Match("42");
            var actual = m.GetGroupValue<int>("n");
            Assert.Equal(42, actual);
        }

        [Fact]
        public void CanGetGroupNullableValue()
        {
            var regex = new Regex(@"^(?<n>\d+)");
            var m = regex.Match("42");
            var actual = m.GetNullableGroupValue<int>("n");
            Assert.Equal(42, actual);
        }

        [Fact]
        public void CheckMissingGroupValueIsNullOnNullable()
        {
            var regex = new Regex(@"^(?<n>\d+)");
            var m = regex.Match("abc");
            var actual = m.GetNullableGroupValue<int>("n");
            Assert.Equal(null, actual);
        }

        [Fact]
        public void CheckMissingGroupValueIsNull()
        {
            var regex = new Regex(@"^(?<n>\d+)");
            var m = regex.Match("abc");
            Assert.Throws<IndexOutOfRangeException>(() => m.GetGroupValue<int>("n"));
        }

        [Fact]
        public void CheckMissingGroupValueIsDefaultValue()
        {
            var regex = new Regex(@"^(?<n>\d+)");
            var m = regex.Match("abc");
            var actual = m.GetGroupValueOrDefault("n", 4);
            Assert.Equal(4, actual);
        }
    }
}
