using Xunit;

namespace WmcSoft.Time
{
    public class DurationTests
    {
        [Fact]
        public void CheckToString()
        {
            var duration = new Duration(1, 2, 3, 4, 5);
            var actual = duration.ToString();
            var expected = "1 day, 2 hours, 3 minutes, 4 seconds, 5 milliseconds";
            Assert.Equal(expected, actual);
        }
    }
}