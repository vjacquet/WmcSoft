﻿#region Licence

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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageSharp;

namespace WmcSoft.Canvas
{
    public class ImageSharpCanvas : ICanvasRect, ICanvasFillStrokeStyles<Color>, IDisposable
    {
        private readonly Image _image;

        public ImageSharpCanvas(int width, int height) {
            _image = new Image(width, height);

            FillStyle = Color.Black;
            StrokeStyle = Color.Black;
        }

        public Variant<Color, CanvasGradient, CanvasPattern> FillStyle { get; set; }
        public Variant<Color, CanvasGradient, CanvasPattern> StrokeStyle { get; set; }

        public void ClearRect(double x, double y, double w, double h) {
            throw new NotImplementedException();
        }

        public CanvasGradient CreateLinearGradient(double x0, double y0, double x1, double y1) {
            throw new NotImplementedException();
        }

        public CanvasPattern CreatePattern(ICanvasImageSource image, Repetition repetition = Repetition.NoRepeat) {
            throw new NotImplementedException();
        }

        public CanvasGradient CreateRadialGradient(double x0, double y0, double r0, double x1, double y1, double r1) {
            throw new NotImplementedException();
        }

        public void FillRect(double x, double y, double w, double h) {
            throw new NotImplementedException();
        }

        public void StrokeRect(double x, double y, double w, double h) {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose() {
            throw new NotImplementedException();
        }
    }
}
