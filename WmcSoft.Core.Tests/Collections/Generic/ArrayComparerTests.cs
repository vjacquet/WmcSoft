using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class ArrayComparerTests
    {
        [Fact]
        public void CheckArrayShapeEqualityComparer()
        {
            var a = new int[3, 2];
            var b = new int[2, 3];
            var c = new double[3, 2];
            var comparer = ArrayShapeEqualityComparer.Default;

            Assert.False(comparer.Equals(a, b));
            Assert.True(comparer.Equals(a, c));
        }

        [Fact]
        public void CheckArrayEqualityComparer()
        {
            var a = new int[3, 2];
            var b = new int[2, 3];
            var c = new int[3, 2];
            var comparer = new ArrayEqualityComparer<int>();

            Assert.False(comparer.Equals(a, b));
            Assert.True(comparer.Equals(a, c));
        }
    }
}