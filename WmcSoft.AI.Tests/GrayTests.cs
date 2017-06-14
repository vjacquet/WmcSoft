using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.AI
{
    [TestClass]
    public class GrayTests
    {
        [TestMethod]
        public void CheckGray8()
        {
            var actual = new[] {
                new Gray8(0), new Gray8(1), new Gray8(2), new Gray8(3),
                new Gray8(4), new Gray8(5), new Gray8(6), new Gray8(7),
            };
            var expected = new[] {
                "00000000", "00000001", "00000011", "00000010",
                "00000110", "00000111", "00000101", "00000100",
            };

            CollectionAssert.AreEqual(expected, actual.ConvertAll(g => g.ToString()));
        }
    }
}