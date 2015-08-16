using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Text.RegularExpressions;

namespace WmcSoft
{
    [TestClass]
    public class RegexExtensionsTests
    {
        [TestMethod]
        public void CanGetGroupValue() {
            var regex = new Regex(@"^(?<n>\d+)");
            var m = regex.Match("42");
            var actual = m.GetGroupValue<int>("n");
            Assert.AreEqual(42, actual);
        }

        [TestMethod]
        public void CheckMissingGroupValueIsNull() {
            var regex = new Regex(@"^(?<n>\d+)");
            var m = regex.Match("abc");
            var actual = m.GetGroupValue<int>("n");
            Assert.AreEqual(null, actual);
        }
    }
}
