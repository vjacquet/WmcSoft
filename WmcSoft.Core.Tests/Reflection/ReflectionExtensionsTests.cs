using System;
using Xunit;

namespace WmcSoft.Reflection
{
    public class ReflectionExtensionsTests
    {
        public class Bench
        {
            public string AString { get; set; }
            public int AInt32 { get; set; }
        }

        [Fact]
        public void CanGetNullValue()
        {
            var bench = new Bench { AString = null };
            var actual = typeof(Bench).GetProperty("AString").GetValue<string>(bench);
            Assert.Null(actual);
        }

        [Fact]
        public void CanGetValueConvertedToString()
        {
            var bench = new Bench { AInt32 = 51 };
            var actual = typeof(Bench).GetProperty("AInt32").GetValue<string>(bench);
            Assert.Equal("51", actual);
        }

        [Fact]
        public void CanGetValueConvertedToInt32()
        {
            var bench = new Bench { AString = "51" };
            var actual = typeof(Bench).GetProperty("AString").GetValue<int>(bench);
            Assert.Equal(51, actual);
        }

        [Fact]
        public void CannotGetValueThatCannotGetConvertedToInt32()
        {
            var bench = new Bench { AString = "abc" };
            Assert.Throws<FormatException>(() => typeof(Bench).GetProperty("AString").GetValue<int>(bench));
        }
    }
}