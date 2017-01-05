using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WmcSoft.Canvas
{
    public class CanvasRectTests
    {
        static GraphicsCanvas CreateCanvas(string filename) {
            var image = new Bitmap(400, 200);
            return new GraphicsCanvas(Graphics.FromImage(image), g => image.Save(filename, ImageFormat.Png));
        }

        [Fact]
        public void CanFillRect() {
            using(var canvas = CreateCanvas("fillRect.png")) {
                canvas.FillStyle = Color.Green;
                canvas.FillRect(10, 10, 100, 100);
            }
        }

    }
}
