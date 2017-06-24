using Xunit;

namespace WmcSoft.Text
{
    public class StringsTests
    {
        [Fact]
        public void CheckCountOnDefaultStrings()
        {
            var s = default(Strings);
            Assert.Equal(0, s.Count);
        }

        [Fact]
        public void CheckToArrayOnDefaultStrings()
        {
            var s = default(Strings);
            var actual = s.ToArray();
            var expected = new string[0];
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckToStringOnDefaultStrings()
        {
            var s = default(Strings);
            var actual = s.ToString();
            var expected = "";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanConcat()
        {
            Strings a = "a";
            Strings b = "b";
            Strings c = new[] { "c", "d" };

            var actual = Strings.Concat(a, b);
            Assert.Equal(new[] { "a", "b" }, (string[])actual);

            actual = Strings.Concat(actual, c);
            Assert.Equal("a,b,c,d", (string)actual);
        }

        [Fact]
        public void CanZip()
        {
            Strings a = "a";
            Strings b = "b";
            Strings e = "";
            Strings d = default(Strings);

            Assert.Equal(new[] { "ab" }, (string[])Strings.Zip(a, b));
            Assert.Equal(new[] { "a" }, (string[])Strings.Zip(a, e));
            Assert.Equal(new[] { "b" }, (string[])Strings.Zip(e, b));
            Assert.Equal(new string[0], (string[])Strings.Zip(d, b));
        }

        [Fact]
        public void CanConstructStrings()
        {
            var a = new Strings("a");
            Assert.Equal(1, a.Count);
        }

        [Fact]
        public void CanCheckInConditionals()
        {
            var empty = default(Strings);
            if (empty)
                Assert.True(false, "Fail");

            Strings value = "value";
            if (!value)
                Assert.True(false, "Fail");
        }

        [Fact]
        public void CheckCheckInCombinedConditionals()
        {
            Strings a = "a";
            Strings b = "b";
            Strings d = default(Strings);

            if (a && d)
                Assert.True(false, "Fail");

            if (a && b)
                Noop();
            else
                Assert.True(false, "Fail");

            if (a || d)
                Noop();
            else
                Assert.True(false, "Fail");
        }

        static void Noop()
        {
        }
    }
}
