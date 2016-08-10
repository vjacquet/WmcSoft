using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Drawing
{
    [TestClass]
    public class ImageExtensionsTests
    {
        [TestMethod]
        public void AssertBitmapDefaults() {
            using (var b = new Bitmap(100, 100)) {
                Assert.AreEqual(96F, b.HorizontalResolution);
                Assert.AreEqual(96F, b.VerticalResolution);
                Assert.AreEqual(PixelFormat.Format32bppArgb, b.PixelFormat);
            }
        }

        [TestMethod]
        public void CanConvertImageToBitmap() {
        }
    }
}
