using System;
using System.Globalization;
using Xunit;

namespace WmcSoft
{
    public class DateTimeTests
    {
        [Theory]
        [InlineData(5, 2009, 9, 30)]
        [InlineData(1, 2009, 10, 1)]
        [InlineData(1, 2009, 10, 3)]
        [InlineData(2, 2009, 10, 4)]
        [InlineData(2, 2009, 10, 5)]
        [InlineData(3, 2009, 10, 12)]
        [InlineData(5, 2009, 10, 25)]
        [InlineData(5, 2009, 10, 31)]
        [InlineData(1, 2009, 11, 1)]
        public void CheckWeekOfMonth(int expected, int year, int month, int day)
        {
            var actual = new DateTime(year, month, day);
            Assert.Equal(expected, actual.WeekOfMonth());
        }

        [Fact]
        public void CanFindFirstDayOfWeek()
        {
            var date = new DateTime(2016, 08, 10);
            var info = new DateTimeFormatInfo {
                FirstDayOfWeek = DayOfWeek.Monday,
            };
            var expected = new DateTime(2016, 08, 08);
            var actual = date.FirstDayOfWeek(info);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanFindLastDayOfWeek()
        {
            var date = new DateTime(2016, 08, 10);
            var info = new DateTimeFormatInfo {
                FirstDayOfWeek = DayOfWeek.Monday,
            };
            var expected = new DateTime(2016, 08, 14);
            var actual = date.LastDayOfWeek(info);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanFindFirstDayOfMonth()
        {
            var date = new DateTime(2016, 08, 10);
            var expected = new DateTime(2016, 08, 01);
            var actual = date.FirstDayOfMonth();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanFindLastDayOfMonth()
        {
            var date = new DateTime(2016, 02, 10);
            var expected = new DateTime(2016, 02, 29);
            var actual = date.LastDayOfMonth();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AssertUnspecifiedKindOnNewDateTime()
        {
            var unspecified = new DateTime(2016, 01, 01, 12, 0, 0);
            Assert.Equal(DateTimeKind.Unspecified, unspecified.Kind);
        }

        [Fact]
        public void CheckAsLocalTime()
        {
            var unspecified = new DateTime(2016, 01, 01, 12, 0, 0);
            var local = new DateTime(2016, 01, 01, 12, 0, 0, DateTimeKind.Local);
            var utc = new DateTime(2016, 01, 01, 12, 0, 0, DateTimeKind.Utc);

            Assert.Equal(local.ToLocalTime(), local.AsLocalTime());
            Assert.Equal(utc.ToLocalTime(), utc.AsLocalTime());
            Assert.NotEqual(unspecified.ToLocalTime(), unspecified.AsLocalTime());
        }

        [Fact]
        public void CheckAsUniversalTime()
        {
            var unspecified = new DateTime(2016, 01, 01, 12, 0, 0);
            var local = new DateTime(2016, 01, 01, 12, 0, 0, DateTimeKind.Local);
            var utc = new DateTime(2016, 01, 01, 12, 0, 0, DateTimeKind.Utc);

            Assert.Equal(local.ToUniversalTime(), local.AsUniversalTime());
            Assert.Equal(utc.ToUniversalTime(), utc.AsUniversalTime());
            Assert.NotEqual(unspecified.ToUniversalTime(), unspecified.AsUniversalTime());
        }
    }
}
