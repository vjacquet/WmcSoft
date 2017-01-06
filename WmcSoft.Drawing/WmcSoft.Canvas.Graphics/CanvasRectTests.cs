using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
            var path = Path.GetFullPath(Path.Combine(@"..\Tests", filename));
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            return new GraphicsCanvas(Graphics.FromImage(image), g => image.Save(path, ImageFormat.Png));
        }

        [Fact]
        public void CanFillRect() {
            using(var canvas = CreateCanvas("fillRect.png")) {
                canvas.FillStyle = Color.Green;
                canvas.FillRect(10, 10, 100, 100);
            }
        }

        [Fact]
        public void CanStrokeRect() {
            using(var canvas = CreateCanvas("strokeRect.png")) {
                canvas.StrokeStyle = Color.Green;
                canvas.StrokeRect(10, 10, 100, 100);
            }
        }

        [Fact]
        public void CanClearRect() {
            using(var canvas = CreateCanvas("clearRect.png")) {
                canvas.BeginPath();
                canvas.MoveTo(20, 20);
                canvas.LineTo(200, 20);
                canvas.LineTo(120, 120);
                canvas.ClosePath(); // draws last line of the triangle
                canvas.Stroke();

                canvas.ClearRect(10, 10, 100, 100);
            }
        }
    }
}
