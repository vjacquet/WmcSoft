using System.Linq;
using System.Windows.Forms;
using Xunit;

namespace WmcSoft.Windows.Forms
{
    public class ControlExtensionsTests
    {
        Panel Build(string name, params Panel[] children)
        {
            var panel = new Panel { Name = name };
            panel.Controls.AddRange(children);
            return panel;
        }

        [Fact]
        public void CheckDescendantsAndSelf()
        {
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
            var actual = string.Join("", root.DescendantsAndSelf().Select(x => x.Name));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckAncestorsAndSelf()
        {
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
            var actual = string.Join("", h.AncestorsAndSelf().Select(x => x.Name));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckDescendants()
        {
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
            var actual = string.Join("", root.Descendants().Select(x => x.Name));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckAncestors()
        {
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
            var actual = string.Join("", h.Ancestors().Select(x => x.Name));
            Assert.Equal(expected, actual);
        }
    }
}