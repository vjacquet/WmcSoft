using System.Collections.Generic;
using Xunit;

namespace WmcSoft.Text
{
    public class RomanConverterTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void CanConvertFromInt32(string text, int number)
        {
            var actual = RomanConverter.FromInt32(number);
            Assert.Equal(text, actual);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void CanConvertToInt32(string text, int number)
        {
            var actual = RomanConverter.ToInt32(text);
            Assert.Equal(number, actual);
        }

        public static IReadOnlyList<object[]> Data => new List<object[]>
        {
            new object[] { "I", 1},
            new object[] { "II", 2},
            new object[] { "III", 3},
            new object[] { "IV", 4},
            new object[] { "V", 5},
            new object[] { "VI", 6},
            new object[] { "VII", 7},
            new object[] { "IX", 9},
            new object[] { "X", 10},
            new object[] { "XIII", 13},
            new object[] { "MDCCLXXVI", 1776},
            new object[] { "MCMXC", 1990},
        };
    }
}
