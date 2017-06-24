using System;
using System.Linq;
using Xunit;
using System.Linq.Dynamic;

namespace WmcSoft.Linq.Expressions
{
    public class DynamicTests
    {
        [Fact]
        public void CanParseWhere()
        {
            var collection = new[] { "a", "b", "c", "D", "e", "F", "g" };
            var queryable = collection.AsQueryable();
            var result = String.Concat(queryable.Where("it.ToUpper() == it").ToArray());
            Assert.Equal("DF", result);
        }
    }
}
