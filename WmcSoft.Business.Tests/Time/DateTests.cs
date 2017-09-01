using System;
using Xunit;

namespace WmcSoft.Time
{
    public class DateTests
    {
        [Fact]
        public void AssertDateIsOfUnspecifiedKind()
        {
            var manual = new Date(1973, 5, 2);
            Assert.Equal(DateTimeKind.Unspecified, ((DateTime)manual).Kind);

            var dateTime = new DateTime(1973, 5, 2, 15, 30, 25, DateTimeKind.Local);
            var fromDateTime = (Date)dateTime;
            Assert.Equal(DateTimeKind.Unspecified, ((DateTime)fromDateTime).Kind);
        }

        [Fact]
        public void CheckAsTimepoint()
        {
            var manual = new Date(1973, 5, 2);
            var zone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
            var tp = manual.AsTimepoint(zone);
            var dateTimeOffset = (DateTimeOffset)tp;
            var actual = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.DateTime, zone);
            Assert.Equal(1973, actual.Year);
            Assert.Equal(5, actual.Month);
            Assert.Equal(2, actual.Day);
        }
    }
}
