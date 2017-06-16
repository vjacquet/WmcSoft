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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Drawing.Abstractions
{
    public interface IGraphics<T, TColor, TPen, TBrush, TPath, TRegion, TRect, TPoint> : IDisposable
    {
        TRegion Clip { get; set; }
        TRect ClipBounds { get; }
        CompositingMode CompositingMode { get; set; }
        CompositingQuality CompositingQuality { get; set; }
        T DpiX { get; }
        T DpiY { get; }
        InterpolationMode InterpolationMode { get; set; }
        bool IsClipEmpty { get; }
        bool IsVisibleClipEmpty { get; }
        float PageScale { get; set; }
        GraphicsUnit PageUnit { get; set; }
        PixelOffsetMode PixelOffsetMode { get; set; }
        TPoint RenderingOrigin { get; set; }
        SmoothingMode SmoothingMode { get; set; }
        int TextContrast { get; set; }
        TextRenderingHint TextRenderingHint { get; set; }
        Matrix Transform { get; set; }
        TRect VisibleClipBounds { get; }
        GraphicsContainer BeginContainer();
        GraphicsContainer BeginContainer(TRect dstrect, TRect srcrect, GraphicsUnit unit);
        void Clear(TColor color);
        void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle);
        //
        // Summary:
        //     Draws an arc representing a portion of an ellipse specified by a System.Drawing.Rectangle
        //     structure.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the arc.
        //
        //   rect:
        //     System.Drawing.RectangleF structure that defines the boundaries of the ellipse.
        //
        //   startAngle:
        //     Angle in degrees measured clockwise from the x-axis to the starting point of
        //     the arc.
        //
        //   sweepAngle:
        //     Angle in degrees measured clockwise from the startAngle parameter to ending point
        //     of the arc.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle);
        //
        // Summary:
        //     Draws an arc representing a portion of an ellipse specified by a pair of coordinates,
        //     a width, and a height.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the arc.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the rectangle that defines the ellipse.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the rectangle that defines the ellipse.
        //
        //   width:
        //     Width of the rectangle that defines the ellipse.
        //
        //   height:
        //     Height of the rectangle that defines the ellipse.
        //
        //   startAngle:
        //     Angle in degrees measured clockwise from the x-axis to the starting point of
        //     the arc.
        //
        //   sweepAngle:
        //     Angle in degrees measured clockwise from the startAngle parameter to ending point
        //     of the arc.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle);
        //
        // Summary:
        //     Draws an arc representing a portion of an ellipse specified by a pair of coordinates,
        //     a width, and a height.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the arc.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the rectangle that defines the ellipse.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the rectangle that defines the ellipse.
        //
        //   width:
        //     Width of the rectangle that defines the ellipse.
        //
        //   height:
        //     Height of the rectangle that defines the ellipse.
        //
        //   startAngle:
        //     Angle in degrees measured clockwise from the x-axis to the starting point of
        //     the arc.
        //
        //   sweepAngle:
        //     Angle in degrees measured clockwise from the startAngle parameter to ending point
        //     of the arc.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-rects is null.
        //
        //   T:System.ArgumentNullException:
        //     rects is a zero-length array.
        void DrawArc(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle);
        //
        // Summary:
        //     Draws a Bézier spline defined by four System.Drawing.Point structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen structure that determines the color, width, and style of the
        //     curve.
        //
        //   pt1:
        //     System.Drawing.Point structure that represents the starting point of the curve.
        //
        //   pt2:
        //     System.Drawing.Point structure that represents the first control point for the
        //     curve.
        //
        //   pt3:
        //     System.Drawing.Point structure that represents the second control point for the
        //     curve.
        //
        //   pt4:
        //     System.Drawing.Point structure that represents the ending point of the curve.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4);
        //
        // Summary:
        //     Draws a Bézier spline defined by four System.Drawing.PointF structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the curve.
        //
        //   pt1:
        //     System.Drawing.PointF structure that represents the starting point of the curve.
        //
        //   pt2:
        //     System.Drawing.PointF structure that represents the first control point for the
        //     curve.
        //
        //   pt3:
        //     System.Drawing.PointF structure that represents the second control point for
        //     the curve.
        //
        //   pt4:
        //     System.Drawing.PointF structure that represents the ending point of the curve.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4);
        //
        // Summary:
        //     Draws a Bézier spline defined by four ordered pairs of coordinates that represent
        //     points.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the curve.
        //
        //   x1:
        //     The x-coordinate of the starting point of the curve.
        //
        //   y1:
        //     The y-coordinate of the starting point of the curve.
        //
        //   x2:
        //     The x-coordinate of the first control point of the curve.
        //
        //   y2:
        //     The y-coordinate of the first control point of the curve.
        //
        //   x3:
        //     The x-coordinate of the second control point of the curve.
        //
        //   y3:
        //     The y-coordinate of the second control point of the curve.
        //
        //   x4:
        //     The x-coordinate of the ending point of the curve.
        //
        //   y4:
        //     The y-coordinate of the ending point of the curve.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4);
        //
        // Summary:
        //     Draws a series of Bézier splines from an array of System.Drawing.PointF structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the curve.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that represent the points that determine
        //     the curve. The number of points in the array should be a multiple of 3 plus 1,
        //     such as 4, 7, or 10.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawBeziers(Pen pen, PointF[] points);
        //
        // Summary:
        //     Draws a series of Bézier splines from an array of System.Drawing.Point structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the curve.
        //
        //   points:
        //     Array of System.Drawing.Point structures that represent the points that determine
        //     the curve. The number of points in the array should be a multiple of 3 plus 1,
        //     such as 4, 7, or 10.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawBeziers(Pen pen, Point[] points);
        //
        // Summary:
        //     Draws a closed cardinal spline defined by an array of System.Drawing.PointF structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and height of the curve.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that define the spline.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawClosedCurve(Pen pen, PointF[] points);
        //
        // Summary:
        //     Draws a closed cardinal spline defined by an array of System.Drawing.Point structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and height of the curve.
        //
        //   points:
        //     Array of System.Drawing.Point structures that define the spline.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawClosedCurve(Pen pen, Point[] points);
        //
        // Summary:
        //     Draws a closed cardinal spline defined by an array of System.Drawing.PointF structures
        //     using a specified tension.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and height of the curve.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that define the spline.
        //
        //   tension:
        //     Value greater than or equal to 0.0F that specifies the tension of the curve.
        //
        //   fillmode:
        //     Member of the System.Drawing.Drawing2D.FillMode enumeration that determines how
        //     the curve is filled. This parameter is required but is ignored.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawClosedCurve(Pen pen, PointF[] points, float tension, FillMode fillmode);
        //
        // Summary:
        //     Draws a closed cardinal spline defined by an array of System.Drawing.Point structures
        //     using a specified tension.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and height of the curve.
        //
        //   points:
        //     Array of System.Drawing.Point structures that define the spline.
        //
        //   tension:
        //     Value greater than or equal to 0.0F that specifies the tension of the curve.
        //
        //   fillmode:
        //     Member of the System.Drawing.Drawing2D.FillMode enumeration that determines how
        //     the curve is filled. This parameter is required but ignored.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawClosedCurve(Pen pen, Point[] points, float tension, FillMode fillmode);
        //
        // Summary:
        //     Draws a cardinal spline through a specified array of System.Drawing.Point structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and height of the curve.
        //
        //   points:
        //     Array of System.Drawing.Point structures that define the spline.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawCurve(Pen pen, Point[] points);
        //
        // Summary:
        //     Draws a cardinal spline through a specified array of System.Drawing.PointF structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the curve.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that define the spline.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawCurve(Pen pen, PointF[] points);
        //
        // Summary:
        //     Draws a cardinal spline through a specified array of System.Drawing.PointF structures
        //     using a specified tension.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the curve.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that represent the points that define
        //     the curve.
        //
        //   tension:
        //     Value greater than or equal to 0.0F that specifies the tension of the curve.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawCurve(Pen pen, PointF[] points, float tension);
        //
        // Summary:
        //     Draws a cardinal spline through a specified array of System.Drawing.Point structures
        //     using a specified tension.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the curve.
        //
        //   points:
        //     Array of System.Drawing.Point structures that define the spline.
        //
        //   tension:
        //     Value greater than or equal to 0.0F that specifies the tension of the curve.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawCurve(Pen pen, Point[] points, float tension);
        //
        // Summary:
        //     Draws a cardinal spline through a specified array of System.Drawing.PointF structures.
        //     The drawing begins offset from the beginning of the array.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the curve.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that define the spline.
        //
        //   offset:
        //     Offset from the first element in the array of the points parameter to the starting
        //     point in the curve.
        //
        //   numberOfSegments:
        //     Number of segments after the starting point to include in the curve.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments);
        //
        // Summary:
        //     Draws a cardinal spline through a specified array of System.Drawing.PointF structures
        //     using a specified tension. The drawing begins offset from the beginning of the
        //     array.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the curve.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that define the spline.
        //
        //   offset:
        //     Offset from the first element in the array of the points parameter to the starting
        //     point in the curve.
        //
        //   numberOfSegments:
        //     Number of segments after the starting point to include in the curve.
        //
        //   tension:
        //     Value greater than or equal to 0.0F that specifies the tension of the curve.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments, float tension);
        //
        // Summary:
        //     Draws a cardinal spline through a specified array of System.Drawing.Point structures
        //     using a specified tension.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the curve.
        //
        //   points:
        //     Array of System.Drawing.Point structures that define the spline.
        //
        //   offset:
        //     Offset from the first element in the array of the points parameter to the starting
        //     point in the curve.
        //
        //   numberOfSegments:
        //     Number of segments after the starting point to include in the curve.
        //
        //   tension:
        //     Value greater than or equal to 0.0F that specifies the tension of the curve.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawCurve(Pen pen, Point[] points, int offset, int numberOfSegments, float tension);
        //
        // Summary:
        //     Draws an ellipse specified by a bounding System.Drawing.Rectangle structure.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the ellipse.
        //
        //   rect:
        //     System.Drawing.Rectangle structure that defines the boundaries of the ellipse.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawEllipse(Pen pen, Rectangle rect);
        //
        // Summary:
        //     Draws an ellipse defined by a bounding System.Drawing.RectangleF.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the ellipse.
        //
        //   rect:
        //     System.Drawing.RectangleF structure that defines the boundaries of the ellipse.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawEllipse(Pen pen, RectangleF rect);
        //
        // Summary:
        //     Draws an ellipse defined by a bounding rectangle specified by coordinates for
        //     the upper-left corner of the rectangle, a height, and a width.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the ellipse.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse.
        //
        //   width:
        //     Width of the bounding rectangle that defines the ellipse.
        //
        //   height:
        //     Height of the bounding rectangle that defines the ellipse.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawEllipse(Pen pen, int x, int y, int width, int height);
        //
        // Summary:
        //     Draws an ellipse defined by a bounding rectangle specified by a pair of coordinates,
        //     a height, and a width.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the ellipse.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse.
        //
        //   width:
        //     Width of the bounding rectangle that defines the ellipse.
        //
        //   height:
        //     Height of the bounding rectangle that defines the ellipse.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawEllipse(Pen pen, float x, float y, float width, float height);
        //
        // Summary:
        //     Draws the image represented by the specified System.Drawing.Icon within the area
        //     specified by a System.Drawing.Rectangle structure.
        //
        // Parameters:
        //   icon:
        //     System.Drawing.Icon to draw.
        //
        //   targetRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     resulting image on the display surface. The image contained in the icon parameter
        //     is scaled to the dimensions of this rectangular area.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     icon is null.
        void DrawIcon(Icon icon, Rectangle targetRect);
        //
        // Summary:
        //     Draws the image represented by the specified System.Drawing.Icon at the specified
        //     coordinates.
        //
        // Parameters:
        //   icon:
        //     System.Drawing.Icon to draw.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the drawn image.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the drawn image.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     icon is null.
        void DrawIcon(Icon icon, int x, int y);
        //
        // Summary:
        //     Draws the image represented by the specified System.Drawing.Icon without scaling
        //     the image.
        //
        // Parameters:
        //   icon:
        //     System.Drawing.Icon to draw.
        //
        //   targetRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     resulting image. The image is not scaled to fit this rectangle, but retains its
        //     original size. If the image is larger than the rectangle, it is clipped to fit
        //     inside it.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     icon is null.
        void DrawIconUnstretched(Icon icon, Rectangle targetRect);
        //
        // Summary:
        //     Draws the specified System.Drawing.Image, using its original physical size, at
        //     the specified location.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   point:
        //     System.Drawing.PointF structure that represents the upper-left corner of the
        //     drawn image.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, PointF point);
        //
        // Summary:
        //     Draws the specified System.Drawing.Image at the specified location and with the
        //     specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   rect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn image.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Rectangle rect);
        //
        // Summary:
        //     Draws the specified System.Drawing.Image at the specified location and with the
        //     specified shape and size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destPoints:
        //     Array of three System.Drawing.PointF structures that define a parallelogram.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, PointF[] destPoints);
        //
        // Summary:
        //     Draws the specified System.Drawing.Image at the specified location and with the
        //     specified shape and size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destPoints:
        //     Array of three System.Drawing.Point structures that define a parallelogram.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Point[] destPoints);
        //
        // Summary:
        //     Draws the specified System.Drawing.Image at the specified location and with the
        //     specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   rect:
        //     System.Drawing.RectangleF structure that specifies the location and size of the
        //     drawn image.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, RectangleF rect);
        //
        // Summary:
        //     Draws the specified System.Drawing.Image, using its original physical size, at
        //     the specified location.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   point:
        //     System.Drawing.Point structure that represents the location of the upper-left
        //     corner of the drawn image.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Point point);
        //
        // Summary:
        //     Draws the specified System.Drawing.Image, using its original physical size, at
        //     the specified location.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the drawn image.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the drawn image.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, float x, float y);
        //
        // Summary:
        //     Draws the specified image, using its original physical size, at the location
        //     specified by a coordinate pair.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the drawn image.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the drawn image.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, int x, int y);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn image. The image is scaled to fit the rectangle.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the image object
        //     to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used by the srcRect parameter.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destRect:
        //     System.Drawing.RectangleF structure that specifies the location and size of the
        //     drawn image. The image is scaled to fit the rectangle.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the image object
        //     to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used by the srcRect parameter.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destPoints:
        //     Array of three System.Drawing.PointF structures that define a parallelogram.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the image object
        //     to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used by the srcRect parameter.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destPoints:
        //     Array of three System.Drawing.Point structures that define a parallelogram.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the image object
        //     to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used by the srcRect parameter.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit);
        //
        // Summary:
        //     Draws the specified System.Drawing.Image at the specified location and with the
        //     specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the drawn image.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the drawn image.
        //
        //   width:
        //     Width of the drawn image.
        //
        //   height:
        //     Height of the drawn image.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, float x, float y, float width, float height);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destPoints:
        //     Array of three System.Drawing.Point structures that define a parallelogram.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the image object
        //     to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used by the srcRect parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies recoloring and gamma information
        //     for the image object.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr);
        //
        // Summary:
        //     Draws the specified System.Drawing.Image at the specified location and with the
        //     specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the drawn image.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the drawn image.
        //
        //   width:
        //     Width of the drawn image.
        //
        //   height:
        //     Height of the drawn image.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, int x, int y, int width, int height);
        //
        // Summary:
        //     Draws a portion of an image at a specified location.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the drawn image.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the drawn image.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the image object
        //     to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used by the srcRect parameter.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destPoints:
        //     Array of three System.Drawing.PointF structures that define a parallelogram.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the image object
        //     to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used by the srcRect parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies recoloring and gamma information
        //     for the image object.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr);
        //
        // Summary:
        //     Draws a portion of an image at a specified location.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the drawn image.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the drawn image.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the System.Drawing.Image
        //     to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used by the srcRect parameter.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destPoints:
        //     Array of three System.Drawing.PointF structures that define a parallelogram.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the image object
        //     to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used by the srcRect parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies recoloring and gamma information
        //     for the image object.
        //
        //   callback:
        //     System.Drawing.Graphics.DrawImageAbort delegate that specifies a method to call
        //     during the drawing of the image. This method is called frequently to check whether
        //     to stop execution of the System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.PointF[],System.Drawing.RectangleF,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort)
        //     method according to application-determined criteria.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destPoints:
        //     Array of three System.Drawing.PointF structures that define a parallelogram.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the image object
        //     to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used by the srcRect parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies recoloring and gamma information
        //     for the image object.
        //
        //   callback:
        //     System.Drawing.Graphics.DrawImageAbort delegate that specifies a method to call
        //     during the drawing of the image. This method is called frequently to check whether
        //     to stop execution of the System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Point[],System.Drawing.Rectangle,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort)
        //     method according to application-determined criteria.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn image. The image is scaled to fit the rectangle.
        //
        //   srcX:
        //     The x-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcY:
        //     The y-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcWidth:
        //     Width of the portion of the source image to draw.
        //
        //   srcHeight:
        //     Height of the portion of the source image to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used to determine the source rectangle.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destPoints:
        //     Array of three System.Drawing.PointF structures that define a parallelogram.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the image object
        //     to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used by the srcRect parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies recoloring and gamma information
        //     for the image object.
        //
        //   callback:
        //     System.Drawing.Graphics.DrawImageAbort delegate that specifies a method to call
        //     during the drawing of the image. This method is called frequently to check whether
        //     to stop execution of the System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Point[],System.Drawing.Rectangle,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort,System.Int32)
        //     method according to application-determined criteria.
        //
        //   callbackData:
        //     Value specifying additional data for the System.Drawing.Graphics.DrawImageAbort
        //     delegate to use when checking whether to stop execution of the System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Point[],System.Drawing.Rectangle,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort,System.Int32)
        //     method.
        void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback, int callbackData);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn image. The image is scaled to fit the rectangle.
        //
        //   srcX:
        //     The x-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcY:
        //     The y-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcWidth:
        //     Width of the portion of the source image to draw.
        //
        //   srcHeight:
        //     Height of the portion of the source image to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used to determine the source rectangle.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destPoints:
        //     Array of three System.Drawing.PointF structures that define a parallelogram.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the image object
        //     to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used by the srcRect parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies recoloring and gamma information
        //     for the image object.
        //
        //   callback:
        //     System.Drawing.Graphics.DrawImageAbort delegate that specifies a method to call
        //     during the drawing of the image. This method is called frequently to check whether
        //     to stop execution of the System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.PointF[],System.Drawing.RectangleF,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort,System.Int32)
        //     method according to application-determined criteria.
        //
        //   callbackData:
        //     Value specifying additional data for the System.Drawing.Graphics.DrawImageAbort
        //     delegate to use when checking whether to stop execution of the System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.PointF[],System.Drawing.RectangleF,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort,System.Int32)
        //     method.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback, int callbackData);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn image. The image is scaled to fit the rectangle.
        //
        //   srcX:
        //     The x-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcY:
        //     The y-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcWidth:
        //     Width of the portion of the source image to draw.
        //
        //   srcHeight:
        //     Height of the portion of the source image to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used to determine the source rectangle.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies recoloring and gamma information
        //     for the image object.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn image. The image is scaled to fit the rectangle.
        //
        //   srcX:
        //     The x-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcY:
        //     The y-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcWidth:
        //     Width of the portion of the source image to draw.
        //
        //   srcHeight:
        //     Height of the portion of the source image to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used to determine the source rectangle.
        //
        //   imageAttrs:
        //     System.Drawing.Imaging.ImageAttributes that specifies recoloring and gamma information
        //     for the image object.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn image. The image is scaled to fit the rectangle.
        //
        //   srcX:
        //     The x-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcY:
        //     The y-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcWidth:
        //     Width of the portion of the source image to draw.
        //
        //   srcHeight:
        //     Height of the portion of the source image to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used to determine the source rectangle.
        //
        //   imageAttrs:
        //     System.Drawing.Imaging.ImageAttributes that specifies recoloring and gamma information
        //     for the image object.
        //
        //   callback:
        //     System.Drawing.Graphics.DrawImageAbort delegate that specifies a method to call
        //     during the drawing of the image. This method is called frequently to check whether
        //     to stop execution of the System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Rectangle,System.Single,System.Single,System.Single,System.Single,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort)
        //     method according to application-determined criteria.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn image. The image is scaled to fit the rectangle.
        //
        //   srcX:
        //     The x-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcY:
        //     The y-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcWidth:
        //     Width of the portion of the source image to draw.
        //
        //   srcHeight:
        //     Height of the portion of the source image to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used to determine the source rectangle.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies recoloring and gamma information
        //     for image.
        //
        //   callback:
        //     System.Drawing.Graphics.DrawImageAbort delegate that specifies a method to call
        //     during the drawing of the image. This method is called frequently to check whether
        //     to stop execution of the System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Rectangle,System.Int32,System.Int32,System.Int32,System.Int32,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort)
        //     method according to application-determined criteria.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn image. The image is scaled to fit the rectangle.
        //
        //   srcX:
        //     The x-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcY:
        //     The y-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcWidth:
        //     Width of the portion of the source image to draw.
        //
        //   srcHeight:
        //     Height of the portion of the source image to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used to determine the source rectangle.
        //
        //   imageAttrs:
        //     System.Drawing.Imaging.ImageAttributes that specifies recoloring and gamma information
        //     for the image object.
        //
        //   callback:
        //     System.Drawing.Graphics.DrawImageAbort delegate that specifies a method to call
        //     during the drawing of the image. This method is called frequently to check whether
        //     to stop execution of the System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Rectangle,System.Single,System.Single,System.Single,System.Single,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort,System.IntPtr)
        //     method according to application-determined criteria.
        //
        //   callbackData:
        //     Value specifying additional data for the System.Drawing.Graphics.DrawImageAbort
        //     delegate to use when checking whether to stop execution of the DrawImage method.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData);
        //
        // Summary:
        //     Draws the specified portion of the specified System.Drawing.Image at the specified
        //     location and with the specified size.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn image. The image is scaled to fit the rectangle.
        //
        //   srcX:
        //     The x-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcY:
        //     The y-coordinate of the upper-left corner of the portion of the source image
        //     to draw.
        //
        //   srcWidth:
        //     Width of the portion of the source image to draw.
        //
        //   srcHeight:
        //     Height of the portion of the source image to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the units
        //     of measure used to determine the source rectangle.
        //
        //   imageAttrs:
        //     System.Drawing.Imaging.ImageAttributes that specifies recoloring and gamma information
        //     for the image object.
        //
        //   callback:
        //     System.Drawing.Graphics.DrawImageAbort delegate that specifies a method to call
        //     during the drawing of the image. This method is called frequently to check whether
        //     to stop execution of the System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Rectangle,System.Int32,System.Int32,System.Int32,System.Int32,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort,System.IntPtr)
        //     method according to application-determined criteria.
        //
        //   callbackData:
        //     Value specifying additional data for the System.Drawing.Graphics.DrawImageAbort
        //     delegate to use when checking whether to stop execution of the DrawImage method.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData);
        //
        // Summary:
        //     Draws a specified image using its original physical size at a specified location.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   point:
        //     System.Drawing.Point structure that specifies the upper-left corner of the drawn
        //     image.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImageUnscaled(Image image, Point point);
        //
        // Summary:
        //     Draws a specified image using its original physical size at a specified location.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   rect:
        //     System.Drawing.Rectangle that specifies the upper-left corner of the drawn image.
        //     The X and Y properties of the rectangle specify the upper-left corner. The Width
        //     and Height properties are ignored.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImageUnscaled(Image image, Rectangle rect);
        //
        // Summary:
        //     Draws the specified image using its original physical size at the location specified
        //     by a coordinate pair.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the drawn image.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the drawn image.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImageUnscaled(Image image, int x, int y);
        //
        // Summary:
        //     Draws a specified image using its original physical size at a specified location.
        //
        // Parameters:
        //   image:
        //     System.Drawing.Image to draw.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the drawn image.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the drawn image.
        //
        //   width:
        //     Not used.
        //
        //   height:
        //     Not used.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImageUnscaled(Image image, int x, int y, int width, int height);
        //
        // Summary:
        //     Draws the specified image without scaling and clips it, if necessary, to fit
        //     in the specified rectangle.
        //
        // Parameters:
        //   image:
        //     The System.Drawing.Image to draw.
        //
        //   rect:
        //     The System.Drawing.Rectangle in which to draw the image.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     image is null.
        void DrawImageUnscaledAndClipped(Image image, Rectangle rect);
        //
        // Summary:
        //     Draws a line connecting two System.Drawing.Point structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the line.
        //
        //   pt1:
        //     System.Drawing.Point structure that represents the first point to connect.
        //
        //   pt2:
        //     System.Drawing.Point structure that represents the second point to connect.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawLine(Pen pen, Point pt1, Point pt2);
        //
        // Summary:
        //     Draws a line connecting two System.Drawing.PointF structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the line.
        //
        //   pt1:
        //     System.Drawing.PointF structure that represents the first point to connect.
        //
        //   pt2:
        //     System.Drawing.PointF structure that represents the second point to connect.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawLine(Pen pen, PointF pt1, PointF pt2);
        //
        // Summary:
        //     Draws a line connecting the two points specified by the coordinate pairs.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the line.
        //
        //   x1:
        //     The x-coordinate of the first point.
        //
        //   y1:
        //     The y-coordinate of the first point.
        //
        //   x2:
        //     The x-coordinate of the second point.
        //
        //   y2:
        //     The y-coordinate of the second point.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawLine(Pen pen, int x1, int y1, int x2, int y2);
        //
        // Summary:
        //     Draws a line connecting the two points specified by the coordinate pairs.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the line.
        //
        //   x1:
        //     The x-coordinate of the first point.
        //
        //   y1:
        //     The y-coordinate of the first point.
        //
        //   x2:
        //     The x-coordinate of the second point.
        //
        //   y2:
        //     The y-coordinate of the second point.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawLine(Pen pen, float x1, float y1, float x2, float y2);
        //
        // Summary:
        //     Draws a series of line segments that connect an array of System.Drawing.Point
        //     structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the line segments.
        //
        //   points:
        //     Array of System.Drawing.Point structures that represent the points to connect.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawLines(Pen pen, Point[] points);
        //
        // Summary:
        //     Draws a series of line segments that connect an array of System.Drawing.PointF
        //     structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the line segments.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that represent the points to connect.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawLines(Pen pen, PointF[] points);
        //
        // Summary:
        //     Draws a System.Drawing.Drawing2D.GraphicsPath.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the path.
        //
        //   path:
        //     System.Drawing.Drawing2D.GraphicsPath to draw.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-path is null.
        void DrawPath(Pen pen, GraphicsPath path);
        //
        // Summary:
        //     Draws a pie shape defined by an ellipse specified by a System.Drawing.Rectangle
        //     structure and two radial lines.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the pie shape.
        //
        //   rect:
        //     System.Drawing.Rectangle structure that represents the bounding rectangle that
        //     defines the ellipse from which the pie shape comes.
        //
        //   startAngle:
        //     Angle measured in degrees clockwise from the x-axis to the first side of the
        //     pie shape.
        //
        //   sweepAngle:
        //     Angle measured in degrees clockwise from the startAngle parameter to the second
        //     side of the pie shape.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle);
        //
        // Summary:
        //     Draws a pie shape defined by an ellipse specified by a System.Drawing.RectangleF
        //     structure and two radial lines.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the pie shape.
        //
        //   rect:
        //     System.Drawing.RectangleF structure that represents the bounding rectangle that
        //     defines the ellipse from which the pie shape comes.
        //
        //   startAngle:
        //     Angle measured in degrees clockwise from the x-axis to the first side of the
        //     pie shape.
        //
        //   sweepAngle:
        //     Angle measured in degrees clockwise from the startAngle parameter to the second
        //     side of the pie shape.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle);
        //
        // Summary:
        //     Draws a pie shape defined by an ellipse specified by a coordinate pair, a width,
        //     a height, and two radial lines.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the pie shape.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse from which the pie shape comes.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse from which the pie shape comes.
        //
        //   width:
        //     Width of the bounding rectangle that defines the ellipse from which the pie shape
        //     comes.
        //
        //   height:
        //     Height of the bounding rectangle that defines the ellipse from which the pie
        //     shape comes.
        //
        //   startAngle:
        //     Angle measured in degrees clockwise from the x-axis to the first side of the
        //     pie shape.
        //
        //   sweepAngle:
        //     Angle measured in degrees clockwise from the startAngle parameter to the second
        //     side of the pie shape.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawPie(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle);
        //
        // Summary:
        //     Draws a pie shape defined by an ellipse specified by a coordinate pair, a width,
        //     a height, and two radial lines.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the pie shape.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse from which the pie shape comes.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse from which the pie shape comes.
        //
        //   width:
        //     Width of the bounding rectangle that defines the ellipse from which the pie shape
        //     comes.
        //
        //   height:
        //     Height of the bounding rectangle that defines the ellipse from which the pie
        //     shape comes.
        //
        //   startAngle:
        //     Angle measured in degrees clockwise from the x-axis to the first side of the
        //     pie shape.
        //
        //   sweepAngle:
        //     Angle measured in degrees clockwise from the startAngle parameter to the second
        //     side of the pie shape.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawPie(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle);
        //
        // Summary:
        //     Draws a polygon defined by an array of System.Drawing.Point structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the polygon.
        //
        //   points:
        //     Array of System.Drawing.Point structures that represent the vertices of the polygon.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawPolygon(Pen pen, Point[] points);
        //
        // Summary:
        //     Draws a polygon defined by an array of System.Drawing.PointF structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the polygon.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that represent the vertices of the
        //     polygon.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-points is null.
        void DrawPolygon(Pen pen, PointF[] points);
        //
        // Summary:
        //     Draws a rectangle specified by a System.Drawing.Rectangle structure.
        //
        // Parameters:
        //   pen:
        //     A System.Drawing.Pen that determines the color, width, and style of the rectangle.
        //
        //   rect:
        //     A System.Drawing.Rectangle structure that represents the rectangle to draw.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawRectangle(Pen pen, Rectangle rect);
        //
        // Summary:
        //     Draws a rectangle specified by a coordinate pair, a width, and a height.
        //
        // Parameters:
        //   pen:
        //     A System.Drawing.Pen that determines the color, width, and style of the rectangle.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the rectangle to draw.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the rectangle to draw.
        //
        //   width:
        //     The width of the rectangle to draw.
        //
        //   height:
        //     The height of the rectangle to draw.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawRectangle(Pen pen, float x, float y, float width, float height);
        //
        // Summary:
        //     Draws a rectangle specified by a coordinate pair, a width, and a height.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the rectangle.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the rectangle to draw.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the rectangle to draw.
        //
        //   width:
        //     Width of the rectangle to draw.
        //
        //   height:
        //     Height of the rectangle to draw.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.
        void DrawRectangle(Pen pen, int x, int y, int width, int height);
        //
        // Summary:
        //     Draws a series of rectangles specified by System.Drawing.Rectangle structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the outlines
        //     of the rectangles.
        //
        //   rects:
        //     Array of System.Drawing.Rectangle structures that represent the rectangles to
        //     draw.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-rects is null.
        //
        //   T:System.ArgumentException:
        //     rects is a zero-length array.
        void DrawRectangles(Pen pen, Rectangle[] rects);
        //
        // Summary:
        //     Draws a series of rectangles specified by System.Drawing.RectangleF structures.
        //
        // Parameters:
        //   pen:
        //     System.Drawing.Pen that determines the color, width, and style of the outlines
        //     of the rectangles.
        //
        //   rects:
        //     Array of System.Drawing.RectangleF structures that represent the rectangles to
        //     draw.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     pen is null.-or-rects is null.
        //
        //   T:System.ArgumentException:
        //     rects is a zero-length array.
        void DrawRectangles(Pen pen, RectangleF[] rects);
        //
        // Summary:
        //     Draws the specified text string in the specified rectangle with the specified
        //     System.Drawing.Brush and System.Drawing.Font objects.
        //
        // Parameters:
        //   s:
        //     String to draw.
        //
        //   font:
        //     System.Drawing.Font that defines the text format of the string.
        //
        //   brush:
        //     System.Drawing.Brush that determines the color and texture of the drawn text.
        //
        //   layoutRectangle:
        //     System.Drawing.RectangleF structure that specifies the location of the drawn
        //     text.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-s is null.
        void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle);
        //
        // Summary:
        //     Draws the specified text string at the specified location with the specified
        //     System.Drawing.Brush and System.Drawing.Font objects.
        //
        // Parameters:
        //   s:
        //     String to draw.
        //
        //   font:
        //     System.Drawing.Font that defines the text format of the string.
        //
        //   brush:
        //     System.Drawing.Brush that determines the color and texture of the drawn text.
        //
        //   point:
        //     System.Drawing.PointF structure that specifies the upper-left corner of the drawn
        //     text.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-s is null.
        void DrawString(string s, Font font, Brush brush, PointF point);
        //
        // Summary:
        //     Draws the specified text string at the specified location with the specified
        //     System.Drawing.Brush and System.Drawing.Font objects.
        //
        // Parameters:
        //   s:
        //     String to draw.
        //
        //   font:
        //     System.Drawing.Font that defines the text format of the string.
        //
        //   brush:
        //     System.Drawing.Brush that determines the color and texture of the drawn text.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the drawn text.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the drawn text.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-s is null.
        void DrawString(string s, Font font, Brush brush, float x, float y);
        //
        // Summary:
        //     Draws the specified text string in the specified rectangle with the specified
        //     System.Drawing.Brush and System.Drawing.Font objects using the formatting attributes
        //     of the specified System.Drawing.StringFormat.
        //
        // Parameters:
        //   s:
        //     String to draw.
        //
        //   font:
        //     System.Drawing.Font that defines the text format of the string.
        //
        //   brush:
        //     System.Drawing.Brush that determines the color and texture of the drawn text.
        //
        //   layoutRectangle:
        //     System.Drawing.RectangleF structure that specifies the location of the drawn
        //     text.
        //
        //   format:
        //     System.Drawing.StringFormat that specifies formatting attributes, such as line
        //     spacing and alignment, that are applied to the drawn text.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-s is null.
        void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format);
        //
        // Summary:
        //     Draws the specified text string at the specified location with the specified
        //     System.Drawing.Brush and System.Drawing.Font objects using the formatting attributes
        //     of the specified System.Drawing.StringFormat.
        //
        // Parameters:
        //   s:
        //     String to draw.
        //
        //   font:
        //     System.Drawing.Font that defines the text format of the string.
        //
        //   brush:
        //     System.Drawing.Brush that determines the color and texture of the drawn text.
        //
        //   point:
        //     System.Drawing.PointF structure that specifies the upper-left corner of the drawn
        //     text.
        //
        //   format:
        //     System.Drawing.StringFormat that specifies formatting attributes, such as line
        //     spacing and alignment, that are applied to the drawn text.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-s is null.
        void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format);
        //
        // Summary:
        //     Draws the specified text string at the specified location with the specified
        //     System.Drawing.Brush and System.Drawing.Font objects using the formatting attributes
        //     of the specified System.Drawing.StringFormat.
        //
        // Parameters:
        //   s:
        //     String to draw.
        //
        //   font:
        //     System.Drawing.Font that defines the text format of the string.
        //
        //   brush:
        //     System.Drawing.Brush that determines the color and texture of the drawn text.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the drawn text.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the drawn text.
        //
        //   format:
        //     System.Drawing.StringFormat that specifies formatting attributes, such as line
        //     spacing and alignment, that are applied to the drawn text.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-s is null.
        void DrawString(string s, Font font, Brush brush, float x, float y, StringFormat format);
        //
        // Summary:
        //     Closes the current graphics container and restores the state of this System.Drawing.Graphics
        //     to the state saved by a call to the System.Drawing.Graphics.BeginContainer method.
        //
        // Parameters:
        //   container:
        //     System.Drawing.Drawing2D.GraphicsContainer that represents the container this
        //     method restores.
        void EndContainer(GraphicsContainer container);
        //
        // Summary:
        //     Sends the records in the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display at a specified point.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoint:
        //     System.Drawing.Point structure that specifies the location of the upper-left
        //     corner of the drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        void EnumerateMetafile(Metafile metafile, Point destPoint, EnumerateMetafileProc callback);
        //
        // Summary:
        //     Sends the records of the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display in a specified rectangle.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        void EnumerateMetafile(Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback);
        //
        // Summary:
        //     Sends the records in the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display at a specified point.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoint:
        //     System.Drawing.PointF structure that specifies the location of the upper-left
        //     corner of the drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        void EnumerateMetafile(Metafile metafile, PointF destPoint, EnumerateMetafileProc callback);
        //
        // Summary:
        //     Sends the records in the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display in a specified parallelogram.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoints:
        //     Array of three System.Drawing.Point structures that define a parallelogram that
        //     determines the size and location of the drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        void EnumerateMetafile(Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback);
        //
        // Summary:
        //     Sends the records of the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display in a specified rectangle.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destRect:
        //     System.Drawing.RectangleF structure that specifies the location and size of the
        //     drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        void EnumerateMetafile(Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback);
        //
        // Summary:
        //     Sends the records in the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display in a specified parallelogram.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoints:
        //     Array of three System.Drawing.PointF structures that define a parallelogram that
        //     determines the size and location of the drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        void EnumerateMetafile(Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback);
        //
        // Summary:
        //     Sends the records of the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display in a specified rectangle.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destRect:
        //     System.Drawing.RectangleF structure that specifies the location and size of the
        //     drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        void EnumerateMetafile(Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback, IntPtr callbackData);
        //
        // Summary:
        //     Sends the records of the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display in a specified rectangle.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        void EnumerateMetafile(Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback, IntPtr callbackData);
        //
        // Summary:
        //     Sends the records in the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display in a specified parallelogram.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoints:
        //     Array of three System.Drawing.PointF structures that define a parallelogram that
        //     determines the size and location of the drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        void EnumerateMetafile(Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData);
        //
        // Summary:
        //     Sends the records in the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display at a specified point.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoint:
        //     System.Drawing.PointF structure that specifies the location of the upper-left
        //     corner of the drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        void EnumerateMetafile(Metafile metafile, PointF destPoint, EnumerateMetafileProc callback, IntPtr callbackData);
        //
        // Summary:
        //     Sends the records in the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display in a specified parallelogram.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoints:
        //     Array of three System.Drawing.Point structures that define a parallelogram that
        //     determines the size and location of the drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        void EnumerateMetafile(Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData);
        //
        // Summary:
        //     Sends the records in the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display at a specified point.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoint:
        //     System.Drawing.Point structure that specifies the location of the upper-left
        //     corner of the drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        void EnumerateMetafile(Metafile metafile, Point destPoint, EnumerateMetafileProc callback, IntPtr callbackData);
        //
        // Summary:
        //     Sends the records in a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display in a specified parallelogram.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoints:
        //     Array of three System.Drawing.PointF structures that define a parallelogram that
        //     determines the size and location of the drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structures that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback);
        //
        // Summary:
        //     Sends the records in a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display in a specified parallelogram.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoints:
        //     Array of three System.Drawing.Point structures that define a parallelogram that
        //     determines the size and location of the drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback);
        //
        // Summary:
        //     Sends the records of a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display in a specified rectangle.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback);
        //
        // Summary:
        //     Sends the records of a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display in a specified rectangle.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destRect:
        //     System.Drawing.RectangleF structure that specifies the location and size of the
        //     drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback);
        //
        // Summary:
        //     Sends the records in a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display at a specified point.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoint:
        //     System.Drawing.Point structure that specifies the location of the upper-left
        //     corner of the drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback);
        //
        // Summary:
        //     Sends the records in the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display in a specified parallelogram using specified
        //     image attributes.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoints:
        //     Array of three System.Drawing.Point structures that define a parallelogram that
        //     determines the size and location of the drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies image attribute information
        //     for the drawn image.
        void EnumerateMetafile(Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr);
        //
        // Summary:
        //     Sends the records in the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display in a specified parallelogram using specified
        //     image attributes.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoints:
        //     Array of three System.Drawing.PointF structures that define a parallelogram that
        //     determines the size and location of the drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies image attribute information
        //     for the drawn image.
        void EnumerateMetafile(Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr);
        //
        // Summary:
        //     Sends the records of the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display in a specified rectangle using specified
        //     image attributes.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies image attribute information
        //     for the drawn image.
        void EnumerateMetafile(Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr);
        //
        // Summary:
        //     Sends the records of the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display in a specified rectangle using specified
        //     image attributes.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destRect:
        //     System.Drawing.RectangleF structure that specifies the location and size of the
        //     drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies image attribute information
        //     for the drawn image.
        void EnumerateMetafile(Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr);
        //
        // Summary:
        //     Sends the records in the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display at a specified point using specified image
        //     attributes.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoint:
        //     System.Drawing.Point structure that specifies the location of the upper-left
        //     corner of the drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies image attribute information
        //     for the drawn image.
        void EnumerateMetafile(Metafile metafile, Point destPoint, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr);
        //
        // Summary:
        //     Sends the records in the specified System.Drawing.Imaging.Metafile, one at a
        //     time, to a callback method for display at a specified point using specified image
        //     attributes.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoint:
        //     System.Drawing.PointF structure that specifies the location of the upper-left
        //     corner of the drawn metafile.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies image attribute information
        //     for the drawn image.
        void EnumerateMetafile(Metafile metafile, PointF destPoint, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr);
        //
        // Summary:
        //     Sends the records in a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display at a specified point.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoint:
        //     System.Drawing.PointF structure that specifies the location of the upper-left
        //     corner of the drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback);
        //
        // Summary:
        //     Sends the records in a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display in a specified parallelogram.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoints:
        //     Array of three System.Drawing.PointF structures that define a parallelogram that
        //     determines the size and location of the drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData);
        //
        // Summary:
        //     Sends the records in a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display in a specified parallelogram.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoints:
        //     Array of three System.Drawing.Point structures that define a parallelogram that
        //     determines the size and location of the drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData);
        //
        // Summary:
        //     Sends the records of a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display in a specified rectangle.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData);
        //
        // Summary:
        //     Sends the records in a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display at a specified point.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoint:
        //     System.Drawing.PointF structure that specifies the location of the upper-left
        //     corner of the drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData);
        //
        // Summary:
        //     Sends the records of a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display in a specified rectangle.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destRect:
        //     System.Drawing.RectangleF structure that specifies the location and size of the
        //     drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData);
        //
        // Summary:
        //     Sends the records in a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display at a specified point.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoint:
        //     System.Drawing.Point structure that specifies the location of the upper-left
        //     corner of the drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   srcUnit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData);
        //
        // Summary:
        //     Sends the records in a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display in a specified parallelogram
        //     using specified image attributes.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoints:
        //     Array of three System.Drawing.PointF structures that define a parallelogram that
        //     determines the size and location of the drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   unit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies image attribute information
        //     for the drawn image.
        void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr);
        //
        // Summary:
        //     Sends the records in a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display at a specified point using specified
        //     image attributes.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoint:
        //     System.Drawing.PointF structure that specifies the location of the upper-left
        //     corner of the drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   unit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies image attribute information
        //     for the drawn image.
        void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr);
        //
        // Summary:
        //     Sends the records in a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display at a specified point using specified
        //     image attributes.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoint:
        //     System.Drawing.Point structure that specifies the location of the upper-left
        //     corner of the drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   unit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies image attribute information
        //     for the drawn image.
        void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr);
        //
        // Summary:
        //     Sends the records of a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display in a specified rectangle using
        //     specified image attributes.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destRect:
        //     System.Drawing.RectangleF structure that specifies the location and size of the
        //     drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.RectangleF structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   unit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies image attribute information
        //     for the drawn image.
        void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr);
        //
        // Summary:
        //     Sends the records of a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display in a specified rectangle using
        //     specified image attributes.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destRect:
        //     System.Drawing.Rectangle structure that specifies the location and size of the
        //     drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   unit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies image attribute information
        //     for the drawn image.
        void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr);
        //
        // Summary:
        //     Sends the records in a selected rectangle from a System.Drawing.Imaging.Metafile,
        //     one at a time, to a callback method for display in a specified parallelogram
        //     using specified image attributes.
        //
        // Parameters:
        //   metafile:
        //     System.Drawing.Imaging.Metafile to enumerate.
        //
        //   destPoints:
        //     Array of three System.Drawing.Point structures that define a parallelogram that
        //     determines the size and location of the drawn metafile.
        //
        //   srcRect:
        //     System.Drawing.Rectangle structure that specifies the portion of the metafile,
        //     relative to its upper-left corner, to draw.
        //
        //   unit:
        //     Member of the System.Drawing.GraphicsUnit enumeration that specifies the unit
        //     of measure used to determine the portion of the metafile that the rectangle specified
        //     by the srcRect parameter contains.
        //
        //   callback:
        //     System.Drawing.Graphics.EnumerateMetafileProc delegate that specifies the method
        //     to which the metafile records are sent.
        //
        //   callbackData:
        //     Internal pointer that is required, but ignored. You can pass System.IntPtr.Zero
        //     for this parameter.
        //
        //   imageAttr:
        //     System.Drawing.Imaging.ImageAttributes that specifies image attribute information
        //     for the drawn image.
        void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr);
        //
        // Summary:
        //     Updates the clip region of this System.Drawing.Graphics to exclude the area specified
        //     by a System.Drawing.Region.
        //
        // Parameters:
        //   region:
        //     System.Drawing.Region that specifies the region to exclude from the clip region.
        void ExcludeClip(Region region);
        //
        // Summary:
        //     Updates the clip region of this System.Drawing.Graphics to exclude the area specified
        //     by a System.Drawing.Rectangle structure.
        //
        // Parameters:
        //   rect:
        //     System.Drawing.Rectangle structure that specifies the rectangle to exclude from
        //     the clip region.
        void ExcludeClip(Rectangle rect);
        //
        // Summary:
        //     Fills the interior of a closed cardinal spline curve defined by an array of System.Drawing.Point
        //     structures.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   points:
        //     Array of System.Drawing.Point structures that define the spline.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-points is null.
        void FillClosedCurve(Brush brush, Point[] points);
        //
        // Summary:
        //     Fills the interior of a closed cardinal spline curve defined by an array of System.Drawing.PointF
        //     structures.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that define the spline.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-points is null.
        void FillClosedCurve(Brush brush, PointF[] points);
        //
        // Summary:
        //     Fills the interior of a closed cardinal spline curve defined by an array of System.Drawing.Point
        //     structures using the specified fill mode.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   points:
        //     Array of System.Drawing.Point structures that define the spline.
        //
        //   fillmode:
        //     Member of the System.Drawing.Drawing2D.FillMode enumeration that determines how
        //     the curve is filled.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-points is null.
        void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode);
        //
        // Summary:
        //     Fills the interior of a closed cardinal spline curve defined by an array of System.Drawing.PointF
        //     structures using the specified fill mode.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that define the spline.
        //
        //   fillmode:
        //     Member of the System.Drawing.Drawing2D.FillMode enumeration that determines how
        //     the curve is filled.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-points is null.
        void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode);
        //
        // Summary:
        //     Fills the interior of a closed cardinal spline curve defined by an array of System.Drawing.PointF
        //     structures using the specified fill mode and tension.
        //
        // Parameters:
        //   brush:
        //     A System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that define the spline.
        //
        //   fillmode:
        //     Member of the System.Drawing.Drawing2D.FillMode enumeration that determines how
        //     the curve is filled.
        //
        //   tension:
        //     Value greater than or equal to 0.0F that specifies the tension of the curve.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-points is null.
        void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode, float tension);
        //
        // Summary:
        //     Fills the interior of a closed cardinal spline curve defined by an array of System.Drawing.Point
        //     structures using the specified fill mode and tension.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   points:
        //     Array of System.Drawing.Point structures that define the spline.
        //
        //   fillmode:
        //     Member of the System.Drawing.Drawing2D.FillMode enumeration that determines how
        //     the curve is filled.
        //
        //   tension:
        //     Value greater than or equal to 0.0F that specifies the tension of the curve.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-points is null.
        void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode, float tension);
        //
        // Summary:
        //     Fills the interior of an ellipse defined by a bounding rectangle specified by
        //     a System.Drawing.RectangleF structure.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   rect:
        //     System.Drawing.RectangleF structure that represents the bounding rectangle that
        //     defines the ellipse.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.
        void FillEllipse(Brush brush, RectangleF rect);
        //
        // Summary:
        //     Fills the interior of an ellipse defined by a bounding rectangle specified by
        //     a System.Drawing.Rectangle structure.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   rect:
        //     System.Drawing.Rectangle structure that represents the bounding rectangle that
        //     defines the ellipse.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.
        void FillEllipse(Brush brush, Rectangle rect);
        //
        // Summary:
        //     Fills the interior of an ellipse defined by a bounding rectangle specified by
        //     a pair of coordinates, a width, and a height.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse.
        //
        //   width:
        //     Width of the bounding rectangle that defines the ellipse.
        //
        //   height:
        //     Height of the bounding rectangle that defines the ellipse.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.
        void FillEllipse(Brush brush, float x, float y, float width, float height);
        //
        // Summary:
        //     Fills the interior of an ellipse defined by a bounding rectangle specified by
        //     a pair of coordinates, a width, and a height.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse.
        //
        //   width:
        //     Width of the bounding rectangle that defines the ellipse.
        //
        //   height:
        //     Height of the bounding rectangle that defines the ellipse.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.
        void FillEllipse(Brush brush, int x, int y, int width, int height);
        //
        // Summary:
        //     Fills the interior of a System.Drawing.Drawing2D.GraphicsPath.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   path:
        //     System.Drawing.Drawing2D.GraphicsPath that represents the path to fill.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-path is null.
        void FillPath(Brush brush, GraphicsPath path);
        //
        // Summary:
        //     Fills the interior of a pie section defined by an ellipse specified by a System.Drawing.RectangleF
        //     structure and two radial lines.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   rect:
        //     System.Drawing.Rectangle structure that represents the bounding rectangle that
        //     defines the ellipse from which the pie section comes.
        //
        //   startAngle:
        //     Angle in degrees measured clockwise from the x-axis to the first side of the
        //     pie section.
        //
        //   sweepAngle:
        //     Angle in degrees measured clockwise from the startAngle parameter to the second
        //     side of the pie section.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.
        void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle);
        //
        // Summary:
        //     Fills the interior of a pie section defined by an ellipse specified by a pair
        //     of coordinates, a width, a height, and two radial lines.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse from which the pie section comes.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse from which the pie section comes.
        //
        //   width:
        //     Width of the bounding rectangle that defines the ellipse from which the pie section
        //     comes.
        //
        //   height:
        //     Height of the bounding rectangle that defines the ellipse from which the pie
        //     section comes.
        //
        //   startAngle:
        //     Angle in degrees measured clockwise from the x-axis to the first side of the
        //     pie section.
        //
        //   sweepAngle:
        //     Angle in degrees measured clockwise from the startAngle parameter to the second
        //     side of the pie section.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.
        void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle);
        //
        // Summary:
        //     Fills the interior of a pie section defined by an ellipse specified by a pair
        //     of coordinates, a width, a height, and two radial lines.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse from which the pie section comes.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the bounding rectangle that defines
        //     the ellipse from which the pie section comes.
        //
        //   width:
        //     Width of the bounding rectangle that defines the ellipse from which the pie section
        //     comes.
        //
        //   height:
        //     Height of the bounding rectangle that defines the ellipse from which the pie
        //     section comes.
        //
        //   startAngle:
        //     Angle in degrees measured clockwise from the x-axis to the first side of the
        //     pie section.
        //
        //   sweepAngle:
        //     Angle in degrees measured clockwise from the startAngle parameter to the second
        //     side of the pie section.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.
        void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle);
        //
        // Summary:
        //     Fills the interior of a polygon defined by an array of points specified by System.Drawing.PointF
        //     structures.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that represent the vertices of the
        //     polygon to fill.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-points is null.
        void FillPolygon(Brush brush, PointF[] points);
        //
        // Summary:
        //     Fills the interior of a polygon defined by an array of points specified by System.Drawing.Point
        //     structures.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   points:
        //     Array of System.Drawing.Point structures that represent the vertices of the polygon
        //     to fill.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-points is null.
        void FillPolygon(Brush brush, Point[] points);
        //
        // Summary:
        //     Fills the interior of a polygon defined by an array of points specified by System.Drawing.Point
        //     structures using the specified fill mode.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   points:
        //     Array of System.Drawing.Point structures that represent the vertices of the polygon
        //     to fill.
        //
        //   fillMode:
        //     Member of the System.Drawing.Drawing2D.FillMode enumeration that determines the
        //     style of the fill.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-points is null.
        void FillPolygon(Brush brush, Point[] points, FillMode fillMode);
        //
        // Summary:
        //     Fills the interior of a polygon defined by an array of points specified by System.Drawing.PointF
        //     structures using the specified fill mode.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   points:
        //     Array of System.Drawing.PointF structures that represent the vertices of the
        //     polygon to fill.
        //
        //   fillMode:
        //     Member of the System.Drawing.Drawing2D.FillMode enumeration that determines the
        //     style of the fill.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-points is null.
        void FillPolygon(Brush brush, PointF[] points, FillMode fillMode);
        //
        // Summary:
        //     Fills the interior of a rectangle specified by a System.Drawing.Rectangle structure.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   rect:
        //     System.Drawing.Rectangle structure that represents the rectangle to fill.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.
        void FillRectangle(Brush brush, Rectangle rect);
        //
        // Summary:
        //     Fills the interior of a rectangle specified by a System.Drawing.RectangleF structure.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   rect:
        //     System.Drawing.RectangleF structure that represents the rectangle to fill.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.
        void FillRectangle(Brush brush, RectangleF rect);
        //
        // Summary:
        //     Fills the interior of a rectangle specified by a pair of coordinates, a width,
        //     and a height.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the rectangle to fill.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the rectangle to fill.
        //
        //   width:
        //     Width of the rectangle to fill.
        //
        //   height:
        //     Height of the rectangle to fill.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.
        void FillRectangle(Brush brush, float x, float y, float width, float height);
        //
        // Summary:
        //     Fills the interior of a rectangle specified by a pair of coordinates, a width,
        //     and a height.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   x:
        //     The x-coordinate of the upper-left corner of the rectangle to fill.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the rectangle to fill.
        //
        //   width:
        //     Width of the rectangle to fill.
        //
        //   height:
        //     Height of the rectangle to fill.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.
        void FillRectangle(Brush brush, int x, int y, int width, int height);
        //
        // Summary:
        //     Fills the interiors of a series of rectangles specified by System.Drawing.RectangleF
        //     structures.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   rects:
        //     Array of System.Drawing.RectangleF structures that represent the rectangles to
        //     fill.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-rects is null.
        //
        //   T:System.ArgumentException:
        //     Rects is a zero-length array.
        void FillRectangles(Brush brush, RectangleF[] rects);
        //
        // Summary:
        //     Fills the interiors of a series of rectangles specified by System.Drawing.Rectangle
        //     structures.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   rects:
        //     Array of System.Drawing.Rectangle structures that represent the rectangles to
        //     fill.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-rects is null.
        //
        //   T:System.ArgumentException:
        //     rects is a zero-length array.
        void FillRectangles(Brush brush, Rectangle[] rects);
        //
        // Summary:
        //     Fills the interior of a System.Drawing.Region.
        //
        // Parameters:
        //   brush:
        //     System.Drawing.Brush that determines the characteristics of the fill.
        //
        //   region:
        //     System.Drawing.Region that represents the area to fill.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     brush is null.-or-region is null.
        void FillRegion(Brush brush, Region region);
        //
        // Summary:
        //     Forces execution of all pending graphics operations and returns immediately without
        //     waiting for the operations to finish.
        void Flush();
        //
        // Summary:
        //     Forces execution of all pending graphics operations with the method waiting or
        //     not waiting, as specified, to return before the operations finish.
        //
        // Parameters:
        //   intention:
        //     Member of the System.Drawing.Drawing2D.FlushIntention enumeration that specifies
        //     whether the method returns immediately or waits for any existing operations to
        //     finish.
        void Flush(FlushIntention intention);
        //
        // Summary:
        //     Gets the cumulative graphics context.
        //
        // Returns:
        //     An System.Object representing the cumulative graphics context.
        [EditorBrowsable(EditorBrowsableState.Never)]
        object GetContextInfo();
        //
        // Summary:
        //     Gets the handle to the device context associated with this System.Drawing.Graphics.
        //
        // Returns:
        //     Handle to the device context associated with this System.Drawing.Graphics.
        IntPtr GetHdc();
        //
        // Summary:
        //     Gets the nearest color to the specified System.Drawing.Color structure.
        //
        // Parameters:
        //   color:
        //     System.Drawing.Color structure for which to find a match.
        //
        // Returns:
        //     A System.Drawing.Color structure that represents the nearest color to the one
        //     specified with the color parameter.
        Color GetNearestColor(Color color);
        //
        // Summary:
        //     Updates the clip region of this System.Drawing.Graphics to the intersection of
        //     the current clip region and the specified System.Drawing.Rectangle structure.
        //
        // Parameters:
        //   rect:
        //     System.Drawing.Rectangle structure to intersect with the current clip region.
        void IntersectClip(Rectangle rect);
        //
        // Summary:
        //     Updates the clip region of this System.Drawing.Graphics to the intersection of
        //     the current clip region and the specified System.Drawing.RectangleF structure.
        //
        // Parameters:
        //   rect:
        //     System.Drawing.RectangleF structure to intersect with the current clip region.
        void IntersectClip(RectangleF rect);
        //
        // Summary:
        //     Updates the clip region of this System.Drawing.Graphics to the intersection of
        //     the current clip region and the specified System.Drawing.Region.
        //
        // Parameters:
        //   region:
        //     System.Drawing.Region to intersect with the current region.
        void IntersectClip(Region region);
        //
        // Summary:
        //     Indicates whether the specified System.Drawing.PointF structure is contained
        //     within the visible clip region of this System.Drawing.Graphics.
        //
        // Parameters:
        //   point:
        //     System.Drawing.PointF structure to test for visibility.
        //
        // Returns:
        //     true if the point specified by the point parameter is contained within the visible
        //     clip region of this System.Drawing.Graphics; otherwise, false.
        bool IsVisible(PointF point);
        //
        // Summary:
        //     Indicates whether the rectangle specified by a System.Drawing.RectangleF structure
        //     is contained within the visible clip region of this System.Drawing.Graphics.
        //
        // Parameters:
        //   rect:
        //     System.Drawing.RectangleF structure to test for visibility.
        //
        // Returns:
        //     true if the rectangle specified by the rect parameter is contained within the
        //     visible clip region of this System.Drawing.Graphics; otherwise, false.
        bool IsVisible(RectangleF rect);
        //
        // Summary:
        //     Indicates whether the specified System.Drawing.Point structure is contained within
        //     the visible clip region of this System.Drawing.Graphics.
        //
        // Parameters:
        //   point:
        //     System.Drawing.Point structure to test for visibility.
        //
        // Returns:
        //     true if the point specified by the point parameter is contained within the visible
        //     clip region of this System.Drawing.Graphics; otherwise, false.
        bool IsVisible(Point point);
        //
        // Summary:
        //     Indicates whether the rectangle specified by a System.Drawing.Rectangle structure
        //     is contained within the visible clip region of this System.Drawing.Graphics.
        //
        // Parameters:
        //   rect:
        //     System.Drawing.Rectangle structure to test for visibility.
        //
        // Returns:
        //     true if the rectangle specified by the rect parameter is contained within the
        //     visible clip region of this System.Drawing.Graphics; otherwise, false.
        bool IsVisible(Rectangle rect);
        //
        // Summary:
        //     Indicates whether the point specified by a pair of coordinates is contained within
        //     the visible clip region of this System.Drawing.Graphics.
        //
        // Parameters:
        //   x:
        //     The x-coordinate of the point to test for visibility.
        //
        //   y:
        //     The y-coordinate of the point to test for visibility.
        //
        // Returns:
        //     true if the point defined by the x and y parameters is contained within the visible
        //     clip region of this System.Drawing.Graphics; otherwise, false.
        bool IsVisible(int x, int y);
        //
        // Summary:
        //     Indicates whether the point specified by a pair of coordinates is contained within
        //     the visible clip region of this System.Drawing.Graphics.
        //
        // Parameters:
        //   x:
        //     The x-coordinate of the point to test for visibility.
        //
        //   y:
        //     The y-coordinate of the point to test for visibility.
        //
        // Returns:
        //     true if the point defined by the x and y parameters is contained within the visible
        //     clip region of this System.Drawing.Graphics; otherwise, false.
        bool IsVisible(float x, float y);
        //
        // Summary:
        //     Indicates whether the rectangle specified by a pair of coordinates, a width,
        //     and a height is contained within the visible clip region of this System.Drawing.Graphics.
        //
        // Parameters:
        //   x:
        //     The x-coordinate of the upper-left corner of the rectangle to test for visibility.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the rectangle to test for visibility.
        //
        //   width:
        //     Width of the rectangle to test for visibility.
        //
        //   height:
        //     Height of the rectangle to test for visibility.
        //
        // Returns:
        //     true if the rectangle defined by the x, y, width, and height parameters is contained
        //     within the visible clip region of this System.Drawing.Graphics; otherwise, false.
        bool IsVisible(int x, int y, int width, int height);
        //
        // Summary:
        //     Indicates whether the rectangle specified by a pair of coordinates, a width,
        //     and a height is contained within the visible clip region of this System.Drawing.Graphics.
        //
        // Parameters:
        //   x:
        //     The x-coordinate of the upper-left corner of the rectangle to test for visibility.
        //
        //   y:
        //     The y-coordinate of the upper-left corner of the rectangle to test for visibility.
        //
        //   width:
        //     Width of the rectangle to test for visibility.
        //
        //   height:
        //     Height of the rectangle to test for visibility.
        //
        // Returns:
        //     true if the rectangle defined by the x, y, width, and height parameters is contained
        //     within the visible clip region of this System.Drawing.Graphics; otherwise, false.
        bool IsVisible(float x, float y, float width, float height);
        //
        // Summary:
        //     Gets an array of System.Drawing.Region objects, each of which bounds a range
        //     of character positions within the specified string.
        //
        // Parameters:
        //   text:
        //     String to measure.
        //
        //   font:
        //     System.Drawing.Font that defines the text format of the string.
        //
        //   layoutRect:
        //     System.Drawing.RectangleF structure that specifies the layout rectangle for the
        //     string.
        //
        //   stringFormat:
        //     System.Drawing.StringFormat that represents formatting information, such as line
        //     spacing, for the string.
        //
        // Returns:
        //     This method returns an array of System.Drawing.Region objects, each of which
        //     bounds a range of character positions within the specified string.
        Region[] MeasureCharacterRanges(string text, Font font, RectangleF layoutRect, StringFormat stringFormat);
        //
        // Summary:
        //     Measures the specified string when drawn with the specified System.Drawing.Font.
        //
        // Parameters:
        //   text:
        //     String to measure.
        //
        //   font:
        //     System.Drawing.Font that defines the text format of the string.
        //
        // Returns:
        //     This method returns a System.Drawing.SizeF structure that represents the size,
        //     in the units specified by the System.Drawing.Graphics.PageUnit property, of the
        //     string specified by the text parameter as drawn with the font parameter.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     font is null.
        SizeF MeasureString(string text, Font font);
        //
        // Summary:
        //     Measures the specified string when drawn with the specified System.Drawing.Font.
        //
        // Parameters:
        //   text:
        //     String to measure.
        //
        //   font:
        //     System.Drawing.Font that defines the format of the string.
        //
        //   width:
        //     Maximum width of the string in pixels.
        //
        // Returns:
        //     This method returns a System.Drawing.SizeF structure that represents the size,
        //     in the units specified by the System.Drawing.Graphics.PageUnit property, of the
        //     string specified in the text parameter as drawn with the font parameter.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     font is null.
        SizeF MeasureString(string text, Font font, int width);
        //
        // Summary:
        //     Measures the specified string when drawn with the specified System.Drawing.Font
        //     within the specified layout area.
        //
        // Parameters:
        //   text:
        //     String to measure.
        //
        //   font:
        //     System.Drawing.Font defines the text format of the string.
        //
        //   layoutArea:
        //     System.Drawing.SizeF structure that specifies the maximum layout area for the
        //     text.
        //
        // Returns:
        //     This method returns a System.Drawing.SizeF structure that represents the size,
        //     in the units specified by the System.Drawing.Graphics.PageUnit property, of the
        //     string specified by the text parameter as drawn with the font parameter.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     font is null.
        SizeF MeasureString(string text, Font font, SizeF layoutArea);
        //
        // Summary:
        //     Measures the specified string when drawn with the specified System.Drawing.Font
        //     and formatted with the specified System.Drawing.StringFormat.
        //
        // Parameters:
        //   text:
        //     String to measure.
        //
        //   font:
        //     System.Drawing.Font that defines the text format of the string.
        //
        //   width:
        //     Maximum width of the string.
        //
        //   format:
        //     System.Drawing.StringFormat that represents formatting information, such as line
        //     spacing, for the string.
        //
        // Returns:
        //     This method returns a System.Drawing.SizeF structure that represents the size,
        //     in the units specified by the System.Drawing.Graphics.PageUnit property, of the
        //     string specified in the text parameter as drawn with the font parameter and the
        //     stringFormat parameter.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     font is null.
        SizeF MeasureString(string text, Font font, int width, StringFormat format);
        //
        // Summary:
        //     Measures the specified string when drawn with the specified System.Drawing.Font
        //     and formatted with the specified System.Drawing.StringFormat.
        //
        // Parameters:
        //   text:
        //     String to measure.
        //
        //   font:
        //     System.Drawing.Font defines the text format of the string.
        //
        //   layoutArea:
        //     System.Drawing.SizeF structure that specifies the maximum layout area for the
        //     text.
        //
        //   stringFormat:
        //     System.Drawing.StringFormat that represents formatting information, such as line
        //     spacing, for the string.
        //
        // Returns:
        //     This method returns a System.Drawing.SizeF structure that represents the size,
        //     in the units specified by the System.Drawing.Graphics.PageUnit property, of the
        //     string specified in the text parameter as drawn with the font parameter and the
        //     stringFormat parameter.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     font is null.
        SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat);
        //
        // Summary:
        //     Measures the specified string when drawn with the specified System.Drawing.Font
        //     and formatted with the specified System.Drawing.StringFormat.
        //
        // Parameters:
        //   text:
        //     String to measure.
        //
        //   font:
        //     System.Drawing.Font defines the text format of the string.
        //
        //   origin:
        //     System.Drawing.PointF structure that represents the upper-left corner of the
        //     string.
        //
        //   stringFormat:
        //     System.Drawing.StringFormat that represents formatting information, such as line
        //     spacing, for the string.
        //
        // Returns:
        //     This method returns a System.Drawing.SizeF structure that represents the size,
        //     in the units specified by the System.Drawing.Graphics.PageUnit property, of the
        //     string specified by the text parameter as drawn with the font parameter and the
        //     stringFormat parameter.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     font is null.
        SizeF MeasureString(string text, Font font, PointF origin, StringFormat stringFormat);
        //
        // Summary:
        //     Measures the specified string when drawn with the specified System.Drawing.Font
        //     and formatted with the specified System.Drawing.StringFormat.
        //
        // Parameters:
        //   text:
        //     String to measure.
        //
        //   font:
        //     System.Drawing.Font that defines the text format of the string.
        //
        //   layoutArea:
        //     System.Drawing.SizeF structure that specifies the maximum layout area for the
        //     text.
        //
        //   stringFormat:
        //     System.Drawing.StringFormat that represents formatting information, such as line
        //     spacing, for the string.
        //
        //   charactersFitted:
        //     Number of characters in the string.
        //
        //   linesFilled:
        //     Number of text lines in the string.
        //
        // Returns:
        //     This method returns a System.Drawing.SizeF structure that represents the size
        //     of the string, in the units specified by the System.Drawing.Graphics.PageUnit
        //     property, of the text parameter as drawn with the font parameter and the stringFormat
        //     parameter.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     font is null.
        SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat, out int charactersFitted, out int linesFilled);
        //
        // Summary:
        //     Multiplies the world transformation of this System.Drawing.Graphics and specified
        //     the System.Drawing.Drawing2D.Matrix.
        //
        // Parameters:
        //   matrix:
        //     4x4 System.Drawing.Drawing2D.Matrix that multiplies the world transformation.
        void MultiplyTransform(Matrix matrix);
        //
        // Summary:
        //     Multiplies the world transformation of this System.Drawing.Graphics and specified
        //     the System.Drawing.Drawing2D.Matrix in the specified order.
        //
        // Parameters:
        //   matrix:
        //     4x4 System.Drawing.Drawing2D.Matrix that multiplies the world transformation.
        //
        //   order:
        //     Member of the System.Drawing.Drawing2D.MatrixOrder enumeration that determines
        //     the order of the multiplication.
        void MultiplyTransform(Matrix matrix, MatrixOrder order);
        //
        // Summary:
        //     Releases a device context handle obtained by a previous call to the System.Drawing.Graphics.GetHdc
        //     method of this System.Drawing.Graphics.
        void ReleaseHdc();
        //
        // Summary:
        //     Releases a device context handle obtained by a previous call to the System.Drawing.Graphics.GetHdc
        //     method of this System.Drawing.Graphics.
        //
        // Parameters:
        //   hdc:
        //     Handle to a device context obtained by a previous call to the System.Drawing.Graphics.GetHdc
        //     method of this System.Drawing.Graphics.
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        void ReleaseHdc(IntPtr hdc);
        //
        // Summary:
        //     Releases a handle to a device context.
        //
        // Parameters:
        //   hdc:
        //     Handle to a device context.
        [EditorBrowsable(EditorBrowsableState.Never)]
        void ReleaseHdcInternal(IntPtr hdc);
        //
        // Summary:
        //     Resets the clip region of this System.Drawing.Graphics to an infinite region.
        void ResetClip();
        //
        // Summary:
        //     Resets the world transformation matrix of this System.Drawing.Graphics to the
        //     identity matrix.
        void ResetTransform();
        //
        // Summary:
        //     Restores the state of this System.Drawing.Graphics to the state represented by
        //     a System.Drawing.Drawing2D.GraphicsState.
        //
        // Parameters:
        //   gstate:
        //     System.Drawing.Drawing2D.GraphicsState that represents the state to which to
        //     restore this System.Drawing.Graphics.
        void Restore(GraphicsState gstate);
        //
        // Summary:
        //     Applies the specified rotation to the transformation matrix of this System.Drawing.Graphics.
        //
        // Parameters:
        //   angle:
        //     Angle of rotation in degrees.
        void RotateTransform(float angle);
        //
        // Summary:
        //     Applies the specified rotation to the transformation matrix of this System.Drawing.Graphics
        //     in the specified order.
        //
        // Parameters:
        //   angle:
        //     Angle of rotation in degrees.
        //
        //   order:
        //     Member of the System.Drawing.Drawing2D.MatrixOrder enumeration that specifies
        //     whether the rotation is appended or prepended to the matrix transformation.
        void RotateTransform(float angle, MatrixOrder order);
        //
        // Summary:
        //     Saves the current state of this System.Drawing.Graphics and identifies the saved
        //     state with a System.Drawing.Drawing2D.GraphicsState.
        //
        // Returns:
        //     This method returns a System.Drawing.Drawing2D.GraphicsState that represents
        //     the saved state of this System.Drawing.Graphics.
        GraphicsState Save();
        //
        // Summary:
        //     Applies the specified scaling operation to the transformation matrix of this
        //     System.Drawing.Graphics by prepending it to the object's transformation matrix.
        //
        // Parameters:
        //   sx:
        //     Scale factor in the x direction.
        //
        //   sy:
        //     Scale factor in the y direction.
        void ScaleTransform(float sx, float sy);
        //
        // Summary:
        //     Applies the specified scaling operation to the transformation matrix of this
        //     System.Drawing.Graphics in the specified order.
        //
        // Parameters:
        //   sx:
        //     Scale factor in the x direction.
        //
        //   sy:
        //     Scale factor in the y direction.
        //
        //   order:
        //     Member of the System.Drawing.Drawing2D.MatrixOrder enumeration that specifies
        //     whether the scaling operation is prepended or appended to the transformation
        //     matrix.
        void ScaleTransform(float sx, float sy, MatrixOrder order);
        //
        // Summary:
        //     Sets the clipping region of this System.Drawing.Graphics to the Clip property
        //     of the specified System.Drawing.Graphics.
        //
        // Parameters:
        //   g:
        //     System.Drawing.Graphics from which to take the new clip region.
        void SetClip(Graphics g);
        //
        // Summary:
        //     Sets the clipping region of this System.Drawing.Graphics to the rectangle specified
        //     by a System.Drawing.Rectangle structure.
        //
        // Parameters:
        //   rect:
        //     System.Drawing.Rectangle structure that represents the new clip region.
        void SetClip(Rectangle rect);
        //
        // Summary:
        //     Sets the clipping region of this System.Drawing.Graphics to the rectangle specified
        //     by a System.Drawing.RectangleF structure.
        //
        // Parameters:
        //   rect:
        //     System.Drawing.RectangleF structure that represents the new clip region.
        void SetClip(RectangleF rect);
        //
        // Summary:
        //     Sets the clipping region of this System.Drawing.Graphics to the specified System.Drawing.Drawing2D.GraphicsPath.
        //
        // Parameters:
        //   path:
        //     System.Drawing.Drawing2D.GraphicsPath that represents the new clip region.
        void SetClip(GraphicsPath path);
        //
        // Summary:
        //     Sets the clipping region of this System.Drawing.Graphics to the result of the
        //     specified combining operation of the current clip region and the System.Drawing.Graphics.Clip
        //     property of the specified System.Drawing.Graphics.
        //
        // Parameters:
        //   g:
        //     System.Drawing.Graphics that specifies the clip region to combine.
        //
        //   combineMode:
        //     Member of the System.Drawing.Drawing2D.CombineMode enumeration that specifies
        //     the combining operation to use.
        void SetClip(Graphics g, CombineMode combineMode);
        //
        // Summary:
        //     Sets the clipping region of this System.Drawing.Graphics to the result of the
        //     specified operation combining the current clip region and the specified System.Drawing.Region.
        //
        // Parameters:
        //   region:
        //     System.Drawing.Region to combine.
        //
        //   combineMode:
        //     Member from the System.Drawing.Drawing2D.CombineMode enumeration that specifies
        //     the combining operation to use.
        void SetClip(Region region, CombineMode combineMode);
        //
        // Summary:
        //     Sets the clipping region of this System.Drawing.Graphics to the result of the
        //     specified operation combining the current clip region and the specified System.Drawing.Drawing2D.GraphicsPath.
        //
        // Parameters:
        //   path:
        //     System.Drawing.Drawing2D.GraphicsPath to combine.
        //
        //   combineMode:
        //     Member of the System.Drawing.Drawing2D.CombineMode enumeration that specifies
        //     the combining operation to use.
        void SetClip(GraphicsPath path, CombineMode combineMode);
        //
        // Summary:
        //     Sets the clipping region of this System.Drawing.Graphics to the result of the
        //     specified operation combining the current clip region and the rectangle specified
        //     by a System.Drawing.RectangleF structure.
        //
        // Parameters:
        //   rect:
        //     System.Drawing.RectangleF structure to combine.
        //
        //   combineMode:
        //     Member of the System.Drawing.Drawing2D.CombineMode enumeration that specifies
        //     the combining operation to use.
        void SetClip(RectangleF rect, CombineMode combineMode);
        //
        // Summary:
        //     Sets the clipping region of this System.Drawing.Graphics to the result of the
        //     specified operation combining the current clip region and the rectangle specified
        //     by a System.Drawing.Rectangle structure.
        //
        // Parameters:
        //   rect:
        //     System.Drawing.Rectangle structure to combine.
        //
        //   combineMode:
        //     Member of the System.Drawing.Drawing2D.CombineMode enumeration that specifies
        //     the combining operation to use.
        void SetClip(Rectangle rect, CombineMode combineMode);
        //
        // Summary:
        //     Transforms an array of points from one coordinate space to another using the
        //     current world and page transformations of this System.Drawing.Graphics.
        //
        // Parameters:
        //   destSpace:
        //     Member of the System.Drawing.Drawing2D.CoordinateSpace enumeration that specifies
        //     the destination coordinate space.
        //
        //   srcSpace:
        //     Member of the System.Drawing.Drawing2D.CoordinateSpace enumeration that specifies
        //     the source coordinate space.
        //
        //   pts:
        //     Array of System.Drawing.PointF structures that represent the points to transform.
        void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts);
        //
        // Summary:
        //     Transforms an array of points from one coordinate space to another using the
        //     current world and page transformations of this System.Drawing.Graphics.
        //
        // Parameters:
        //   destSpace:
        //     Member of the System.Drawing.Drawing2D.CoordinateSpace enumeration that specifies
        //     the destination coordinate space.
        //
        //   srcSpace:
        //     Member of the System.Drawing.Drawing2D.CoordinateSpace enumeration that specifies
        //     the source coordinate space.
        //
        //   pts:
        //     Array of System.Drawing.Point structures that represents the points to transformation.
        void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, Point[] pts);
        //
        // Summary:
        //     Translates the clipping region of this System.Drawing.Graphics by specified amounts
        //     in the horizontal and vertical directions.
        //
        // Parameters:
        //   dx:
        //     The x-coordinate of the translation.
        //
        //   dy:
        //     The y-coordinate of the translation.
        void TranslateClip(int dx, int dy);
        //
        // Summary:
        //     Translates the clipping region of this System.Drawing.Graphics by specified amounts
        //     in the horizontal and vertical directions.
        //
        // Parameters:
        //   dx:
        //     The x-coordinate of the translation.
        //
        //   dy:
        //     The y-coordinate of the translation.
        void TranslateClip(float dx, float dy);
        //
        // Summary:
        //     Changes the origin of the coordinate system by prepending the specified translation
        //     to the transformation matrix of this System.Drawing.Graphics.
        //
        // Parameters:
        //   dx:
        //     The x-coordinate of the translation.
        //
        //   dy:
        //     The y-coordinate of the translation.
        void TranslateTransform(float dx, float dy);
        //
        // Summary:
        //     Changes the origin of the coordinate system by applying the specified translation
        //     to the transformation matrix of this System.Drawing.Graphics in the specified
        //     order.
        //
        // Parameters:
        //   dx:
        //     The x-coordinate of the translation.
        //
        //   dy:
        //     The y-coordinate of the translation.
        //
        //   order:
        //     Member of the System.Drawing.Drawing2D.MatrixOrder enumeration that specifies
        //     whether the translation is prepended or appended to the transformation matrix.
        void TranslateTransform(float dx, float dy, MatrixOrder order);
    }
}
