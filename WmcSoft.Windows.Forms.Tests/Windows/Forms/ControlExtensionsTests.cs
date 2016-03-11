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
        public void CheckDescendantsAndSelf() {
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

        [TestMethod]
        public void CheckAncestorsAndSelf() {
            Panel h;
            var root = Build("A",
                Build("B",
                    Build("C"),
                    Build("D")),
                Build("E"),
                Build("F",
                    Build("G",
                        h = Build("H"),
                        Build("I")),
                    Build("J")),
                Build("K")
            );
            var expected = "HGFA";
            var actual = String.Join("", h.AncestorsAndSelf().Select(x => x.Name));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckDescendants() {
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

            var expected = "BCDEFGHIJK";
            var actual = String.Join("", root.Descendants().Select(x => x.Name));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckAncestors() {
            Panel h;
            var root = Build("A",
                Build("B",
                    Build("C"),
                    Build("D")),
                Build("E"),
                Build("F",
                    Build("G",
                        h = Build("H"),
                        Build("I")),
                    Build("J")),
                Build("K")
            );
            var expected = "GFA";
            var actual = String.Join("", h.Ancestors().Select(x => x.Name));
            Assert.AreEqual(expected, actual);
        }
    }
}
