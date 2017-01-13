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
    /// Line caps/joins & dashed lines.
    /// </summary>
    public interface ICanvasPathDrawingStyles<T>
    {
        #region line caps/joins

        /// <summary>
        /// The line width.
        /// </summary>
        /// <remarks>The default value is 1.</remarks>
        T LineWidth { get; set; }

        /// <summary>
        /// The line cap style.
        /// </summary>
        /// <remarks>The default value is <see cref="CanvasLineCap.Butt"/>.</remarks>
        CanvasLineCap LineCap { get; set; }

        /// <summary>
        /// The line join style.
        /// </summary>
        /// <remarks>The default value is <see cref="CanvasLineJoin.Miter"/>.</remarks>
        CanvasLineJoin LineJoin { get; set; }

        /// <summary>
        /// The miter limit ratio.
        /// </summary>
        /// <<remarks>The default value is 10.</remarks>
        T MiterLimit { get; set; }

        #endregion

        #region dashed lines

        /// <summary>
        /// The distances for which to alternately have the line on and the line off.
        /// </summary>
        /// <remarks>The length of the array is even. On settings, odd arrays are repeated once.</remarks>
        T[] LineDash { get; set; }

        /// <summary>
        /// The phase offset, in the same units as the line dash pattern.
        /// </summary>
        T LineDashOffset { get; set; }

        #endregion
    };
}