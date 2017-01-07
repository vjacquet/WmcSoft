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
    public class CanvasPathTests : CanvasTestsBase
    {
        [Fact]
        public void SupportBeginPath() {
            using (var ctx = CreateCanvas("beginPath.png")) {
                // First path
                ctx.BeginPath();
                ctx.StrokeStyle = Color.Blue;
                ctx.MoveTo(20, 20);
                ctx.LineTo(200, 20);
                ctx.Stroke();

                // Second path
                ctx.BeginPath();
                ctx.StrokeStyle = Color.Green;
                ctx.MoveTo(20, 20);
                ctx.LineTo(120, 120);
                ctx.Stroke();
            }
        }

        [Fact]
        public void SupportLineCap() {
            using (var ctx = CreateCanvas("lineCap.png")) {
                //var p = new System.Drawing.Drawing2D.GraphicsPath();
                //p.StartFigure();
                //p.AddLine(10, 10, 200, 10);
                //p.AddLine(10, 200, 200, 200);
                //p.CloseFigure();
                //ctx.g.DrawPath(Pens.Red, p);
                //return;
                var lineCap = new[] { CanvasLineCap.Butt, CanvasLineCap.Round, CanvasLineCap.Square };

                // Draw guides
                ctx.StrokeStyle = Color.FromArgb(0, 0x99, 0xff);
                ctx.BeginPath();
                ctx.MoveTo(10, 10);
                ctx.LineTo(140, 10);
                ctx.MoveTo(10, 140);
                ctx.LineTo(140, 140);
                ctx.Stroke();

                // Draw lines
                ctx.StrokeStyle = Color.Black;
                for (var i = 0; i < lineCap.Length; i++) {
                    ctx.LineWidth = 15;
                    ctx.LineCap = lineCap[i];
                    ctx.BeginPath();
                    ctx.MoveTo(25 + i * 50, 10);
                    ctx.LineTo(25 + i * 50, 140);
                    ctx.Stroke();
                }
            }
        }
    }
}