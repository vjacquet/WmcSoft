using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class IndexedAlgorithmsTests
    {
        [TestMethod]
        public void CheckSwapItems() {
            var expected = new[] { 1, 4, 3, 2, 5 };
            var actual = new[] { 1, 2, 3, 4, 5 };
            actual.SwapItems(1, 3);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckCopyBackwardsTo() {
            var expected = new[] { 1, 4, 3, 2, 5 };
            var data =  new[] { 1, 2, 3, 4, 5 };
            var actual = new[] { 1, 0, 0, 0, 5 };
            data.CopyBackwardsTo(1, actual, 1, 3);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
