using System;
using Xunit;
using WmcSoft.Time;
using WmcSoft.Business.Calendars.Specifications;

namespace WmcSoft.Business.Calendars
{
    public class BusinessDayConventionTests : IClassFixture<BusinessDayConventionTests.Fixture>
    {
        public class Fixture
        {
            public Fixture()
            {
                Calendar = new BusinessCalendar(new Date(2017, 01, 01), new Date(2017, 12, 31)
                    , new WeekendSpecification(DayOfWeek.Saturday, DayOfWeek.Sunday));
            }

            public BusinessCalendar Calendar;
        }

        readonly BusinessCalendar Calendar;

        public BusinessDayConventionTests(Fixture fixture)
        {
            Calendar = fixture.Calendar;
        }

        [Theory]
        [InlineData("2017-09-22", "2017-09-22")]
        [InlineData("2017-09-23", "2017-09-23")]
        [InlineData("2017-09-30", "2017-09-30")]
        public void CanAdjustWithUnadjusted(DateTime date, DateTime adjusted)
        {
            Date expected = adjusted;
            Date actual = BusinessDayConventions.Unadjusted.Adjust(Calendar, date);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2017-09-22", "2017-09-22")]
        [InlineData("2017-09-23", "2017-09-25")]
        [InlineData("2017-09-30", "2017-10-02")]
        public void CanAdjustWithFollowing(DateTime date, DateTime adjusted)
        {
            Date expected = adjusted;
            Date actual = BusinessDayConventions.Following.Adjust(Calendar, date);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2017-09-22", "2017-09-22")]
        [InlineData("2017-09-23", "2017-09-25")]
        [InlineData("2017-09-30", "2017-09-29")]
        public void CanAdjustWithModifiedFollowing(DateTime date, DateTime adjusted)
        {
            Date expected = adjusted;
            Date actual = BusinessDayConventions.ModifiedFollowing.Adjust(Calendar, date);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2017-09-22", "2017-09-22")]
        [InlineData("2017-09-23", "2017-09-22")]
        [InlineData("2017-09-24", "2017-09-22")]
        [InlineData("2017-07-02", "2017-06-30")]
        [InlineData("2017-07-01", "2017-06-30")]
        public void CanAdjustWithPreceding(DateTime date, DateTime adjusted)
        {
            Date expected = adjusted;
            Date actual = BusinessDayConventions.Preceding.Adjust(Calendar, date);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2017-09-22", "2017-09-22")]
        [InlineData("2017-09-23", "2017-09-22")]
        [InlineData("2017-09-24", "2017-09-22")]
        [InlineData("2017-07-02", "2017-07-03")]
        [InlineData("2017-07-01", "2017-07-03")]
        public void CanAdjustWithModifiedPreceding(DateTime date, DateTime adjusted)
        {
            Date expected = adjusted;
            Date actual = BusinessDayConventions.ModifiedPreceding.Adjust(Calendar, date);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2017-10-14", "2017-10-13")]
        [InlineData("2017-09-16", "2017-09-18")]
        public void CanAdjustWithHalfMonthModifiedFollowing(DateTime date, DateTime adjusted)
        {
            Date expected = adjusted;
            Date actual = BusinessDayConventions.HalfMonthModifiedFollowing.Adjust(Calendar, date);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2017-09-22", "2017-09-22")]
        [InlineData("2017-09-23", "2017-09-22")]
        [InlineData("2017-09-24", "2017-09-25")]
        [InlineData("2017-09-25", "2017-09-25")]
        public void CanAdjustWithNearest(DateTime date, DateTime adjusted)
        {
            Date expected = adjusted;
            Date actual = BusinessDayConventions.Nearest.Adjust(Calendar, date);
            Assert.Equal(expected, actual);
        }
    }
}
