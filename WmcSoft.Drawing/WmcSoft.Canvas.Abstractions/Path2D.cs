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
using System.Numerics;

namespace WmcSoft.Canvas
{
    public abstract class Path2D<T> : ICanvasPath<T>
    {
        public Path2D(Path2D<T> path) {
        }

        public Path2D(IEnumerable<Path2D<T>> paths, CanvasFillRule fillRule = CanvasFillRule.NonZero) {
        }

        public Path2D(string d) {
        }

        public abstract void AddPath(Path2D<T> path);
        public abstract void AddPath(Path2D<T> path, Matrix4x4 transform);
        public abstract void ClosePath();
        public abstract void MoveTo(T x, T y);
        public abstract void LineTo(T x, T y);
        public abstract void QuadraticCurveTo(T cpx, T cpy, T x, T y);
        public abstract void BezierCurveTo(T cp1x, T cp1y, T cp2x, T cp2y, T x, T y);
        public virtual void ArcTo(T x1, T y1, T x2, T y2, T radius) {
            ArcTo(x1, y1, x2, y2, radius, radius, default(T));
        }
        public abstract void ArcTo(T x1, T y1, T x2, T y2, T radiusX, T radiusY, T rotation);
        public abstract void Rect(T x, T y, T w, T h);
        public virtual void Arc(T x, T y, T radius, T startAngle, T endAngle, bool anticlockwise) {
            Ellipse(x, y, radius, radius, default(T), startAngle, endAngle, anticlockwise);
        }
        public abstract void Ellipse(T x, T y, T radiusX, T radiusY, T rotation, T startAngle, T endAngle, bool anticlockwise);
    }
}
