using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Dynamic;

namespace WmcSoft.Linq.Expressions
{
    [TestClass]
    public class DynamicTests
    {
        [TestMethod]
        public void CanParseWhere() {
            var collection = new[] { "a", "b", "c", "D", "e", "F", "g" };
            var queryable = collection.AsQueryable();
            var result = String.Concat(queryable.Where("it.ToUpper() == it").ToArray());
            Assert.AreEqual("DF", result);
        }
    }
}
