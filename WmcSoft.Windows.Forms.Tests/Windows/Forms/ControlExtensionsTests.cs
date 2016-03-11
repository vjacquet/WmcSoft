using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Windows.Forms
{
    [TestClass]
    public class ControlExtensionsTests
    {
        Panel Build(string name, params Panel[] children) {
            var panel = new Panel { Name = name };
            panel.Controls.AddRange(children);
            return panel;
        }

        [TestMethod]
        public void CheckDescendantsOrSelf() {
            var root = Build("A",
                Build("B",
                    Build("C"),
                    Build("D")),
                Build("E"),
                Build("F",
                    Build("G",
                        Build("H"),
                        Build("I")),
                    Build("J")),
                Build("K")
            );

            var expected = "ABCDEFGHIJK";
            var actual = String.Join("", root.DescendantsAndSelf().Select(x => x.Name));
            Assert.AreEqual(expected, actual);
        }
    }
}
