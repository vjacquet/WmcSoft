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
    /// <summary>Specifies the quality of text rendering.</summary>
    public enum TextRenderingHint
    {
        /// <summary>Each character is drawn using its glyph bitmap, with the system default rendering hint. The text will be drawn using whatever font-smoothing settings the user has selected for the system.</summary>
        SystemDefault = 0,

        /// <summary>Each character is drawn using its glyph bitmap. Hinting is used to improve character appearance on stems and curvature.</summary>
        SingleBitPerPixelGridFit = 1,

        /// <summary>Each character is drawn using its glyph bitmap. Hinting is not used.</summary>
        SingleBitPerPixel = 2,

        /// <summary>Each character is drawn using its antialiased glyph bitmap with hinting. Much better quality due to antialiasing, but at a higher performance cost.</summary>
        AntiAliasGridFit = 3,

        /// <summary>Each character is drawn using its antialiased glyph bitmap without hinting. Better quality due to antialiasing. Stem width differences may be noticeable because hinting is turned off.</summary>
        AntiAlias = 4,

        /// <summary>Each character is drawn using its glyph ClearType bitmap with hinting. The highest quality setting. Used to take advantage of ClearType font features.</summary>
        ClearTypeGridFit = 5,
    }
}
