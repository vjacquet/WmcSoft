using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Time
{
    [TestClass]
    public class DurationTests
    {
        [TestMethod]
        public void CheckToString() {
            var duration = new Duration(1, 2, 3, 4, 5);
            var actual = duration.ToString();
            var expected = "1 day, 2 hours, 3 minutes, 4 seconds, 5 milliseconds";
            Assert.AreEqual(expected,actual);
        }
    }
}
