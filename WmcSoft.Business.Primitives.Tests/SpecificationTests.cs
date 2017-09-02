using System;
using Xunit;

namespace WmcSoft.Business
{
    public class SpecificationTests
    {
        struct IsOddSpecification : ISpecification<int>
        {
            public bool IsSatisfiedBy(int candidate)
            {
                return (candidate & 1) == 1;
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(6)]
        public void EvenNumberDoesNotSatisfyIsOddSpecification(int candidate)
        {
            var specification = new IsOddSpecification();
            Assert.False(specification.IsSatisfiedBy(candidate));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void OddNumberSatisfiesIsOddSpecification(int candidate)
        {
            var specification = new IsOddSpecification();
            Assert.True(specification.IsSatisfiedBy(candidate));
        }
    }
}
