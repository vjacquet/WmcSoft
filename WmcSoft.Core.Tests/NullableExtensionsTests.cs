using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class NullableExtensionsTests
    {
        [TestMethod]
        public void CheckFormattable() {
            double? d = 3.14159d;
            var expected = d.GetValueOrDefault().ToString("g");
            var actual = d.ToString("g");
            Assert.AreEqual(expected, actual);
        }

        //[TestMethod]
        //public void CheckBind() {
        //    TimeSpan? t = null;
        //    var actual = t.Bind(x => x.TotalSeconds);
        //    Assert.AreEqual(0d, actual);
        //}

        //[TestMethod]
        //public void CheckMap() {
        //    TimeSpan? t = null;
        //    var actual = t.Map(x => x.TotalSeconds);
        //    Assert.AreEqual(null, actual);
        //}
    }
}
