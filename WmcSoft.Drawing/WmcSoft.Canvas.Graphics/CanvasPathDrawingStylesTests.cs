using System;
using System.Drawing;
using Xunit;

namespace WmcSoft.Canvas
{
    public class CanvasPathDrawingStylesTests : CanvasTestsBase
    {
        [Fact]
        public void SupportLineCap() {
            using (var ctx = CreateCanvas("lineCap.png", 150, 150)) {
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
            using (var ctx = CreateCanvas("lineWidth.png", 150, 150)) {
                for (var i = 0; i < 10; i++) {
                    ctx.LineWidth = 1 + i;
                    ctx.BeginPath();
                    ctx.MoveTo(5 + i * 14, 5);
                    ctx.LineTo(5 + i * 14, 140);
                    ctx.Stroke();
                }
            }
        }

        [Fact]
        public void SupportLineJoin() {
            var lineJoin = new[] { CanvasLineJoin.Round, CanvasLineJoin.Bevel, CanvasLineJoin.Miter };
            using (var ctx = CreateCanvas("lineJoin.png")) {
                ctx.LineWidth = 10;
                for (var i = 0; i < lineJoin.Length; i++) {
                    ctx.LineJoin = lineJoin[i];
                    ctx.BeginPath();
                    ctx.MoveTo(-5, 5 + i * 40);
                    ctx.LineTo(35, 45 + i * 40);
                    ctx.LineTo(75, 5 + i * 40);
                    ctx.LineTo(115, 45 + i * 40);
                    ctx.LineTo(155, 5 + i * 40);
                    ctx.Stroke();
                }
            }
        }

        [Fact]
        public void SupportMiterLimit() {
            using (var ctx = CreateCanvas("miterLimit.png", 150, 150)) {
                // Clear canvas
                ctx.ClearRect(0, 0, 150, 150);

                // Draw guides
                ctx.StrokeStyle = Color.FromArgb(0x00, 0x99, 0xff);
                ctx.LineWidth = 2;
                ctx.StrokeRect(-5, 50, 160, 50);

                ctx.MiterLimit = 10f;

                // Set line styles
                ctx.StrokeStyle = Color.Black;
                ctx.LineWidth = 10;

                // Draw lines
                ctx.BeginPath();
                ctx.MoveTo(0, 100);
                for (var i = 0; i < 24; i++) {
                    var dy = i % 2 == 0 ? 25 : -25;
                    ctx.LineTo((float)Math.Pow(i, 1.5) * 2, 75 + dy);
                }
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

        [Fact]
        public void CanDemonstrateFillStyle() {
            using (var ctx = CreateCanvas("fillStyle.png", 150, 150)) {
                for (var i = 0; i < 6; i++) {
                    for (var j = 0; j < 6; j++) {
                        ctx.FillStyle = Color.FromArgb((int)Math.Floor(255 - 42.5d * i), (int)Math.Floor(255 - 42.5d * j), 0);
                        ctx.FillRect(j * 25, i * 25, 25, 25);
                    }
                }
            }
        }

        [Fact]
        public void CanDemonstrateStrokeStyle() {
            using (var ctx = CreateCanvas("strokeStyle.png", 150, 150)) {
                for (var i = 0; i < 6; i++) {
                    for (var j = 0; j < 6; j++) {
                        ctx.StrokeStyle = Color.FromArgb(0, (int)Math.Floor(255 - 42.5 * i), (int)Math.Floor(255 - 42.5 * j));
                        ctx.BeginPath();
                        ctx.Arc(12.5f + j * 25, 12.5f + i * 25, 10, 0, Math.PI * 2, true);
                        ctx.Stroke();
                    }
                }
            }
        }
    }
}