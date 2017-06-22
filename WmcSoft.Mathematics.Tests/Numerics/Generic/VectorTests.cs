using Xunit;

using Vect = WmcSoft.Numerics.Generic.Vector<int, WmcSoft.Arithmetics.Int32Arithmetics>;

namespace WmcSoft.Numerics.Generic.Tests
{
    public class VectorTests
    {
        [Fact]
        public void CheckAdd()
        {
            var u = new Vect(1, 2, 3);
            var v = new Vect(1, 2, 3);
            var expected = new Vect(2, 4, 6);
            Assert.Equal(expected, u + v);
        }

        [Fact]
        public void CheckDotProduct()
        {
            var u = new Vect(1, 2, 3);
            var v = new Vect(1, 2, 3);
            var expected = 14d;
            Assert.Equal(expected, Vect.DotProduct(u, v));
        }
    }
}
