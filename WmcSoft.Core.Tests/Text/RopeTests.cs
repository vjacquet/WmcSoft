using Xunit;

namespace WmcSoft.Text
{
    public class RopeTests
    {
        [Fact]
        public void CanCreateRope()
        {
            var rope = new Rope("Hello");
            rope.Append("World");
            rope.Insert(5, " ");
            rope.Append("!");

            var expected = "Hello World!";
            Assert.Equal(expected.Length, rope.Length);
            Assert.Equal(expected, rope.ToString());
        }

        [Fact]
        public void CanSubstringRope()
        {
            var rope = new Rope("Hello");
            rope.Append("World");
            rope.Insert(5, " ");
            rope.Append("!");

            Assert.Equal("lo W", rope.Substring(3, 4));
        }
    }
}
