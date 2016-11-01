using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Time
{
    [TestClass]
    public class FixedDateSpecificationTests
    {
        [TestMethod]
        public void CanSpecifyChristmas() {
            var christmas = DateSpecification.Fixed(12, 25);

            var actual = christmas.OfYear(2016);
            var expected = new DateTime(2016, 12, 25);
            Assert.AreEqual(expected, actual);

            Assert.IsTrue(christmas.IsSatisfiedBy(actual));

            var newYearEve = new Date(2016, 12, 31);
            Assert.IsFalse(christmas.IsSatisfiedBy(newYearEve));
        }
    }
}
