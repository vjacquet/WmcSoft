using System;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class SingleItemListTests
    {
        [Fact]
        public void CheckIndexer()
        {
            var list = new SingleItemList<int>(5);
            Assert.Single(list);
            Assert.Equal(5, list[0]);
        }

        [Fact]
        public void CheckInsert()
        {
            var list = new SingleItemList<int>(5);
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(1, 3));
        }

        [Fact]
        public void CheckInsertOnEmpty()
        {
            var list = new SingleItemList<int>();
            list.Insert(0, 3);
        }
    }
}
