using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.AI.FuzzyLogic
{
    [TestClass]
    public class MembershipFunctionTests
    {
        [TestMethod]
        public void CheckTriangularMembershipFunction() {
            var m = new TriangularMembershipFunction(-1d, 0d, 1d);
            Assert.AreEqual(0.5d, (double)m.Evaluate(0.5d), 0.000001d);
            Assert.AreEqual(0.5d, (double)m.Evaluate(-0.5d), 0.000001d);
        }

        [TestMethod]
        public void CheckNegativeInfinityTrapezoidMembershipFunction() {
            var m = new TrapezoidMembershipFunction(Double.NegativeInfinity, 0d, 0d, 1d);
            Assert.AreEqual(0.5d, (double)m.Evaluate(0.5d), 0.000001d);
            Assert.AreEqual(1d, (double)m.Evaluate(-10d), 0.000001d);
        }

        [TestMethod]
        public void CheckPositiveInfinityTrapezoidMembershipFunction() {
            var m = new TrapezoidMembershipFunction(-1d, 0d, 0d, Double.PositiveInfinity);
            Assert.AreEqual(0.5d, (double)m.Evaluate(-0.5d), 0.000001d);
            Assert.AreEqual(1d, (double)m.Evaluate(10d), 0.000001d);
        }
    }
}
