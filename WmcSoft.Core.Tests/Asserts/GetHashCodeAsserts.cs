using System;
using Xunit;

namespace WmcSoft.Asserts
{
    public class GetHashCodeAsserts
    {
        [Fact]
        public void CanGetHashCodeOfNullableType()
        {
            var x = default(DateTime?);

            Assert.True(x == null);
            Assert.Equal(0, x.GetHashCode());
        }

        [Fact]
        public void CannotGetHashCodeOfNullStringFromOrdinalIgnoreCase()
        {
            Assert.Throws<ArgumentNullException>(() => StringComparer.OrdinalIgnoreCase.GetHashCode(null));
        }

        [Fact]
        public void CannotGetHashCodeOfNullStringFromInvariantCulture()
        {
            Assert.Throws<ArgumentNullException>(() => StringComparer.InvariantCulture.GetHashCode(null));
        }

        [Fact]
        public void CannotGetHashCodeOfNullStringFromInvariantCultureIgnoreCase()
        {
            Assert.Throws<ArgumentNullException>(() => StringComparer.InvariantCultureIgnoreCase.GetHashCode(null));
        }
    }
}
