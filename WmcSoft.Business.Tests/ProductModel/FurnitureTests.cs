using Xunit;

namespace WmcSoft.Business.ProductModel
{
    public class FurnitureTests
    {
        [Fact]
        public void CanCreateProductType()
        {
            var karlanda = new Karlanda();

            Assert.NotNull(karlanda.Name);
            Assert.NotNull(karlanda.Description);
            Assert.Equal(2, karlanda.MandatoryFeatures.Count);
            Assert.Equal(0, karlanda.OptionalFeatures.Count);
        }
    }
}