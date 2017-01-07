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
    /// <summary>Specifies the available cap styles with which a <see cref=".Pen" /> object can end a line.</summary>
    public enum LineCap
    {
        /// <summary>Specifies a flat line cap.</summary>
        Flat = 0,

        /// <summary>Specifies a square line cap.</summary>
        Square = 1,

        /// <summary>Specifies a round line cap.</summary>
        Round = 2,

        /// <summary>Specifies a triangular line cap.</summary>
        Triangle = 3,

        /// <summary>Specifies no anchor.</summary>
        NoAnchor = 16,

        /// <summary>Specifies a square anchor line cap.</summary>
        SquareAnchor = 17,

        /// <summary>Specifies a round anchor cap.</summary>
        RoundAnchor = 18,

        /// <summary>Specifies a diamond anchor cap.</summary>
        DiamondAnchor = 19,

        /// <summary>Specifies an arrow-shaped anchor cap.</summary>
        ArrowAnchor = 20,

        /// <summary>Specifies a custom line cap.</summary>
        Custom = 255,

        /// <summary>Specifies a mask used to check whether a line cap is an anchor cap.</summary>
        AnchorMask = 240,
    }
}
