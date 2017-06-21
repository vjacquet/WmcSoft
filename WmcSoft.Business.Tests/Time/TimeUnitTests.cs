using System;
using Xunit;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Time
{
    public class TimeUnitTests
    {
        [Fact]
        public void AreTimeUnitsSorted()
        {
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

            Assert.True(units.IsSorted());
        }

        [Fact]
        public void CanFindNextFinerUnit()
        {
            Assert.Equal(TimeUnit.Day, TimeUnit.Week.NextFinerUnit());
            Assert.Equal(TimeUnit.Hour, TimeUnit.Day.NextFinerUnit());
            Assert.Equal(TimeUnit.Minute, TimeUnit.Hour.NextFinerUnit());
            Assert.Equal(TimeUnit.Second, TimeUnit.Minute.NextFinerUnit());
            Assert.Equal(TimeUnit.Millisecond, TimeUnit.Second.NextFinerUnit());
            Assert.Equal(TimeUnit.Millisecond, TimeUnit.Millisecond.NextFinerUnit());

            Assert.Equal(TimeUnit.Quarter, TimeUnit.Year.NextFinerUnit());
            Assert.Equal(TimeUnit.Month, TimeUnit.Quarter.NextFinerUnit());
            Assert.Equal(TimeUnit.Month, TimeUnit.Month.NextFinerUnit());
        }

        [Fact]
        public void CheckBaseUnits()
        {
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
            Assert.Equal(expected, actual);
        }
    }
}