using Xunit;

namespace WmcSoft
{
    public class TableauTests
    {
        [Fact]
        public void CanAdd()
        {
            var t = new Tableau();

            t.Add(7);
            Assert.Equal(7, t[0, 0]);

            t.Add(2);
            Assert.Equal(2, t[0, 0]);
            Assert.Equal(7, t[1, 0]);

            t.Add(9);
            Assert.Equal(2, t[0, 0]);
            Assert.Equal(7, t[1, 0]);
            Assert.Equal(9, t[0, 1]);

            t.Add(5);
            Assert.Equal(2, t[0, 0]);
            Assert.Equal(5, t[0, 1]);
            Assert.Equal(7, t[1, 0]);
            Assert.Equal(9, t[1, 1]);

            t.Add(3);
            Assert.Equal(2, t[0, 0]);
            Assert.Equal(3, t[0, 1]);
            Assert.Equal(5, t[1, 0]);
            Assert.Equal(9, t[1, 1]);
            Assert.Equal(7, t[2, 0]);
        }

        [Fact(Skip = "Code is not ready")]
        public void CanRemoveObliqueElement()
        {
            var t = new Tableau();
            t.Add(7);
            t.Add(2);
            t.Add(9);
            t.Add(5);
            t.Add(3);

            t.Remove(9);

            Assert.Equal(2, t[0, 0]);
            Assert.Equal(3, t[0, 1]);
            Assert.Equal(5, t[1, 0]);
            Assert.Equal(7, t[2, 0]);
        }

        [Fact(Skip = "Code is not ready")]
        public void CanRemoveCornerElement()
        {
            var t = new Tableau();
            t.Add(7);
            t.Add(2);
            t.Add(9);
            t.Add(5);
            t.Add(3);

            t.Remove(2);

            Assert.Equal(3, t[0, 0]);
            Assert.Equal(9, t[0, 1]);
            Assert.Equal(5, t[1, 0]);
            Assert.Equal(7, t[2, 0]);
        }

        [Fact]
        public void CheckContains()
        {
            var t = new Tableau();

            t.Add(7);
            t.Add(2);
            t.Add(9);
            t.Add(5);
            t.Add(3);

            Assert.Contains(2, t);
            Assert.DoesNotContain(4, t);
        }

        [Fact]
        public void CheckFind()
        {
            var t = new Tableau();

            t.Add(7);
            t.Add(2);
            t.Add(9);
            t.Add(5);
            t.Add(3);

            int i;
            int j;
            Assert.True(t.Find(7, out i, out j));
            Assert.Equal(2, i);
            Assert.Equal(0, j);
        }
    }
}
