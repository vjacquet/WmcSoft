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
        public void SupportArc() {
            using (var ctx = CreateCanvas("arc.png")) {
                ctx.BeginPath();
                ctx.Arc(75, 75, 50, 0, 2 * Math.PI);
                ctx.Stroke();
            }
        }

        [Fact]
        public void CanDemonstrateRect() {
            using (var ctx = CreateCanvas("rect-demo.png", 150, 150)) {
                ctx.FillRect(25, 25, 100, 100);
                ctx.ClearRect(45, 45, 60, 60);
                ctx.StrokeRect(50, 50, 50, 50);
            }
        }

        [Fact]
        public void CanDemonstrateTriangle() {
            using (var ctx = CreateCanvas("triangle-demo.png", 150, 150)) {
                ctx.BeginPath();
                ctx.MoveTo(75, 50);
                ctx.LineTo(100, 75);
                ctx.LineTo(100, 25);
                ctx.Fill();
            }
        }

        [Fact]
        public void CanDemonstrateLines() {
            using (var ctx = CreateCanvas("lines-demo.png", 150, 150)) {
                // Filled triangle
                ctx.BeginPath();
                ctx.MoveTo(25, 25);
                ctx.LineTo(105, 25);
                ctx.LineTo(25, 105);
                ctx.Fill();

                // Stroked triangle
                ctx.BeginPath();
                ctx.MoveTo(125, 125);
                ctx.LineTo(125, 45);
                ctx.LineTo(45, 125);
                ctx.ClosePath();
                ctx.Stroke();
            }
        }

        [Fact]
        public void CanDemonstrateArc() {
            using (var ctx = CreateCanvas("arc-demo.png", 150, 200)) {
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
        public void CanDrawSmiley() {
            using (var ctx = CreateCanvas("smiley.png", 150, 150)) {
                ctx.BeginPath();
                ctx.Arc(75, 75, 50, 0, Math.PI * 2, true); // Outer circle
                ctx.MoveTo(110, 75);
                ctx.Arc(75, 75, 35, 0, Math.PI, false);    // Mouth (clockwise)
                ctx.MoveTo(65, 65);
                ctx.Arc(60, 65, 5, 0, Math.PI * 2, true);  // Left eye
                ctx.MoveTo(95, 65);
                ctx.Arc(90, 65, 5, 0, Math.PI * 2, true);  // Right eye
                ctx.Stroke();
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

        [Fact]
        public void SupportQuadraticCurveTo() {
            using (var ctx = CreateCanvas("quadraticCurveTo.png")) {
                ctx.BeginPath();
                ctx.MoveTo(75, 25);
                ctx.QuadraticCurveTo(25, 25, 25, 62.5f);
                ctx.QuadraticCurveTo(25, 100, 50, 100);
                ctx.QuadraticCurveTo(50, 120, 30, 125);
                ctx.QuadraticCurveTo(60, 120, 65, 100);
                ctx.QuadraticCurveTo(125, 100, 125, 62.5f);
                ctx.QuadraticCurveTo(125, 25, 75, 25);
                ctx.Stroke();
            }
        }

        [Fact]
        public void SupportBezierCurveTo() {
            using (var ctx = CreateCanvas("bezierCurveTo.png")) {
                ctx.BeginPath();
                ctx.MoveTo(75, 40);
                ctx.BezierCurveTo(75, 37, 70, 25, 50, 25);
                ctx.BezierCurveTo(20, 25, 20, 62.5f, 20, 62.5f);
                ctx.BezierCurveTo(20, 80, 40, 102, 75, 120);
                ctx.BezierCurveTo(110, 102, 130, 80, 130, 62.5f);
                ctx.BezierCurveTo(130, 62.5f, 130, 25, 100, 25);
                ctx.BezierCurveTo(85, 25, 75, 37, 75, 40);
                ctx.Fill();
            }
        }

        [Fact]
        public void SupportRectangle() {
            using (var ctx = CreateCanvas("rect.png")) {
                ctx.BeginPath(); // TODO: should not be required.
                ctx.Rect(10, 10, 100, 100);
                ctx.Fill();
            }
        }

        [Fact]
        public void SupportFillRule() {
            using (var ctx = CreateCanvas("fillRule.png")) {
                ctx.BeginPath();
                ctx.Arc(50, 50, 30, 0, Math.PI * 2, true);
                ctx.Arc(50, 50, 15, 0, Math.PI * 2, true);
                ctx.Fill(CanvasFillRule.EvenOdd);
            }
        }
    }
}