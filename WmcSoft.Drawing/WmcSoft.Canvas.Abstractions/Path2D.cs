using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public abstract void AddPath(Path2D<T> path);//void addPath(Path2D path, optional DOMMatrixInit transform);
        public abstract void ClosePath();
        public abstract void MoveTo(T x, T y);
        public abstract void LineTo(T x, T y);
        public abstract void QuadraticCurveTo(T cpx, T cpy, T x, T y);
        public abstract void BezierCurveTo(T cp1x, T cp1y, T cp2x, T cp2y, T x, T y);
        public abstract void ArcTo(T x1, T y1, T x2, T y2, T radius);
        public abstract void ArcTo(T x1, T y1, T x2, T y2, T radiusX, T radiusY, T rotation);
        public abstract void Rect(T x, T y, T w, T h);
        public abstract void Arc(T x, T y, T radius, T startAngle, T endAngle, bool anticlockwise = false);
        public abstract void Ellipse(T x, T y, T radiusX, T radiusY, T rotation, T startAngle, T endAngle, bool anticlockwise = false);
    }
}
