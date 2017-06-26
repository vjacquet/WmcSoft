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
    public interface ICanvasDrawPath<T> : ICanvasPath<T>
    {
        void BeginPath();
        void Fill(CanvasFillRule fillRule);
        void Fill(Path2D<T> path, CanvasFillRule fillRule);
        void Stroke();
        void Stroke(Path2D<T> path);
        void Clip(CanvasFillRule fillRule);
        void Clip(Path2D<T> path, CanvasFillRule fillRule);
        void ResetClip();
        bool IsPointInPath(T x, T y, CanvasFillRule fillRule);
        bool IsPointInPath(Path2D<T> path, T x, T y, CanvasFillRule fillRule);
        bool IsPointInStroke(T x, T y);
        bool IsPointInStroke(Path2D<T> path, T x, T y);
    }

    public static class CanvasDrawPathExtensions
    {
        public static void Fill<T>(this ICanvasDrawPath<T> canvas)
        {
            canvas.Fill(CanvasFillRule.NonZero);
        }

        public static void Fill<T>(this ICanvasDrawPath<T> canvas, Path2D<T> path)
        {
            canvas.Fill(CanvasFillRule.NonZero);
        }

        public static void Clip<T>(this ICanvasDrawPath<T> canvas)
        {
            canvas.Clip(CanvasFillRule.NonZero);
        }

        public static void Clip<T>(this ICanvasDrawPath<T> canvas, Path2D<T> path)
        {
            canvas.Clip(path, CanvasFillRule.NonZero);
        }

        public static bool IsPointInPath<T>(this ICanvasDrawPath<T> canvas, T x, T y)
        {
            return canvas.IsPointInPath(x, y, CanvasFillRule.NonZero);
        }

        public static bool IsPointInPath<T>(this ICanvasDrawPath<T> canvas, Path2D<T> path, T x, T y)
        {
            return canvas.IsPointInPath(path, x, y, CanvasFillRule.NonZero);
        }
    }
}