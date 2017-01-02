using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Drawing
{
    public abstract class Path2D : ICanvasPath
    {
        public Path2D(Path2D path) {
        }

        public Path2D(IEnumerable<Path2D> paths, CanvasFillRule fillRule = CanvasFillRule.NonZero) {
        }

        public Path2D(string d) {
        }

        public abstract void AddPath(Path2D path);//void addPath(Path2D path, optional DOMMatrixInit transform);
        public abstract void ClosePath();
        public abstract void MoveTo(double x, double y);
        public abstract void LineTo(double x, double y);
        public abstract void QuadraticCurveTo(double cpx, double cpy, double x, double y);
        public abstract void BezierCurveTo(double cp1x, double cp1y, double cp2x, double cp2y, double x, double y);
        public abstract void ArcTo(double x1, double y1, double x2, double y2, double radius);
        public abstract void ArcTo(double x1, double y1, double x2, double y2, double radiusX, double radiusY, double rotation);
        public abstract void Rect(double x, double y, double w, double h);
        public abstract void Arc(double x, double y, double radius, double startAngle, double endAngle, bool anticlockwise = false);
        public abstract void Ellipse(double x, double y, double radiusX, double radiusY, double rotation, double startAngle, double endAngle, bool anticlockwise = false);
    }
}
