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
        , ICanvasCompositing<float>
        , IDisposable
    {
        #region Variant visitors

        sealed class PenVisitor : Variant<Color, CanvasGradient<Color>, CanvasPattern>.Visitor
            , IDisposable
        {
            private readonly GraphicsCanvas _canvas;
            private Func<Pen> _factory;
            private Pen _tool;

            public PenVisitor(GraphicsCanvas canvas)
            {
                _canvas = canvas;
            }

            public void Dispose()
            {
                if (_tool != null) {
                    _tool.Dispose();
                    _tool = null;
                }
            }

            public Pen Pen {
                get {
                    if (_tool == null)
                        _tool = _factory();
                    return _tool;
                }
            }

            public static implicit operator Pen(PenVisitor visitor)
            {
                return visitor.Pen;
            }

            public override void Visit(CanvasGradient<Color> instance)
            {
                throw new NotSupportedException();
            }

            public override void Visit(CanvasPattern instance)
            {
                throw new NotSupportedException();
            }

            Pen CreateSolidPen(Color color)
            {
                var alpha = _canvas._globalAlpha < 1f ? Color.FromArgb((int)(255 * _canvas._globalAlpha), color) : color;
                var pen = new Pen(alpha, _canvas.LineWidth);
                pen.LineJoin = _canvas._lineJoin;
                pen.MiterLimit = _canvas._miterLimit;
                pen.StartCap = _canvas._lineCap;
                pen.EndCap = _canvas._lineCap;
                var dashPattern = _canvas._dashPattern;
                if (dashPattern != null && dashPattern.Length > 0)
                    pen.DashPattern = dashPattern;
                pen.DashOffset = _canvas._dashOffset;
                return pen;
            }

            public override void Visit(Color instance)
            {
                _factory = () => CreateSolidPen(instance);
                Dispose();
            }
        }

        sealed class BrushVisitor : Variant<Color, CanvasGradient<Color>, CanvasPattern>.Visitor
            , IDisposable
        {
            private readonly GraphicsCanvas _canvas;
            private Func<Brush> _factory;
            private Brush _tool;

            public BrushVisitor(GraphicsCanvas canvas)
            {
                _canvas = canvas;
            }

            public void Dispose()
            {
                if (_tool != null) {
                    _tool.Dispose();
                    _tool = null;
                }
            }

            public Brush Brush {
                get {
                    if (_tool == null)
                        _tool = _factory();
                    return _tool;
                }
            }

            public static implicit operator Brush(BrushVisitor visitor)
            {
                return visitor.Brush;
            }

            public override void Visit(CanvasGradient<Color> instance)
            {
                var linear = instance as LinearGradient<float, Color>;
                if (linear != null) {
                    _factory = () => Create(linear);
                    Dispose();
                    return;
                }

                var radial = instance as RadialGradient<float, Color>;
                if (radial != null) {
                    _factory = () => Create(linear);
                    Dispose();
                    return;
                }

                throw new NotSupportedException();
            }

            private Brush Create(LinearGradient<float, Color> instance)
            {
                var count = instance.Colors.Count;
                if (count == 0)
                    return CreateSolidBrush(Color.Transparent);


                if (count == 1)
                    return CreateSolidBrush(instance.Colors[0]);

                var startColor = instance.Colors[0];
                var endColor = instance.Colors[count - 1];
                var brush = new LinearGradientBrush(new PointF(instance.X0, instance.Y0), new PointF(instance.X1, instance.Y1), startColor, endColor);
                //brush.WrapMode = WrapMode.Clamp;

                var start = 0;
                var end = count;
                if (!instance.Offsets[0].Equals(0f)) {
                    start = 1;
                    end++;
                }
                if (!instance.Offsets[count - 1].Equals(1f)) {
                    end++;
                }

                if (count > 0) {
                    var blend = new ColorBlend(end);
                    if (start > 0) {
                        blend.Positions[0] = 0f;
                        blend.Colors[0] = startColor;
                    }
                    for (int i = 0; i < count; i++) {
                        blend.Positions[start] = instance.Offsets[i];
                        blend.Colors[start] = instance.Colors[i];
                        start++;
                    }
                    if (start < end) {
                        blend.Positions[start] = 1f;
                        blend.Colors[start] = endColor;
                    }
                    brush.InterpolationColors = blend;
                }
                return brush;
            }

            private Brush Create(RadialGradient<float, Color> instance)
            {
                throw new NotSupportedException();
            }

            public override void Visit(CanvasPattern instance)
            {
                throw new NotSupportedException();
            }

            Brush CreateSolidBrush(Color color)
            {
                var alpha = _canvas._globalAlpha < 1f ? Color.FromArgb((int)(255 * _canvas._globalAlpha), color) : color;
                return new SolidBrush(alpha);
            }

            public override void Visit(Color instance)
            {
                _factory = () => CreateSolidBrush(instance);
                Dispose();
            }
        }

        #endregion

        #region DrawingState 

        //The current transformation matrix.
        //The current clipping region.
        //The current values of the following attributes: strokeStyle, fillStyle, globalAlpha, lineWidth, lineCap, lineJoin, miterLimit, lineDashOffset, shadowOffsetX, shadowOffsetY, shadowBlur, shadowColor, filter, globalCompositeOperation, font, textAlign, textBaseline, direction, imageSmoothingEnabled, imageSmoothingQuality.
        //The current dash list.
        struct DrawingState
        {
            GraphicsState savedState; // contains transformation matrix, clipping region, ImageSmoothingQualityEnabled
            Variant<Color, CanvasGradient<Color>, CanvasPattern> fillStyle;
            Variant<Color, CanvasGradient<Color>, CanvasPattern> strokeStyle;
            float lineWidth;
            LineCap lineCap;
            LineJoin lineJoin;
            float miterLimit;
            float[] dashPattern;
            float dashOffset;

            ImageSmoothingQuality imageSmoothingQuality;
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
        private float _x;
        private float _y;
        private PointF _start;
        private ImageSmoothingQuality _imageSmoothingQuality;
        private float _lineWidth;
        private LineCap _lineCap;
        private LineJoin _lineJoin;
        private float _miterLimit;
        private float[] _dashPattern;
        private float _dashOffset;
        private float _globalAlpha;

        public GraphicsCanvas(Graphics graphics, Action<Graphics> disposer = null)
        {
            g = graphics;
            ImageSmoothingQuality = ImageSmoothingQuality.Low;
            g.SmoothingMode = SmoothingMode.HighQuality;
            if (disposer == null)
                _disposer = _ => _.Dispose();
            else
                _disposer = disposer;

            _lineWidth = 1f;
            _lineCap = System.Drawing.Drawing2D.LineCap.Flat;
            _lineJoin = System.Drawing.Drawing2D.LineJoin.MiterClipped;
            _miterLimit = 10f;
            _globalAlpha = 1f;

            _pen = new PenVisitor(this);
            _brush = new BrushVisitor(this);

            FillStyle = Color.Black;
            StrokeStyle = Color.Black;

        }

        void IDisposable.Dispose()
        {
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

        public Variant<Color, CanvasGradient<Color>, CanvasPattern> FillStyle {
            get {
                return _fillStyle;
            }
            set {
                _fillStyle = value;
                _fillStyle.Visit(_brush);
            }
        }
        Variant<Color, CanvasGradient<Color>, CanvasPattern> _fillStyle;

        public Variant<Color, CanvasGradient<Color>, CanvasPattern> StrokeStyle {
            get {
                return _strokeStyle;
            }
            set {
                _strokeStyle = value;
                _strokeStyle.Visit(_pen);
            }
        }
        Variant<Color, CanvasGradient<Color>, CanvasPattern> _strokeStyle;

        public CanvasGradient<Color> CreateLinearGradient(float x0, float y0, float x1, float y1)
        {
            return new LinearGradient<float, Color>(x0, y0, x1, y1);
        }

        public CanvasPattern CreatePattern(ICanvasImageSource image, Repetition repetition = Repetition.NoRepeat)
        {
            throw new NotImplementedException();
        }

        public CanvasGradient<Color> CreateRadialGradient(float x0, float y0, float r0, float x1, float y1, float r1)
        {
            return new RadialGradient<float, Color>(x0, y0, r0, x1, y1, r1);
        }

        #endregion

        #region ICanvasRect<float>

        public void ClearRect(float x, float y, float w, float h)
        {
            using (GraphicsPath path = new GraphicsPath()) {
                path.AddRectangle(new RectangleF(x, y, w, h));
                g.SetClip(path);
                g.Clear(Color.Transparent);
                g.ResetClip();
            }
        }

        public void FillRect(float x, float y, float w, float h)
        {
            g.FillRectangle(_brush, x, y, w, h);
        }

        public void StrokeRect(float x, float y, float w, float h)
        {
            g.DrawRectangle(_pen, x, y, w, h);
        }

        #endregion

        #region ICanvasPath<float>

        private GraphicsPath CurrentPath {
            get { return _path; }
        }

        public void ClosePath()
        {
            CurrentPath.CloseFigure();
            _x = _start.X;
            _y = _start.Y;
        }

        public void MoveTo(float x, float y)
        {
            _path.StartFigure();
            _start = new PointF(x, y);
            _x = x;
            _y = y;
        }

        public void LineTo(float x, float y)
        {
            CurrentPath.AddLine(_x, _y, x, y);
            _x = x;
            _y = y;
        }

        public void QuadraticCurveTo(float cpx, float cpy, float x, float y)
        {
            BezierCurveTo(_x / 3 + 2 * cpx / 3, _y / 3 + 2 * cpy / 3, x / 3 + 2 * cpx / 3, y / 3 + 2 * cpy / 3, x, y);
        }

        public void BezierCurveTo(float cp1x, float cp1y, float cp2x, float cp2y, float x, float y)
        {
            CurrentPath.AddBezier(_x, _y, cp1x, cp1y, cp2x, cp2y, x, y);
            _x = x;
            _y = y;
        }

        public void ArcTo(float x1, float y1, float x2, float y2, float radius)
        {
            throw new NotImplementedException();
        }

        public void ArcTo(float x1, float y1, float x2, float y2, float radiusX, float radiusY, float rotation)
        {
            throw new NotImplementedException();
        }

        public void Rect(float x, float y, float w, float h)
        {
            BeginPath();
            CurrentPath.AddRectangle(new RectangleF(x, y, w, h));
            _x = x;
            _y = y;
        }

        public void Arc(float x, float y, float radius, float startAngle, float endAngle, bool anticlockwise)
        {
            var sweepAngle = endAngle - startAngle;
            if (!anticlockwise)
                _path.AddArc(x - radius, y - radius, 2 * radius, 2 * radius, Deg(startAngle), Swep(sweepAngle));
            else
                _path.AddArc(x - radius, y - radius, 2 * radius, 2 * radius, Deg(endAngle), Swep(TWO_PI - sweepAngle));
        }

        static float Deg(float value)
        {
            value = 180f * value / PI;
            while (value < 0f)
                value += 360f;
            if (value > 360f)
                value = 360f;
            return value;
        }

        static float Swep(float value)
        {
            value = 180f * value / PI;
            while (value <= 0f) // sweep cannot be 0
                value += 360f;
            if (value > 360f)
                value = 360f;
            return value;
        }

        public void Ellipse(float x, float y, float radiusX, float radiusY, float rotation, float startAngle, float endAngle, bool anticlockwise)
        {
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

        public void BeginPath()
        {
            if (_path != null)
                _path.Dispose();
            _path = new GraphicsPath(FillMode.Alternate);
            _path.StartFigure();
        }

        FillMode Map(CanvasFillRule value)
        {
            switch (value) {
            case CanvasFillRule.NonZero:
                return FillMode.Winding;
            case CanvasFillRule.EvenOdd:
                return FillMode.Alternate;
            default:
                throw new InvalidOperationException();
            }
        }

        public void Fill(CanvasFillRule fillRule)
        {
            CurrentPath.FillMode = Map(fillRule);
            g.FillPath(_brush, CurrentPath);
        }

        public void Fill(Path2D<float> path, CanvasFillRule fillRule)
        {
            throw new NotImplementedException();
        }

        public void Stroke()
        {
            g.DrawPath(_pen, CurrentPath);
        }

        public void Stroke(Path2D<float> path)
        {
            //g.DrawPath(_pen, CurrentPath);
        }

        public void Clip(CanvasFillRule fillRule)
        {
            throw new NotImplementedException();
        }

        public void Clip(Path2D<float> path, CanvasFillRule fillRule)
        {
            throw new NotImplementedException();
        }

        public void ResetClip()
        {
            throw new NotImplementedException();
        }

        public bool IsPointInPath(float x, float y, CanvasFillRule fillRule)
        {
            throw new NotImplementedException();
        }

        public bool IsPointInPath(Path2D<float> path, float x, float y, CanvasFillRule fillRule)
        {
            throw new NotImplementedException();
        }

        public bool IsPointInStroke(float x, float y)
        {
            throw new NotImplementedException();
        }

        public bool IsPointInStroke(Path2D<float> path, float x, float y)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICanvasPathDrawingStyles<T> members

        public float LineWidth {
            get {
                return _lineWidth;
            }
            set {
                Guard.Positive(value);
                if (_lineWidth != value) {
                    _lineWidth = value;
                    _pen.Dispose();
                }
            }
        }

        public CanvasLineCap LineCap {
            get {
                switch (_lineCap) {
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
                LineCap lineCap;
                switch (value) {
                case CanvasLineCap.Butt:
                    lineCap = System.Drawing.Drawing2D.LineCap.Flat;
                    break;
                case CanvasLineCap.Round:
                    lineCap = System.Drawing.Drawing2D.LineCap.Round;
                    break;
                case CanvasLineCap.Square:
                    lineCap = System.Drawing.Drawing2D.LineCap.Square;
                    break;
                default:
                    throw new ArgumentException();
                }
                if (_lineCap != lineCap) {
                    _lineCap = lineCap;
                    _pen.Dispose();
                }
            }
        }

        public CanvasLineJoin LineJoin {
            get {
                switch (_lineJoin) {
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
                LineJoin lineJoin;
                switch (value) {
                case CanvasLineJoin.Round:
                    lineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                    break;
                case CanvasLineJoin.Bevel:
                    lineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                    break;
                case CanvasLineJoin.Miter:
                    lineJoin = System.Drawing.Drawing2D.LineJoin.MiterClipped;
                    break;
                default:
                    throw new ArgumentException();
                }
                if (_lineJoin != lineJoin) {
                    _lineJoin = lineJoin;
                    _pen.Dispose();
                }
            }
        }

        public float MiterLimit {
            get {
                return _miterLimit;
            }
            set {
                Guard.Positive(value);
                if (_miterLimit != value) {
                    _miterLimit = value;
                    _pen.Dispose();
                }
            }
        }

        public float[] LineDash {
            get {
                return _dashPattern ?? Array.Empty<float>();
            }
            set {
                if (!DashPatternEquals(value)) {
                    SetDashPattern(value);
                    _pen.Dispose();
                }
            }
        }

        public float LineDashOffset {
            get {
                return _dashOffset;
            }
            set {
                Guard.Finite(value);
                if (_dashOffset != value) {
                    _dashOffset = value;
                    _pen.Dispose();
                }
            }
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

        #endregion

        #region Helpers

        InterpolationMode Map(ImageSmoothingQuality value)
        {
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

        private void SetDashPattern(float[] value)
        {
            if (value == null || value.Length == 0) {
                _dashPattern = null;
            } else if (value.Length % 1 == 1) {
                _dashPattern = new float[value.Length * 2];
                Array.Copy(value, _dashPattern, value.Length);
                Array.Copy(value, 0, _dashPattern, value.Length, value.Length);
            } else {
                _dashPattern = (float[])value.Clone();
            }
        }

        private bool DashPatternEquals(float[] value)
        {
            if (_dashPattern == null) {
                return (value == null || value.Length == 0);
            }
            if (value == null) {
                return _dashPattern == null;
            }
            if (value.Length % 1 == 1) {
                if (_dashPattern.Length != value.Length * 2)
                    return false;
                for (int i = 0; i < value.Length; i++) {
                    if (value[i] != _dashPattern[i] || value[i] != _dashPattern[i + value.Length])
                        return false;
                }
                return true;
            } else {
                if (_dashPattern.Length != value.Length)
                    return false;
                for (int i = 0; i < value.Length; i++) {
                    if (value[i] != _dashPattern[i])
                        return false;
                }
                return true;
            }
        }

        #endregion

        #region ICanvasCompositing<float>

        public float GlobalAlpha {
            get {
                return _globalAlpha;
            }
            set {
                Guard.AlphaValue(value);
                if (value != _globalAlpha) {
                    _globalAlpha = value;
                    _pen.Dispose();
                    _brush.Dispose();
                }
            }
        }

        public GlobalCompositeOperation GlobalCompositeOperation {
            get {
                switch (g.CompositingMode) {
                case CompositingMode.SourceOver:
                    return GlobalCompositeOperation.SourceOver;
                case CompositingMode.SourceCopy:
                    return GlobalCompositeOperation.Copy;
                default:
                    throw new InvalidCastException();
                }
            }
            set {
                switch (value) {
                case GlobalCompositeOperation.SourceOver:
                    g.CompositingMode = CompositingMode.SourceOver;
                    break;
                case GlobalCompositeOperation.Copy:
                    g.CompositingMode = CompositingMode.SourceCopy;
                    break;
                default:
                    throw new ArgumentException();
                }
            }
        }

        #endregion
    }
}