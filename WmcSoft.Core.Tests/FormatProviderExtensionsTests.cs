using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class FormatProviderExtensionsTests
    {
        [TestMethod]
        public void CanGetRegionInfo() {
            var fr = new CultureInfo("fr-FR");
            var region = fr.GetFormat<RegionInfo>();
            Assert.AreEqual("EUR", region.ISOCurrencySymbol);
        }
    }
}
