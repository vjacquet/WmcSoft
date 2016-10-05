using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Time
{
    [TestClass]
    public class TimeUnitTests
    {
        [TestMethod]
        public void AreTimeUnitsSorted() {
            var units = new[] {
                TimeUnit.Millisecond,
                TimeUnit.Second,
                TimeUnit.Minute,
                TimeUnit.Hour,
                TimeUnit.Day,
                TimeUnit.Week,

                TimeUnit.Month,
                TimeUnit.Quarter,
                TimeUnit.Year,
            };

            Assert.IsTrue(units.IsSorted());
        }

        [TestMethod]
        public void CanFindNextFinerUnit() {
            Assert.AreEqual(TimeUnit.Day, TimeUnit.Week.NextFinerUnit());
            Assert.AreEqual(TimeUnit.Hour, TimeUnit.Day.NextFinerUnit());
            Assert.AreEqual(TimeUnit.Minute, TimeUnit.Hour.NextFinerUnit());
            Assert.AreEqual(TimeUnit.Second, TimeUnit.Minute.NextFinerUnit());
            Assert.AreEqual(TimeUnit.Millisecond, TimeUnit.Second.NextFinerUnit());
            Assert.AreEqual(TimeUnit.Millisecond, TimeUnit.Millisecond.NextFinerUnit());

            Assert.AreEqual(TimeUnit.Quarter, TimeUnit.Year.NextFinerUnit());
            Assert.AreEqual(TimeUnit.Month, TimeUnit.Quarter.NextFinerUnit());
            Assert.AreEqual(TimeUnit.Month, TimeUnit.Month.NextFinerUnit());
        }

        [TestMethod]
        public void CheckBaseUnits() {
            var units = new[] {
                TimeUnit.Millisecond,
                TimeUnit.Second,
                TimeUnit.Minute,
                TimeUnit.Hour,
                TimeUnit.Day,
                TimeUnit.Week,

                TimeUnit.Month,
                TimeUnit.Quarter,
                TimeUnit.Year,
            };
            var expected = new[] {
                TimeUnit.Millisecond,
                TimeUnit.Millisecond,
                TimeUnit.Millisecond,
                TimeUnit.Millisecond,
                TimeUnit.Millisecond,
                TimeUnit.Millisecond,

                TimeUnit.Month,
                TimeUnit.Month,
                TimeUnit.Month,
            };

            var actual = Array.ConvertAll(units, _ => _.BaseUnit);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
