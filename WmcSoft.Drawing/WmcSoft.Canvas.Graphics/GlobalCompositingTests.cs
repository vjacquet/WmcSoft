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
    public class GlobalCompositingTests : CanvasTestsBase
    {
        [Fact]
        public void SupportGlobalAlpha() {
            using (var ctx = CreateCanvas("globalAlpha.png")) {
                // draw background
                ctx.FillStyle = Color.FromArgb(0xFF, 0xDD, 0x00);
                ctx.FillRect(0, 0, 75, 75);
                ctx.FillStyle = Color.FromArgb(0x66, 0xcc, 0x00);
                ctx.FillRect(75, 0, 75, 75);
                ctx.FillStyle = Color.FromArgb(0x00, 0x99, 0xFF);
                ctx.FillRect(0, 75, 75, 75);
                ctx.FillStyle = Color.FromArgb(0xFF, 0x33, 0x00);
                ctx.FillRect(75, 75, 75, 75);

                ctx.FillStyle = Color.FromArgb(0xFF, 0xFF, 0xFF);

                // set transparency value
                ctx.GlobalAlpha = 0.2f;

                // Draw semi transparent circles
                for (int i = 0; i < 7; i++) {
                    ctx.BeginPath();
                    ctx.Arc(75, 75, 10 + 10 * i, 0, Math.PI * 2, true);
                    ctx.Fill();
                }
            }
        }
    }
}
