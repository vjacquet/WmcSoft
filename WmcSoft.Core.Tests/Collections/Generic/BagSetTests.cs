using System.Linq;
using Xunit;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    public class BagSetTests
    {
        [Fact]
        public void CheckBagSetIsCollection()
        {
            ContractAssert.Collection(new BagSet<int>());
        }

        [Fact]
        public void CanAddToBagSet()
        {
            var bag = new BagSet<int>() { 1, 2, 3, 4, 5, 6 };
            Assert.Equal(6, bag.Count);
            Assert.DoesNotContain(0, bag);
            Assert.DoesNotContain(8, bag);
            Assert.Contains(2, bag);
        }

        [Fact]
        public void CanRemoveFromBagSet()
        {
            var bag = new BagSet<int>() { 1, 2, 3, 4, 5, 6 };
            Assert.Equal(6, bag.Count);

            Assert.True(bag.Remove(4));
            Assert.False(bag.Remove(0));
            Assert.Equal(5, bag.Count);
        }

        [Fact]
        public void CanEnumerateBagSet()
        {
            var bag = new BagSet<int>() { 1, 2, 3, 4, 5, 6 };

            var expected = new[] { 1, 2, 3 };
            var actual = bag.Where(i => i < 4).ToArray();
            Assert.True(expected.Equivalent(actual));
        }

        [Fact]
        public void CanAddNullItemsToBagSet()
        {
            var bag = new BagSet<string> { "a", "b", null, "d", "e" };

            Assert.Contains("a", bag);
            Assert.DoesNotContain("c", bag);
            Assert.Contains(null, bag);
        }
    }
}
