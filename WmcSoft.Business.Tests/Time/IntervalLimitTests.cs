using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Time
{
    /// <summary>
    /// Description résumée pour IntervalLimitTests
    /// </summary>
    [TestClass]
    public class IntervalLimitTests
    {
        [TestMethod]
        public void CanCompareIntervalLimits() {
            var one = IntervalLimit.Lower(false, 1);
            var two = IntervalLimit.Upper(false, 2);

            Assert.IsTrue(one < two);
            Assert.IsTrue(two > one);
        }

        [TestMethod]
        public void CheckLowerAndUpperOfSameValueAreEqual() {
            var lower = IntervalLimit.Lower(false, 1);
            var upper = IntervalLimit.Upper(false, 1);
            Assert.IsTrue(lower == upper);
        }

        [TestMethod]
        public void CanCompareIntervalLimitsWithUndefined() {
            var undefined = IntervalLimit<int>.Undefined;
            var lower = IntervalLimit.Lower(false, 1);
            var upper = IntervalLimit.Upper(false, 1);

            Assert.IsTrue(undefined < lower);
            Assert.IsTrue(upper < undefined);
        }
    }
}
