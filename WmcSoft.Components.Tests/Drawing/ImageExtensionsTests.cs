using System.Drawing;
using System.Drawing.Imaging;
using Xunit;

namespace WmcSoft.Drawing
{
    public class ImageExtensionsTests
    {
        [Fact]
        public void AssertBitmapDefaults() {
            using (var b = new Bitmap(100, 100)) {
                Assert.Equal(96F, b.HorizontalResolution);
                Assert.Equal(96F, b.VerticalResolution);
                Assert.Equal(PixelFormat.Format32bppArgb, b.PixelFormat);
            }
        }
    }
}
