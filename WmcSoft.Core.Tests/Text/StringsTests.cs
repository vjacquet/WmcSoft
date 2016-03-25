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
    }
}
