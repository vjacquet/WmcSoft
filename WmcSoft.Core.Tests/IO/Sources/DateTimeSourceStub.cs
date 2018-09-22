using System;

namespace WmcSoft.IO.Sources
{
    class DateTimeSourceStub : IDateTimeSource
    {
        private DateTime now;

        public DateTimeSourceStub(int year, int month = 1, int day = 1, int hour = 0, int minute = 0, int second = 0)
        {
            now = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        }

        public DateTime Now()
        {
            return now;
        }

        public void Advance(TimeSpan timeSpan)
        {
            now += timeSpan;
        }
    }
}
