using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Text
{
    [TestClass]
    public class RopeTests
    {
        [TestMethod]
        public void CanCreateRope()
        {
            var rope = new Rope("Hello");
            rope.Append("World");
            rope.Insert(5, " ");
            rope.Append("!");

            var expected = "Hello World!";
            Assert.AreEqual(expected.Length, rope.Length);
            Assert.AreEqual(expected, rope.ToString());
        }

        [TestMethod]
        public void CanSubstringRope()
        {
            var rope = new Rope("Hello");
            rope.Append("World");
            rope.Insert(5, " ");
            rope.Append("!");

            Assert.AreEqual("lo W", rope.Substring(3, 4));
        }
    }
}
