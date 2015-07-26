using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Business.ProductModel
{
    [TestClass]
    public class FurnitureTests
    {
        [TestMethod]
        public void CanCreateProductType() {
            var karlanda = new Karlanda();

            Assert.IsNotNull(karlanda.Name);
            Assert.IsNotNull(karlanda.Description);
            Assert.AreEqual(2, karlanda.MandatoryFeatures.Count);
            Assert.AreEqual(0, karlanda.OptionalFeatures.Count);
        }
    }
}
