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

using System.Collections.Generic;

namespace WmcSoft.Canvas
{
    /// <summary>
    /// Line caps/joins & dashed lines.
    /// </summary>
    public interface ICanvasPathDrawingStyles<T>
    {
        // line caps/joins
        T LineWidth { get; set; } // (default 1)
        CanvasLineCap LineCap { get; set; } // (default "butt")
        CanvasLineJoin LineJoin { get; set; } // (default "miter")
        T MiterLimit { get; set; } // (default 10)

        // dashed lines
        T[] LineDash { get; set; } // default empty
        T LineDashOffset { get; set; }
    };
}