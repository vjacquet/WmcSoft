using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.AI.FuzzyLogic
{
    [TestClass]
    public class FuzzyVarTests
    {
        [TestMethod]
        public void CheckConstruct() {
            FuzzyVar var = (FuzzyVar)0.5;
        }

        [TestMethod]
        public void CheckCompareTo() {
            FuzzyVar lhs = (FuzzyVar)0.5;
            FuzzyVar rhs = (FuzzyVar)0.7;
            Assert.IsTrue(lhs.CompareTo(rhs) == -1);
            Assert.IsTrue(rhs.CompareTo(lhs) == +1);
            rhs = lhs;
            Assert.IsTrue(lhs.CompareTo(rhs) == 0);
            Assert.IsTrue(rhs.CompareTo(lhs) == 0);
        }


        [TestMethod]
        public void CheckAnd() {
            FuzzyVar lhs = (FuzzyVar)0.5d;
            FuzzyVar rhs = (FuzzyVar)0.7d;
            FuzzyVar result = lhs & rhs;
            Assert.AreEqual((double)result, 0.5d);
        }

        [TestMethod]
        public void CheckOr() {
            FuzzyVar lhs = (FuzzyVar)0.5d;
            FuzzyVar rhs = (FuzzyVar)0.7d;
            FuzzyVar result = lhs | rhs;
            Assert.AreEqual((double)result, 0.7d);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CheckOutOfRange() {
            FuzzyVar var = (FuzzyVar)1.5d;
        }
    }
}
