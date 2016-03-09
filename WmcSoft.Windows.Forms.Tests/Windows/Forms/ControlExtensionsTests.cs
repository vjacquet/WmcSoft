using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Windows.Forms
{
    [TestClass]
    public class ControlExtensionsTests
    {
        [TestMethod]
        public void CheckDescendantsOrSelf() {
            var a = new Panel { Tag = "A" };
            var b = new Panel { Tag = "B" };
            var c = new Panel { Tag = "C" };
            var d = new Panel { Tag = "D" };
            var e = new Panel { Tag = "E" };
            var f = new Panel { Tag = "F" };
            var g = new Panel { Tag = "G" };
            var h = new Panel { Tag = "H" };
            var i = new Panel { Tag = "I" };
            var j = new Panel { Tag = "J" };
            var k = new Panel { Tag = "K" };

            b.Controls.AddRange(new Control[] { c, d });
            g.Controls.AddRange(new Control[] { h, i });
            f.Controls.AddRange(new Control[] { g, j });
            a.Controls.AddRange(new Control[] { b, e, f, k });

            var expected = "ABCDEFGHIJK";
            var actual = String.Join("", a.DescendantsAndSelf().Select(x => x.Tag));
            Assert.AreEqual(expected, actual);
        }
    }
}
