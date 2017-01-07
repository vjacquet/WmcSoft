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
    public class CanvasRectTests : CanvasTestsBase
    {
        [Fact]
        public void SupportFillRect() {
            using (var ctx = CreateCanvas("fillRect.png")) {
                ctx.FillStyle = Color.Green;
                ctx.FillRect(10, 10, 100, 100);
            }
        }

        [Fact]
        public void SupportStrokeRect() {
            using (var ctx = CreateCanvas("strokeRect.png")) {
                ctx.StrokeStyle = Color.Green;
                ctx.StrokeRect(10, 10, 100, 100);
            }
        }

        [Fact]
        public void SupportClearRect() {
            using (var ctx = CreateCanvas("clearRect.png")) {
                ctx.BeginPath();
                ctx.MoveTo(20, 20);
                ctx.LineTo(200, 20);
                ctx.LineTo(120, 120);
                ctx.ClosePath(); // draws last line of the triangle
                ctx.Stroke();

                ctx.ClearRect(10, 10, 100, 100);
            }
        }
    }
}
