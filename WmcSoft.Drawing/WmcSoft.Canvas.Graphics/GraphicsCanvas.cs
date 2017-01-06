#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Canvas
{
    public class GraphicsCanvas : ICanvasRect<float>, ICanvasFillStrokeStyles<float, Color>, IDisposable
    {
        private Graphics g;
        private Action<Graphics> _disposer;

        public GraphicsCanvas(Graphics graphics, Action<Graphics> disposer = null) {
            g = graphics;
            if (disposer == null)
                _disposer = _ => _.Dispose();
            else
                _disposer = disposer;

            FillStyle = Color.Black;
            StrokeStyle = Color.Black;
        }

        public Variant<Color, CanvasGradient<float,Color>, CanvasPattern> FillStyle { get; set; }
        public Variant<Color, CanvasGradient<float, Color>, CanvasPattern> StrokeStyle { get; set; }

        public void ClearRect(float x, float y, float w, float h) {
            throw new NotImplementedException();
        }

        public CanvasGradient<float, Color> CreateLinearGradient(float x0, float y0, float x1, float y1) {
            throw new NotImplementedException();
        }

        public CanvasPattern CreatePattern(ICanvasImageSource image, Repetition repetition = Repetition.NoRepeat) {
            throw new NotImplementedException();
        }

        public CanvasGradient<float, Color> CreateRadialGradient(float x0, float y0, float r0, float x1, float y1, float r1) {
            throw new NotImplementedException();
        }

        public void FillRect(float x, float y, float w, float h) {
            throw new NotImplementedException();
        }

        public void StrokeRect(float x, float y, float w, float h) {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose() {
            if (_disposer != null) {
                _disposer(g);
                _disposer = null;
                g = null;
            }
        }
    }
}
