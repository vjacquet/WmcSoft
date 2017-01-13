using System;
using System.Drawing;
using Xunit;

namespace WmcSoft.Canvas
{
    public class CanvasPathDrawingStylesTests : CanvasTestsBase
    {
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

        [Fact]
        public void SupportLineWidth() {
            using (var ctx = CreateCanvas("lineWidth.png")) {
                ctx.BeginPath();
                ctx.MoveTo(0, 0);
                ctx.LineWidth = 15;
                ctx.LineTo(100, 100);
                ctx.Stroke();
            }
        }

        [Fact]
        public void SupportLineDash() {
            using (var ctx = CreateCanvas("lineDash.png")) {
                ctx.LineDash = new[] { 5f, 15f };

                ctx.BeginPath();
                ctx.MoveTo(0, 100);
                ctx.LineTo(400, 100);
                ctx.Stroke();
            }
        }

        [Fact]
        public void SupportLineDashOffset() {
            using (var ctx = CreateCanvas("lineDashOffset.png")) {
                ctx.LineDash = new[] { 4f, 16f };
                ctx.LineDashOffset = 2f;

                ctx.BeginPath();
                ctx.MoveTo(0, 100);
                ctx.LineTo(400, 100);
                ctx.Stroke();
            }
        }
    }
}