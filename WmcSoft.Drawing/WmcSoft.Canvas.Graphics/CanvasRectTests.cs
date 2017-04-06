using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
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

        [Fact]
        public void SupportOverlappingRgbaRect() {
            using (var ctx = CreateCanvas("overlapping.png")) {
                ctx.FillStyle = Color.FromArgb(200, 0, 0);
                ctx.FillRect(10, 10, 50, 50);

                ctx.FillStyle = Color.FromArgb((int)(0.5d * 255), 0, 0, 200);
                ctx.FillRect(30, 30, 50, 50);
            }
        }

        [Fact]
        public void SupportRgba() {
            using (var ctx = CreateCanvas("rgba.png")) {
                ctx.FillStyle = Color.FromArgb(255, 221, 0);
                ctx.FillRect(0, 0, 150, 37.5f);
                ctx.FillStyle = Color.FromArgb(102, 204, 0);
                ctx.FillRect(0, 37.5f, 150, 37.5f);
                ctx.FillStyle = Color.FromArgb(0, 153, 255);
                ctx.FillRect(0, 75, 150, 37.5f);
                ctx.FillStyle = Color.FromArgb(255, 51, 0);
                ctx.FillRect(0, 112.5f, 150, 37.5f);

                // Draw semi transparent rectangles
                for (var i = 0; i < 10; i++) {
                    ctx.FillStyle = Color.FromArgb((int)((i + 1) * 25.5f), 255, 255, 255);
                    for (var j = 0; j < 4; j++) {
                        ctx.FillRect(5 + i * 14, 5 + j * 37.5f, 14, 27.5f);
                    }
                }
            }
        }

        [Fact]
        public void SupportLinearGradient() {
            using (var ctx = CreateCanvas("linearGradient.png")) {
                var gradient = ctx.CreateLinearGradient(0, 0, 200, 0);
                gradient.AddColorStop(0, Color.Green);
                gradient.AddColorStop(1, Color.Yellow);
                ctx.FillStyle = gradient;
                ctx.FillRect(10, 10, 200, 100);
            }
        }

        [Fact]
        public void SupportComplexLinearGradient() {
            using (var ctx = CreateCanvas("linearGradient2.png")) {
                var gradient = ctx.CreateLinearGradient(0, 0, 200, 0);
                gradient.AddColorStop(0.25f, Color.Green);
                gradient.AddColorStop(0.5f, Color.Red);
                gradient.AddColorStop(0.5f, Color.Blue);
                gradient.AddColorStop(0.75f, Color.White);
                ctx.FillStyle = gradient;
                ctx.FillRect(10, 10, 200, 100);
            }
        }

        [Fact(Skip = "Not implemented yet")]
        public void SupportRadialGradient() {
            using (var ctx = CreateCanvas("radialGradient.png")) {
                var gradient = ctx.CreateRadialGradient(100, 100, 100, 100, 100, 0);
                gradient.AddColorStop(0, Color.Green);
                gradient.AddColorStop(1, Color.White);
                ctx.FillStyle = gradient;
                ctx.FillRect(0, 0, 200, 200);
            }
        }
    }
}
