using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    /// <summary>
    /// Description résumée pour IntervalLimitTests
    /// </summary>
    [TestClass]
    public class IntervalLimitTests
    {
        [TestMethod]
        public void CanCompareIntervalLimits() {
            var one = IntervalLimit.Lower(1, false);
            var two = IntervalLimit.Upper(2, false);

            Assert.IsTrue(one < two);
            Assert.IsTrue(two > one);
        }

        [TestMethod]
        public void CheckLowerAndUpperOfSameValueAreEqual() {
            var lower = IntervalLimit.Lower(1, false);
            var upper = IntervalLimit.Upper(1, false);
            Assert.IsTrue(lower == upper);
        }

        [TestMethod]
        public void CanCompareIntervalLimitsWithUndefined() {
            var undefined = IntervalLimit<int>.Undefined;
            var lower = IntervalLimit.Lower(1, false);
            var upper = IntervalLimit.Upper(1, false);

            Assert.IsTrue(undefined < lower);
            Assert.IsTrue(upper < undefined);
        }
    }
}
