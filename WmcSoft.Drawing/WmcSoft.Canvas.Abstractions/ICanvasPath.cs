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

namespace WmcSoft.Canvas
{
    /// <summary>
    /// Shared path API methods
    /// </summary>
    public interface ICanvasPath<T>
    {
        void ClosePath();
        void MoveTo(T x, T y);
        void LineTo(T x, T y);
        void QuadraticCurveTo(T cpx, T cpy, T x, T y);
        void BezierCurveTo(T cp1x, T cp1y, T cp2x, T cp2y, T x, T y);
        void ArcTo(T x1, T y1, T x2, T y2, T radius);
        void ArcTo(T x1, T y1, T x2, T y2, T radiusX, T radiusY, T rotation);
        void Rect(T x, T y, T w, T h);
        void Arc(T x, T y, T radius, T startAngle, T endAngle, bool anticlockwise = false);
        void Ellipse(T x, T y, T radiusX, T radiusY, T rotation, T startAngle, T endAngle, bool anticlockwise = false);
    }
}