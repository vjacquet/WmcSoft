using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WmcSoft.Canvas
{
    public class CanvasRectTests
    {
        static ImageSharpCanvas CreateCanvas() {
            return new ImageSharpCanvas(400, 200);
        }

        [Fact]
        public void CanFillRect() {
            using(var canvas = CreateCanvas()) {
                canvas.FillStyle = "green";
                canvas.FillRect(10, 10, 100, 100);
            }
        }

    }
}
