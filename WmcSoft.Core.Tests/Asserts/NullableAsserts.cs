using System;
using Xunit;

namespace WmcSoft.Asserts
{
    public class NullableAsserts
    {
        [Fact]
        public void EqualsWithNullDetectsEquality()
        {
            int? a = null;
            int? b = 1;
            int? c = null;

            Assert.False(a == b);
            Assert.True(a != b);
            Assert.NotEqual(a, b);

            Assert.True(a == c);
            Assert.False(a != c);
            Assert.Equal(a, c);
        }

        [Fact]
        public void ComparingWithNullUsingOperatorsAlwaysReturnFalse()
        {
            int? a = null;
            int? b = 1;
            int? c = null;

            Assert.False(a < b);
            Assert.False(a <= b);
            Assert.False(a > b);
            Assert.False(a >= b);


            Assert.False(a < c);
            Assert.False(a <= c);
            Assert.False(a > c);
            Assert.False(a >= c);
        }

        [Fact]
        public void ComparingWithNullUsingComparerConsiderNullAsLessThanAnything()
        {
            int? a = null;
            int? b = 1;
            int? c = null;

            Assert.True(Nullable.Compare(a, b) < 0);
            Assert.True(Nullable.Compare(a, b) <= 0);
            Assert.False(Nullable.Compare(a, b) > 0);
            Assert.False(Nullable.Compare(a, b) >= 0);
            Assert.False(Nullable.Compare(a, b) == 0);

            Assert.False(Nullable.Compare(a, c) < 0);
            Assert.True(Nullable.Compare(a, c) <= 0);
            Assert.False(Nullable.Compare(a, c) > 0);
            Assert.True(Nullable.Compare(a, c) >= 0);
            Assert.True(Nullable.Compare(a, c) == 0);
        }

        [Fact]
        public void ArithmeticsWithNullReturnsNull()
        {
            int? a = null;
            int? b = 1;
            int? c = null;

            Assert.Null(a + b);
            Assert.Equal(a + b, null);

            Assert.Null(a + c);
            Assert.Equal(a + c, null);

            Assert.Null(a - b);
            Assert.Equal(a - b, null);

            Assert.Null(a - c);
            Assert.Equal(a - c, null);

            Assert.Null(a * b);
            Assert.Equal(a * b, null);

            Assert.Null(a * c);
            Assert.Equal(a * c, null);

            Assert.Null(a / b);
            Assert.Equal(a / b, null);

            Assert.Null(a / c);
            Assert.Equal(a / c, null);
        }

        [Fact]
        public void BitwiseWithNullReturnsNull()
        {
            {
                int? a = null;
                int? b = 1;
                int? c = null;

                Assert.Null(a & b);
                Assert.Equal(a & b, null);

                Assert.Null(a & c);
                Assert.Equal(a & c, null);

                Assert.Null(a | b);
                Assert.Equal(a | b, null);

                Assert.Null(a | c);
                Assert.Equal(a | c, null);

                Assert.Null(~a);
            }
        }
    }
}
