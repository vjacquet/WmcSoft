using System;
using System.Collections.Generic;
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

        static List<DateTime>[] GenerateCalendar(DateTime first)
        {
            var calendar = new List<DateTime>[7];
            for (int i = 0; i < 7; i++) {
                calendar[i] = new List<DateTime>();
            }

            var last = first.AddMonths(1);
            while (first < last) {
                var dow = (int)first.DayOfWeek;
                calendar[dow].Add(first);
                first = first.AddDays(1);
            }
            return calendar;
        }

        [Theory]
        [InlineData(2018, 04)] // starts on Sunday
        [InlineData(2018, 01)] // starts on Monday
        [InlineData(2018, 05)] // starts on Tuesday
        [InlineData(2018, 08)] // starts on Wednesday
        [InlineData(2018, 02)] // starts on Thursday
        [InlineData(2018, 06)] // starts on Friday
        [InlineData(2018, 09)] // starts on Saturday
        public void CheckPositiveNthDayOfMonth(int year, int month)
        {
            var first = new DateTime(year, month, 1);
            var calendar = GenerateCalendar(first);
            for (int i = 0; i < 7; i++) {
                var dow = (DayOfWeek)i;
                var n = 1;
                foreach (var expected in calendar[i]) {
                    Assert.Equal(expected, first.NthDayOfMonth(n++, dow));
                }
                Assert.Throws<ArgumentOutOfRangeException>(() => {
                    first.NthDayOfMonth(n, dow);
                });
            }
        }

        [Theory]
        [InlineData(2026, 02)] // starts on Sunday and has 28 days
        [InlineData(2021, 02)] // starts on Monday and has 28 days
        [InlineData(2022, 02)] // starts on Tuesday and has 28 days
        [InlineData(2023, 02)] // starts on Wednesday and has 28 days
        [InlineData(2018, 02)] // starts on Thursday and has 28 days
        [InlineData(2019, 02)] // starts on Friday and has 28 days
        [InlineData(2025, 02)] // starts on Saturday and has 28 days
        [InlineData(2032, 02)] // starts on Sunday and has 29 days
        [InlineData(2044, 02)] // starts on Monday and has 29 days
        [InlineData(2028, 02)] // starts on Tuesday and has 29 days
        [InlineData(2040, 02)] // starts on Wednesday and has 29 days
        [InlineData(2024, 02)] // starts on Thursday and has 29 days
        [InlineData(2036, 02)] // starts on Friday and has 29 days
        [InlineData(2020, 02)] // starts on Saturday and has 29 days
        [InlineData(2018, 04)] // starts on Sunday and has 30 days
        [InlineData(2019, 04)] // starts on Monday and has 30 days
        [InlineData(2020, 09)] // starts on Tuesday and has 30 days
        [InlineData(2020, 04)] // starts on Wednesday and has 30 days
        [InlineData(2018, 11)] // starts on Thursday and has 30 days
        [InlineData(2018, 06)] // starts on Friday and has 30 days
        [InlineData(2018, 09)] // starts on Saturday and has 30 days
        [InlineData(2018, 07)] // starts on Sunday and has 31 days
        [InlineData(2018, 01)] // starts on Monday and has 31 days
        [InlineData(2018, 05)] // starts on Tuesday and has 31 days
        [InlineData(2018, 08)] // starts on Wednesday and has 31 days
        [InlineData(2018, 03)] // starts on Thursday and has 31 days
        [InlineData(2019, 03)] // starts on Friday and has 31 days
        [InlineData(2018, 12)] // starts on Saturday and has 31 days
        public void CheckNegativeNthDayOfMonth(int year, int month)
        {
            var first = new DateTime(year, month, 1);
            var calendar = GenerateCalendar(first);
            for (int i = 0; i < 7; i++) {
                calendar[i].Reverse();
                var dow = (DayOfWeek)i;
                var n = -1;
                foreach (var expected in calendar[i]) {
                    Assert.Equal(expected, first.NthDayOfMonth(n--, dow));
                }
                Assert.Throws<ArgumentOutOfRangeException>(() => {
                    first.NthDayOfMonth(n, dow);
                });
            }
        }
    }
}
