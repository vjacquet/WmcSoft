using System;
using System.Linq;
using Xunit;

namespace WmcSoft.Numerics
{
    public class ValarrayTests
    {
        [Fact]
        public void CanDeconstructDimensions1()
        {
            var d = new Dimensions(1);
            var n = (int)d;
            Assert.Equal(1, n);
        }

        [Fact]
        public void CanDeconstructDimensions2()
        {
            var d = new Dimensions(1, 2);
            var (m, n) = d;
            Assert.Equal(1, m);
            Assert.Equal(2, n);
        }

        [Fact]
        public void CanDeconstructDimensions3()
        {
            var d = new Dimensions(1, 2, 3);
            var (m, n, o) = d;
            Assert.Equal(1, m);
            Assert.Equal(2, n);
            Assert.Equal(3, o);
        }

        [Fact]
        public void CanDeconstructDimensions4()
        {
            var d = new Dimensions(1, 2, 3, 4);
            var (m, n, o, p) = d;
            Assert.Equal(1, m);
            Assert.Equal(2, n);
            Assert.Equal(3, o);
            Assert.Equal(4, p);
        }

        [Fact]
        public void CanDeconstructDimensions5()
        {
            var d = new Dimensions(1, 2, 3, 4, 5);
            var (m, n, o, p, q) = d;
            Assert.Equal(1, m);
            Assert.Equal(2, n);
            Assert.Equal(3, o);
            Assert.Equal(4, p);
            Assert.Equal(5, q);
        }

        [Fact]
        public void CanDeconstructHomogenousDimensionsAsScalar()
        {
            var d = new Dimensions(3, 3, 3);
            var n = (int)d;
            Assert.Equal(3, n);
        }

        [Fact]
        public void CannotDeconstructHeteogenousDimensionsAsScalar()
        {
            var d = new Dimensions(3, 3, 1);
            Assert.Throws<InvalidCastException>(() => {
                var n = (int)d;
            });
        }

        [Fact]
        public void CheckRange()
        {
            var actual = Valarray.Range(1, 9).ToArray();
            var expected = new double[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckNegativeRange()
        {
            var actual = Valarray.Range(8, 0, -1).ToArray();
            var expected = new double[] { 8, 7, 6, 5, 4, 3, 2, 1 };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckIndices()
        {
            var valarray = Valarray.Range(1, 9).Reshape(2, 2, 2);

            Assert.Equal(8, valarray[1, 1, 1]);
        }

        [Fact]
        public void CheckNegativeIndices()
        {
            var valarray = new Valarray(
                0, 1, 2,
                10, 11, 12,
                20, 21, 22,
                30, 31, 32
            );
            valarray.Reshape(4, 3);

            Assert.Equal(10, valarray[1, 0]);
            Assert.Equal(12, valarray[1, -1]);
        }

        [Fact]
        public void CanCombineTwoDimensions()
        {
            var x = new Dimensions(2, 5, 4);
            var y = new Dimensions(3, 6);
            var expected = new Dimensions(2, 5, 6);
            Assert.Equal(expected, Dimensions.Combine(x, y));
        }

        [Fact]
        public void CanCombineADimensionsWithAnEmptyDimensions()
        {
            var x = new Dimensions(2, 5, 4);
            var y = new Dimensions();
            var expected = new Dimensions(2, 5, 4);
            Assert.Equal(expected, Dimensions.Combine(x, y));
        }

        [Fact]
        public void CheckAdd2DWithUnidirectionalGrowth()
        {
            var x = new Valarray(1, 2, 3, 4).Reshape(2, 2);
            var y = new Valarray(10, 20, 30, 40, 50, 60).Reshape(3, 2);
            var expected = new Valarray(11, 22, 33, 44, 50, 60).Reshape(3, 2);
            var actual = x + y;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckAdd2DWithBidirectionalGrowth()
        {
            var x = new Valarray(1, 2, 3, 4, 5, 6).Reshape(2, 3);
            var y = new Valarray(10, 20, 30, 40, 50, 60).Reshape(3, 2);
            var expected = new Valarray(11, 22, 3, 34, 45, 6, 50, 60, 0).Reshape(3, 3);
            var actual = x + y;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckBoolarrayIndices()
        {
            var valarray = Valarray.Range(1, 9).Reshape(2, 2, 2);
            var expected = new[] { 7d, 8d };
            var actual = valarray[valarray > 6].ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckBoolarrayMasking()
        {
            var valarray = Valarray.Range(1, 9).Reshape(2, 2, 2);
            var backwards = Valarray.Range(8, 0, -1).Reshape(2, 2, 2);
            var mask = new Boolarray(false, true, true, false, false, false, true, false);
            var expected = new[] { 1d, 7d, 6d, 4d, 5d, 6d, 2d, 8d };
            valarray.Assign(mask, backwards);
            var actual = valarray.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCreateEmptyValarray()
        {
            var valarray = new Valarray();
            Assert.NotNull(valarray);
            Assert.Equal(0, valarray.Cardinality);
            Assert.NotSame(Valarray.Empty, valarray);
        }
    }
}
