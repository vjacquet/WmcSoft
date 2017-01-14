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
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Canvas
{
    public class GraphicsCanvas : ICanvasRect<float>
        , ICanvasDrawPath<float>
        , ICanvasPathDrawingStyles<float>
        , ICanvasFillStrokeStyles<float, Color>
        , ICanvasImageSmoothing
        , IDisposable
    {
        #region Variant visitors

        sealed class PenVisitor : Variant<Color, CanvasGradient<float, Color>, CanvasPattern>.Visitor
            , IDisposable
        {
            private Func<Pen> _factory;
            private Pen _tool;

            public PenVisitor() {
                width = 1f;
                lineCap = LineCap.Flat;
                lineJoin = LineJoin.MiterClipped;
                miterLimit = 10f;
            }

            public void Dispose() {
                if (_tool != null) {
                    _tool.Dispose();
                    _tool = null;
                }
            }

            public override void Visit(CanvasGradient<float, Color> instance) {
                throw new NotSupportedException();
            }

            public override void Visit(CanvasPattern instance) {
                throw new NotSupportedException();
            }

            Pen CreateSolidPen(Color color) {
                var pen = new Pen(color, Width);
                pen.LineJoin = lineJoin;
                pen.MiterLimit = MiterLimit;
                pen.StartCap = LineCap;
                pen.EndCap = LineCap;
                if (dashPattern != null)
                    pen.DashPattern = dashPattern;
                pen.DashOffset = dashOffset;
                return pen;
            }

            public override void Visit(Color instance) {
                _factory = () => CreateSolidPen(instance);

                Dispose();
            }

            public Pen Pen {
                get {
                    if (_tool == null)
                        _tool = _factory();
                    return _tool;
                }
            }

            public static implicit operator Pen(PenVisitor visitor) {
                return visitor.Pen;
            }

            public float Width {
                get { return width; }
                set { Guard.Positive(value); if (width != value) { width = value; Dispose(); } }
            }
            float width;

            public LineJoin LineJoin {
                get { return lineJoin; }
                set { if (lineJoin != value) { lineJoin = value; Dispose(); } }
            }
            LineJoin lineJoin;

            public float MiterLimit {
                get { return miterLimit; }
                set { if (miterLimit != value) { miterLimit = value; Dispose(); } }
            }
            float miterLimit;

            public float DashOffset {
                get { return dashOffset; }
                set { if (dashOffset != value) { dashOffset = value; Dispose(); } }
            }
            float dashOffset;

            public float[] DashPattern {
                get { return dashPattern ?? Array.Empty<float>(); }
                set { if (!DashPatternEquals(value)) { SetDashPattern(value); Dispose(); } }
            }

            float[] dashPattern;

            public LineCap LineCap {
                get { return lineCap; }
                set { if (lineCap != value) { lineCap = value; Dispose(); } }
            }
            LineCap lineCap;

            #region Helpers

            private void SetDashPattern(float[] value) {
                if (value == null || value.Length == 0) {
                    dashPattern = null;
                } else if (value.Length % 1 == 1) {
                    dashPattern = new float[value.Length * 2];
                    Array.Copy(value, dashPattern, value.Length);
                    Array.Copy(value, 0, dashPattern, value.Length, value.Length);
                } else {
                    dashPattern = (float[])value.Clone();
                }
            }

            private bool DashPatternEquals(float[] value) {
                if (dashPattern == null) {
                    return (value == null || value.Length == 0);
                }
                if (value == null) {
                    return dashPattern == null;
                }
                if (value.Length % 1 == 1) {
                    if (dashPattern.Length != value.Length * 2)
                        return false;
                    for (int i = 0; i < value.Length; i++) {
                        if (value[i] != dashPattern[i] || value[i] != dashPattern[i + value.Length])
                            return false;
                    }
                    return true;
                } else {
                    if (dashPattern.Length != value.Length)
                        return false;
                    for (int i = 0; i < value.Length; i++) {
                        if (value[i] != dashPattern[i])
                            return false;
                    }
                    return true;
                }
            }

            #endregion
        }

        sealed class BrushVisitor : Variant<Color, CanvasGradient<float, Color>, CanvasPattern>.Visitor
              , IDisposable
        {
            private Brush _tool;

            public void Dispose() {
                if (_tool != null) {
                    _tool.Dispose();
                    _tool = null;
                }
            }

            public override void Visit(CanvasGradient<float, Color> instance) {
                throw new NotSupportedException();
            }

            public override void Visit(CanvasPattern instance) {
                throw new NotSupportedException();
            }

            public override void Visit(Color instance) {
                var brush = new SolidBrush(instance);
                Dispose();
                _tool = brush;
            }

            public Brush Brush { get { return _tool; } }

            public static implicit operator Brush(BrushVisitor visitor) {
                return visitor.Brush;
            }
        }

        #endregion

        #region Constants

        static readonly float PI = (float)Math.PI;
        static readonly float TWO_PI = (float)(2d * Math.PI);

        #endregion

        private Graphics g;
        private Action<Graphics> _disposer;
        private PenVisitor _pen;
        private BrushVisitor _brush;
        private GraphicsPath _path;
        private float[] _lineDash;
        private float _x;
        private float _y;
        private PointF _start;
        private ImageSmoothingQuality _imageSmoothingQuality;

        public GraphicsCanvas(Graphics graphics, Action<Graphics> disposer = null) {
            g = graphics;
            ImageSmoothingQuality = ImageSmoothingQuality.Low;
            g.SmoothingMode = SmoothingMode.HighQuality;
            if (disposer == null)
                _disposer = _ => _.Dispose();
            else
                _disposer = disposer;

            _pen = new PenVisitor();
            _brush = new BrushVisitor();

            FillStyle = Color.Black;
            StrokeStyle = Color.Black;

            _lineDash = Array.Empty<float>();
        }

        void IDisposable.Dispose() {
            if (_disposer != null) {
                _disposer(g);
                _disposer = null;
                g = null;
            }
            _pen.Dispose();
            _brush.Dispose();
            if (_path != null) {
                _path.Dispose();
                _path = null;
            }
        }

        #region ICanvasFillStrokeStyles<float, Color> methods

        public Variant<Color, CanvasGradient<float, Color>, CanvasPattern> FillStyle {
            get {
                return _fillStyle;
            }
            set {
                _fillStyle = value;
                _fillStyle.Visit(_brush);
            }
        }
        Variant<Color, CanvasGradient<float, Color>, CanvasPattern> _fillStyle;

        public Variant<Color, CanvasGradient<float, Color>, CanvasPattern> StrokeStyle {
            get {
                return _strokeStyle;
            }
            set {
                _strokeStyle = value;
                _strokeStyle.Visit(_pen);
            }
        }
        Variant<Color, CanvasGradient<float, Color>, CanvasPattern> _strokeStyle;

        public CanvasGradient<float, Color> CreateLinearGradient(float x0, float y0, float x1, float y1) {
            throw new NotImplementedException();
        }

        public CanvasPattern CreatePattern(ICanvasImageSource image, Repetition repetition = Repetition.NoRepeat) {
            throw new NotImplementedException();
        }

        public CanvasGradient<float, Color> CreateRadialGradient(float x0, float y0, float r0, float x1, float y1, float r1) {
            throw new NotImplementedException();
        }

        #endregion

        #region ICanvasRect<float>

        public void ClearRect(float x, float y, float w, float h) {
            using (GraphicsPath path = new GraphicsPath()) {
                path.AddRectangle(new RectangleF(x, y, w, h));
                g.SetClip(path);
                g.Clear(Color.Transparent);
                g.ResetClip();
            }
        }

        public void FillRect(float x, float y, float w, float h) {
            g.FillRectangle(_brush, x, y, w, h);
        }

        public void StrokeRect(float x, float y, float w, float h) {
            g.DrawRectangle(_pen, x, y, w, h);
        }

        #endregion

        #region ICanvasPath<float>

        private GraphicsPath CurrentPath {
            get { return _path; }
        }

        public void ClosePath() {
            CurrentPath.CloseFigure();
            _x = _start.X;
            _y = _start.Y;
        }

        public void MoveTo(float x, float y) {
            _path.StartFigure();
            _start = new PointF(x, y);
            _x = x;
            _y = y;
        }

        public void LineTo(float x, float y) {
            CurrentPath.AddLine(_x, _y, x, y);
            _x = x;
            _y = y;
        }

        public void QuadraticCurveTo(float cpx, float cpy, float x, float y) {
            throw new NotImplementedException();
        }

        public void BezierCurveTo(float cp1x, float cp1y, float cp2x, float cp2y, float x, float y) {
            throw new NotImplementedException();
        }

        public void ArcTo(float x1, float y1, float x2, float y2, float radius) {
            throw new NotImplementedException();
        }

        public void ArcTo(float x1, float y1, float x2, float y2, float radiusX, float radiusY, float rotation) {
            throw new NotImplementedException();
        }

        public void Rect(float x, float y, float w, float h) {
            throw new NotImplementedException();
        }

        public void Arc(float x, float y, float radius, float startAngle, float endAngle, bool anticlockwise) {
            var sweepAngle = endAngle - startAngle;
            if (!anticlockwise)
                _path.AddArc(x - radius, y - radius, 2 * radius, 2 * radius, Deg(startAngle), Swep(sweepAngle));
            else
                _path.AddArc(x - radius, y - radius, 2 * radius, 2 * radius, Deg(endAngle), Swep(TWO_PI - sweepAngle));
        }

        static float Deg(float value) {
            value = 180f * value / PI;
            while (value < 0f)
                value += 360f;
            if (value > 360f)
                value = 360f;
            return value;
        }

        static float Swep(float value) {
            value = 180f * value / PI;
            while (value <= 0f) // sweep cannot be 0
                value += 360f;
            if (value > 360f)
                value = 360f;
            return value;
        }

        public void Ellipse(float x, float y, float radiusX, float radiusY, float rotation, float startAngle, float endAngle, bool anticlockwise) {
            var sweepAngle = endAngle - startAngle;
            if (rotation == 0f) {
                if (!anticlockwise)
                    _path.AddArc(x - radiusX, y - radiusY, 2 * radiusX, 2 * radiusY, Deg(startAngle), Swep(sweepAngle));
                else
                    _path.AddArc(x - radiusX, y - radiusY, 2 * radiusX, 2 * radiusY, Deg(endAngle), Swep(TWO_PI - sweepAngle));
            } else {
                var matrix = new Matrix();
                matrix.RotateAt(Deg(rotation), new PointF(x, y));
                var path = new GraphicsPath();
                if (!anticlockwise)
                    path.AddArc(x - radiusX, y - radiusY, 2 * radiusX, 2 * radiusY, Deg(startAngle), Swep(sweepAngle));
                else
                    path.AddArc(x - radiusX, y - radiusY, 2 * radiusX, 2 * radiusY, Deg(endAngle), Swep(TWO_PI - sweepAngle));
                path.Transform(matrix);
                _path.AddPath(path, false);
            }
        }

        public void BeginPath() {
            if (_path != null)
                _path.Dispose();
            _path = new GraphicsPath(FillMode.Alternate);
            _path.StartFigure();
        }

        FillMode Map(CanvasFillRule value) {
            switch (value) {
            case CanvasFillRule.NonZero:
                return FillMode.Winding;
            case CanvasFillRule.EvenOdd:
                return FillMode.Alternate;
            default:
                throw new InvalidOperationException();
            }
        }

        public void Fill(CanvasFillRule fillRule) {
            CurrentPath.FillMode = Map(fillRule);
            g.FillPath(_brush, CurrentPath);
        }

        public void Fill(Path2D<float> path, CanvasFillRule fillRule) {
            throw new NotImplementedException();
        }

        public void Stroke() {
            g.DrawPath(_pen, CurrentPath);
        }

        public void Stroke(Path2D<float> path) {
            //g.DrawPath(_pen, CurrentPath);
        }

        public void Clip(CanvasFillRule fillRule) {
            throw new NotImplementedException();
        }

        public void Clip(Path2D<float> path, CanvasFillRule fillRule) {
            throw new NotImplementedException();
        }

        public void ResetClip() {
            throw new NotImplementedException();
        }

        public bool IsPointInPath(float x, float y, CanvasFillRule fillRule) {
            throw new NotImplementedException();
        }

        public bool IsPointInPath(Path2D<float> path, float x, float y, CanvasFillRule fillRule) {
            throw new NotImplementedException();
        }

        public bool IsPointInStroke(float x, float y) {
            throw new NotImplementedException();
        }

        public bool IsPointInStroke(Path2D<float> path, float x, float y) {
            throw new NotImplementedException();
        }

        #endregion

        #region ICanvasPathDrawingStyles<T> members

        public float LineWidth {
            get { return _pen.Width; }
            set { Guard.Positive(value); _pen.Width = value; }
        }

        public CanvasLineCap LineCap {
            get {
                switch (_pen.LineCap) {
                case System.Drawing.Drawing2D.LineCap.Flat:
                    return CanvasLineCap.Butt;
                case System.Drawing.Drawing2D.LineCap.Square:
                    return CanvasLineCap.Square;
                case System.Drawing.Drawing2D.LineCap.Round:
                    return CanvasLineCap.Round;
                case System.Drawing.Drawing2D.LineCap.Triangle:
                case System.Drawing.Drawing2D.LineCap.NoAnchor:
                case System.Drawing.Drawing2D.LineCap.SquareAnchor:
                case System.Drawing.Drawing2D.LineCap.RoundAnchor:
                case System.Drawing.Drawing2D.LineCap.DiamondAnchor:
                case System.Drawing.Drawing2D.LineCap.ArrowAnchor:
                case System.Drawing.Drawing2D.LineCap.Custom:
                case System.Drawing.Drawing2D.LineCap.AnchorMask:
                default:
                    throw new InvalidCastException();
                }
            }
            set {
                switch (value) {
                case CanvasLineCap.Butt:
                    _pen.LineCap = System.Drawing.Drawing2D.LineCap.Flat;
                    break;
                case CanvasLineCap.Round:
                    _pen.LineCap = System.Drawing.Drawing2D.LineCap.Round;
                    break;
                case CanvasLineCap.Square:
                    _pen.LineCap = System.Drawing.Drawing2D.LineCap.Square;
                    break;
                default:
                    throw new ArgumentException();
                }
            }
        }

        public CanvasLineJoin LineJoin {
            get {
                switch (_pen.LineJoin) {
                case System.Drawing.Drawing2D.LineJoin.MiterClipped:
                    return CanvasLineJoin.Miter;
                case System.Drawing.Drawing2D.LineJoin.Bevel:
                    return CanvasLineJoin.Bevel;
                case System.Drawing.Drawing2D.LineJoin.Round:
                    return CanvasLineJoin.Round;
                case System.Drawing.Drawing2D.LineJoin.Miter:
                default:
                    throw new InvalidCastException();
                }
            }
            set {
                switch (value) {
                case CanvasLineJoin.Round:
                    _pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    break;
                case CanvasLineJoin.Bevel:
                    _pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                    break;
                case CanvasLineJoin.Miter:
                    _pen.LineJoin = System.Drawing.Drawing2D.LineJoin.MiterClipped;
                    break;
                default:
                    break;
                }
            }
        }

        public float MiterLimit {
            get { return _pen.MiterLimit; }
            set { Guard.Positive(value); _pen.MiterLimit = value; }
        }

        public float[] LineDash {
            get { return _pen.DashPattern; }
            set { _pen.DashPattern = value; }
        }

        public float LineDashOffset {
            get { return _pen.DashOffset; }
            set { Guard.Finite(value); _pen.DashOffset = value; }
        }

        #endregion

        #region ICanvasImageSmoothing

        public bool ImageSmoothingEnabled {
            get {
                return g.InterpolationMode != InterpolationMode.Default;
            }
            set {
                if (ImageSmoothingEnabled != value) {
                    g.InterpolationMode = (value) ? Map(_imageSmoothingQuality) : InterpolationMode.Default;
                }
            }
        }

        public ImageSmoothingQuality ImageSmoothingQuality {
            get {
                return _imageSmoothingQuality;
            }
            set {
                if (_imageSmoothingQuality != value) {
                    _imageSmoothingQuality = value;
                    if (ImageSmoothingEnabled)
                        g.InterpolationMode = Map(_imageSmoothingQuality);
                }
            }
        }

        InterpolationMode Map(ImageSmoothingQuality value) {
            switch (value) {
            case ImageSmoothingQuality.Low:
                return InterpolationMode.NearestNeighbor;
            case ImageSmoothingQuality.Medium:
                return InterpolationMode.Bicubic;
            case ImageSmoothingQuality.High:
                return InterpolationMode.HighQualityBicubic;
            default:
                return g.InterpolationMode;
            }
        }

        #endregion
    }
}