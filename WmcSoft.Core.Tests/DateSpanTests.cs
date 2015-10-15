using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class DateSpanTests
    {
        [TestMethod]
        public void CheckLessThan() {
            var x = new DateSpan(months: 1);
            var y = new DateSpan(days: 31);
            Assert.IsTrue(x < y);
        }

        [TestMethod]
        public void CheckEquals() {
            var x = new DateSpan(weeks: 1);
            var y = new DateSpan(days: 7);
            Assert.IsTrue(x == y);
        }
    }
}
