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

namespace WmcSoft.Drawing
{
    /// <summary>
    /// Shared path API methods
    /// </summary>
    public interface ICanvasPath
    {
        void ClosePath();
        void MoveTo(double x, double y);
        void LineTo(double x, double y);
        void QuadraticCurveTo(double cpx, double cpy, double x, double y);
        void BezierCurveTo(double cp1x, double cp1y, double cp2x, double cp2y, double x, double y);
        void ArcTo(double x1, double y1, double x2, double y2, double radius);
        void ArcTo(double x1, double y1, double x2, double y2, double radiusX, double radiusY, double rotation);
        void Rect(double x, double y, double w, double h);
        void Arc(double x, double y, double radius, double startAngle, double endAngle, bool anticlockwise = false);
        void Ellipse(double x, double y, double radiusX, double radiusY, double rotation, double startAngle, double endAngle, bool anticlockwise = false);
    }
}