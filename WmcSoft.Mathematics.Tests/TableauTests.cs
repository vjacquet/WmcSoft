using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class TableauTests
    {
        [TestMethod]
        public void CanAdd() {
            var t = new Tableau();

            t.Add(7);
            Assert.AreEqual(7, t[0, 0]);

            t.Add(2);
            Assert.AreEqual(2, t[0, 0]);
            Assert.AreEqual(7, t[1, 0]);

            t.Add(9);
            Assert.AreEqual(2, t[0, 0]);
            Assert.AreEqual(7, t[1, 0]);
            Assert.AreEqual(9, t[0, 1]);

            t.Add(5);
            Assert.AreEqual(2, t[0, 0]);
            Assert.AreEqual(5, t[0, 1]);
            Assert.AreEqual(7, t[1, 0]);
            Assert.AreEqual(9, t[1, 1]);

            t.Add(3);
            Assert.AreEqual(2, t[0, 0]);
            Assert.AreEqual(3, t[0, 1]);
            Assert.AreEqual(5, t[1, 0]);
            Assert.AreEqual(9, t[1, 1]);
            Assert.AreEqual(7, t[2, 0]);
        }
    }
}
