using System;
using System.Drawing;
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
        public void SupportSetLineDash() {
            using (var ctx = CreateCanvas("setLineDash.png")) {
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
        public void SupportArc() {
            using (var ctx = CreateCanvas("arc.png")) {
                ctx.BeginPath();
                ctx.Arc(75, 75, 50, 0, 2 * Math.PI);
                ctx.Stroke();
            }
        }

        [Fact]
        public void CanDemonstrateArc() {
            using (var ctx = CreateCanvas("arc-demo.png")) {
                // Draw shapes
                for (int i = 0; i < 4; i++) {
                    for (int j = 0; j < 3; j++) {
                        ctx.BeginPath();
                        var x = 25 + j * 50;                           // x coordinate
                        var y = 25 + i * 50;                           // y coordinate
                        var radius = 20;                               // Arc radius
                        var startAngle = 0;                            // Starting point on circle
                        var endAngle = Math.PI + (Math.PI * j) / 2;    // End point on circle
                        var anticlockwise = i % 2 == 1;                // Draw anticlockwise

                        ctx.Arc(x, y, radius, startAngle, endAngle, anticlockwise);

                        if (i > 1) {
                            ctx.Fill();
                        } else {
                            ctx.Stroke();
                        }
                    }
                }
            }
        }

        [Fact]
        public void SupportEllipse() {
            using (var ctx = CreateCanvas("ellipse.png")) {
                ctx.BeginPath();
                ctx.Ellipse(100, 100, 50, 75, 45 * Math.PI / 180, 0, 2 * Math.PI);
                ctx.Stroke();
            }
        }

        [Fact]
        public void CanFillNonZero() {
            using (var ctx = CreateCanvas("fillNonZero.png")) {
                ctx.BeginPath();
                ctx.Arc(50, 50, 30, 0, Math.PI * 2, true);
                ctx.Arc(75, 50, 15, 0, Math.PI * 2, true);
                ctx.Fill(CanvasFillRule.NonZero);
            }
        }

        [Fact]
        public void CanFillEvenOdd() {
            using (var ctx = CreateCanvas("fillEvenOdd.png")) {
                ctx.BeginPath();
                ctx.Arc(50, 50, 30, 0, Math.PI * 2, true);
                ctx.Arc(75, 50, 15, 0, Math.PI * 2, true);
                ctx.Fill(CanvasFillRule.EvenOdd);
            }
        }
    }
}