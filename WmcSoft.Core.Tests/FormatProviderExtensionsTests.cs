using System;
using System.Globalization;
using Xunit;

namespace WmcSoft
{
    public class FormatProviderExtensionsTests
    {
        [Fact]
        public void CanGetRegionInfo()
        {
            var fr = new CultureInfo("fr-FR");
            var region = fr.GetFormat<RegionInfo>();
            Assert.Equal("EUR", region.ISOCurrencySymbol);
        }
    }
}