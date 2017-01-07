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

namespace WmcSoft.Drawing.Abstractions
{
    /// <summary>Specifies the direction of a linear gradient.</summary>
    public enum LinearGradientMode
    {
        /// <summary>Specifies a gradient from left to right.</summary>
        Horizontal = 0,

        /// <summary>Specifies a gradient from top to bottom.</summary>
        Vertical = 1,

        /// <summary>Specifies a gradient from upper left to lower right.</summary>
        ForwardDiagonal = 2,

        /// <summary>Specifies a gradient from upper right to lower left.</summary>
        BackwardDiagonal = 3,
    }
}
