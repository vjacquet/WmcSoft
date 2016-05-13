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

        [TestMethod]
        [Ignore/* code is not ready*/] 
        public void CanRemoveObliqueElement() {
            var t = new Tableau();
            t.Add(7);
            t.Add(2);
            t.Add(9);
            t.Add(5);
            t.Add(3);

            t.Remove(9);

            Assert.AreEqual(2, t[0, 0]);
            Assert.AreEqual(3, t[0, 1]);
            Assert.AreEqual(5, t[1, 0]);
            Assert.AreEqual(7, t[2, 0]);
        }

        [TestMethod]
        [Ignore/* code is not ready*/]
        public void CanRemoveCornerElement() {
            var t = new Tableau();
            t.Add(7);
            t.Add(2);
            t.Add(9);
            t.Add(5);
            t.Add(3);

            t.Remove(2);

            Assert.AreEqual(3, t[0, 0]);
            Assert.AreEqual(9, t[0, 1]);
            Assert.AreEqual(5, t[1, 0]);
            Assert.AreEqual(7, t[2, 0]);
        }

        [TestMethod]
        public void CheckContains() {
            var t = new Tableau();

            t.Add(7);
            t.Add(2);
            t.Add(9);
            t.Add(5);
            t.Add(3);

            Assert.IsTrue(t.Contains(2));
            Assert.IsFalse(t.Contains(4));
        }

        [TestMethod]
        public void CheckFind() {
            var t = new Tableau();

            t.Add(7);
            t.Add(2);
            t.Add(9);
            t.Add(5);
            t.Add(3);

            int i;
            int j;
            Assert.IsTrue(t.Find(7, out i, out j));
            Assert.AreEqual(2, i);
            Assert.AreEqual(0, j);
        }
    }
}
