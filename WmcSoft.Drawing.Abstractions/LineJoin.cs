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
    /// <summary>Specifies how to join consecutive line or curve segments in a figure (subpath) contained in a <see cref=".GraphicsPath" /> object.</summary>
    public enum LineJoin
    {
        /// <summary>Specifies a mitered join. This produces a sharp corner or a clipped corner, depending on whether the length of the miter exceeds the miter limit.</summary>
        Miter = 0,

        /// <summary>Specifies a beveled join. This produces a diagonal corner.</summary>
        Bevel = 1,

        /// <summary>Specifies a circular join. This produces a smooth, circular arc between the lines.</summary>
        Round = 2,

        /// <summary>Specifies a mitered join. This produces a sharp corner or a beveled corner, depending on whether the length of the miter exceeds the miter limit.</summary>
        MiterClipped = 3,
    }
}
