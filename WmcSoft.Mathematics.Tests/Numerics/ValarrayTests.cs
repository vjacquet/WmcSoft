using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Numerics.Tests
{
    [TestClass]
    public class ValarrayTests
    {
        [TestMethod]
        public void CheckRange() {
            var actual = Valarray.Range(1, 9).ToArray();
            var expected = new double[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckNegativeRange() {
            var actual = Valarray.Range(8, 0, -1).ToArray();
            var expected = new double[] { 8, 7, 6, 5, 4, 3, 2, 1 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIndices() {
            var valarray = Valarray.Range(1, 9).Reshape(2, 2, 2);

            Assert.AreEqual(8, valarray[1, 1, 1]);
        }

        [TestMethod]
        public void CheckNegativeIndices() {
            var valarray = new Valarray(
                0, 1, 2,
                10, 11, 12,
                20, 21, 22,
                30, 31, 32
            );
            valarray.Reshape(4, 3);

            Assert.AreEqual(10, valarray[1, 0]);
            Assert.AreEqual(12, valarray[1, -1]);
        }

        [TestMethod]
        public void CanCombineTwoDimensions() {
            var x = new Dimensions(2, 5, 4);
            var y = new Dimensions(3, 6);
            var expected = new Dimensions(2, 5, 6);
            Assert.AreEqual(expected, Dimensions.Combine(x, y));
        }

        [TestMethod]
        public void CanCombineADimensionsWithAnEmptyDimensions() {
            var x = new Dimensions(2, 5, 4);
            var y = new Dimensions();
            var expected = new Dimensions(2, 5, 4);
            Assert.AreEqual(expected, Dimensions.Combine(x, y));
        }

        [TestMethod]
        public void CheckAdd2DWithUnidirectionalGrowth() {
            var x = new Valarray(1, 2, 3, 4).Reshape(2, 2);
            var y = new Valarray(10, 20, 30, 40, 50, 60).Reshape(3, 2);
            var expected = new Valarray(11, 22, 33, 44, 50, 60).Reshape(3, 2);
            var actual = x + y;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckAdd2DWithBidirectionalGrowth() {
            var x = new Valarray(1, 2, 3, 4, 5, 6).Reshape(2, 3);
            var y = new Valarray(10, 20, 30, 40, 50, 60).Reshape(3, 2);
            var expected = new Valarray(11, 22, 3, 34, 45, 6, 50, 60, 0).Reshape(3, 3);
            var actual = x + y;
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void CheckBoolarrayIndices() {
            var valarray = Valarray.Range(1, 9).Reshape(2, 2, 2);
            var expected = new[] { 7d, 8d };
            var actual = valarray[valarray > 6].ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

    }
}