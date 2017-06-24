using Xunit;

namespace WmcSoft
{
    public class NullableExtensionsTests
    {
        [Fact]
        public void CheckFormattable()
        {
            double? d = 3.14159d;
            var expected = d.GetValueOrDefault().ToString("g");
            var actual = d.ToString("g");
            Assert.Equal(expected, actual);
        }

        //[Fact]
        //public void CheckBind() {
        //    TimeSpan? t = null;
        //    var actual = t.Bind(x => x.TotalSeconds);
        //    Assert.Equal(0d, actual);
        //}

        //[Fact]
        //public void CheckMap() {
        //    TimeSpan? t = null;
        //    var actual = t.Map(x => x.TotalSeconds);
        //    Assert.Equal(null, actual);
        //}
    }
}
