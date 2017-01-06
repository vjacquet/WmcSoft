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

 ****************************************************************************
 * Based on <https://html.spec.whatwg.org/multipage/scripting.html#2dcontext>
 ****************************************************************************/

#endregion

using System;

namespace WmcSoft.Canvas
{
    /// <summary>
    /// Colors and styles (see also the CanvasPathDrawingStyles and CanvasTextDrawingStyles interfaces)
    /// </summary>
    public interface ICanvasFillStrokeStyles<T, TColor>
    {
        Variant<TColor, CanvasGradient<T, TColor>, CanvasPattern> StrokeStyle { get; set; } // (default black)
        Variant<TColor, CanvasGradient<T, TColor>, CanvasPattern> FillStyle { get; set; }  // (default black)
        CanvasGradient<T, TColor> CreateLinearGradient(T x0, T y0, T x1, T y1);
        CanvasGradient<T, TColor> CreateRadialGradient(T x0, T y0, T r0, T x1, T y1, T r1);
        CanvasPattern CreatePattern(ICanvasImageSource image, Repetition repetition = Repetition.NoRepeat);
    }

    public enum Repetition
    {
        Repeat,
        RepeatX,
        RepeatY,
        NoRepeat
    }
}