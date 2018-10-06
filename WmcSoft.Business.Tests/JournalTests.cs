using System;
using Xunit;

namespace WmcSoft.Business
{
    public class JournalTests
    {
        class Clock
        {
            private DateTime now;

            public Clock(DateTime date)
            {
                now = date;
            }

            public Clock(int year, int month, int day)
                : this(new DateTime(year, month, day, 12, 0, 0, DateTimeKind.Utc))
            {
            }

            public Clock(int year, int month, int day, int hour, int minute)
                : this(new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Utc))
            {
            }

            public DateTime Now()
            {
                return now;
            }

            public void Wait(TimeSpan timeSpan)
            {
                now = now.Add(timeSpan);
            }
        }

        [Fact]
        public void CanRecordEvents()
        {
            var clock = new Clock(2001, 01, 01);

            var journal = new Journal<string>();
            journal.Record(clock.Now(), "A");

            clock.Wait(TimeSpan.FromMinutes(30));
            journal.Record(clock.Now(), "B");
            clock.Wait(TimeSpan.FromMinutes(30));
            journal.Record(clock.Now(), "C");

            Assert.Equal("ABC", string.Concat(journal));
        }

        [Theory]
        [InlineData("2000-01-01", null)]
        [InlineData("2001-01-01", "A")]
        [InlineData("2001-01-31", "A")]
        [InlineData("2001-06-01", "B")]
        [InlineData("2001-07-01", "B")]
        [InlineData("2001-12-31", "C")]
        [InlineData("2002-01-01", null)]
        [InlineData("2002-02-01", null)]
        public void CanAccessEvent(DateTime date, string expected)
        {
            var journal = new Journal<string>();
            journal.Record(new DateTime(2001, 01, 01), "A");
            journal.Record(new DateTime(2001, 06, 01), "B");
            journal.Record(new DateTime(2001, 12, 01), "C");
            journal.Close(new DateTime(2002, 01, 01));

            Assert.Equal(expected, journal[date]);
        }
    }
}
