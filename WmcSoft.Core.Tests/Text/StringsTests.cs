using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Text
{
    [TestClass]
    public class StringsTests
    {
        [TestMethod]
        public void CheckCountOnDefaultStrings() {
            var s = default(Strings);
            Assert.AreEqual(0, s.Count);
        }

        [TestMethod]
        public void CheckToArrayOnDefaultStrings() {
            var s = default(Strings);
            var actual = s.ToArray();
            var expected = new string[0];
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckToStringOnDefaultStrings() {
            var s = default(Strings);
            var actual = s.ToString();
            var expected = "";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanConcat() {
            Strings a = "a";
            Strings b = "b";
            Strings c = new[] { "c", "d" };

            var actual = Strings.Concat(a, b);
            CollectionAssert.AreEqual(new[] { "a", "b" }, (string[])actual);

            actual = Strings.Concat(actual, c);
            Assert.AreEqual("a,b,c,d", (string)actual);
        }

        [TestMethod]
        public void CanZip() {
            Strings a = "a";
            Strings b = "b";
            Strings e = "";
            Strings d = default(Strings);

            CollectionAssert.AreEqual(new[] { "ab" }, (string[])Strings.Zip(a, b));
            CollectionAssert.AreEqual(new[] { "a" }, (string[])Strings.Zip(a, e));
            CollectionAssert.AreEqual(new[] { "b" }, (string[])Strings.Zip(e, b));
            CollectionAssert.AreEqual(new string[0], (string[])Strings.Zip(d, b));
        }

        [TestMethod]
        public void CanConstructStrings() {
            var a = new Strings("a");
            Assert.AreEqual(1, a.Count);
        }

        [TestMethod]
        public void CanCheckInConditionals() {
            var empty = default(Strings);
            if (empty)
                Assert.Fail();

            Strings value = "value";
            if (!value)
                Assert.Fail();
        }

    }
}
