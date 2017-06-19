using System;
using Xunit;

namespace WmcSoft.AI.FuzzyLogic
{
    public class MembershipFunctionTests
    {
        [Fact]
        public void CheckTriangularMembershipFunction()
        {
            var m = new TriangularMembershipFunction(-1d, 0d, 1d);
            Assert.Equal(0.5d, (double)m.Evaluate(0.5d), 6);
            Assert.Equal(0.5d, (double)m.Evaluate(-0.5d), 6);
        }

        [Fact]
        public void CheckNegativeInfinityTrapezoidMembershipFunction()
        {
            var m = new TrapezoidMembershipFunction(Double.NegativeInfinity, 0d, 0d, 1d);
            Assert.Equal(0.5d, (double)m.Evaluate(0.5d), 6);
            Assert.Equal(1d, (double)m.Evaluate(-10d), 6);
        }

        [Fact]
        public void CheckPositiveInfinityTrapezoidMembershipFunction()
        {
            var m = new TrapezoidMembershipFunction(-1d, 0d, 0d, Double.PositiveInfinity);
            Assert.Equal(0.5d, (double)m.Evaluate(-0.5d), 6);
            Assert.Equal(1d, (double)m.Evaluate(10d), 6);
        }

        [Fact]
        public void CheckTrapezoidMembershipFunction()
        {
            var m = new TrapezoidMembershipFunction(-2d, -1d, 1d, 2d);
            Assert.Equal(1d, (double)m.Evaluate(0d), 6);
            Assert.Equal(0.5d, (double)m.Evaluate(1.5d), 6);
            Assert.Equal(0.5d, (double)m.Evaluate(-1.5d), 6);
            Assert.Equal(0d, (double)m.Evaluate(5d), 6);
            Assert.Equal(0d, (double)m.Evaluate(-5d), 6);
        }
    }
}
