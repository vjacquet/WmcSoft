using System;
using Xunit;

namespace WmcSoft.AI.FuzzyLogic
{
    public class FuzzyVarTests
    {
        [Fact]
        public void CheckConstruct()
        {
            FuzzyVar var = 0.5;
        }

        [Fact]
        public void CheckCompareTo()
        {
            FuzzyVar lhs = 0.5;
            FuzzyVar rhs = 0.7;
            Assert.True(lhs.CompareTo(rhs) == -1);
            Assert.True(rhs.CompareTo(lhs) == +1);
            rhs = lhs;
            Assert.True(lhs.CompareTo(rhs) == 0);
            Assert.True(rhs.CompareTo(lhs) == 0);
        }


        [Fact]
        public void CheckAnd()
        {
            FuzzyVar lhs = 0.5d;
            FuzzyVar rhs = 0.7d;
            FuzzyVar result = lhs & rhs;
            Assert.Equal((double)result, 0.5d);
        }

        [Fact]
        public void CheckOr()
        {
            FuzzyVar lhs = 0.5d;
            FuzzyVar rhs = 0.7d;
            FuzzyVar result = lhs | rhs;
            Assert.Equal((double)result, 0.7d);
        }

        [Fact]
        public void CheckOutOfRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                FuzzyVar var = 1.5d;
            });
        }
    }
}
