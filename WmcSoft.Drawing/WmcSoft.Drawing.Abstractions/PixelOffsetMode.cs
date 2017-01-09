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
    /// <summary>Specifies how pixels are offset during rendering.</summary>
    public enum PixelOffsetMode
    {
        /// <summary>Specifies an invalid mode.</summary>
        Invalid = -1,

        /// <summary>Specifies the default mode.</summary>
        Default = 0,

        /// <summary>Specifies high speed, low quality rendering.</summary>
        HighSpeed = 1,

        /// <summary>Specifies high quality, low speed rendering.</summary>
        HighQuality = 2,

        /// <summary>Specifies no pixel offset.</summary>
        None = 3,

        /// <summary>Specifies that pixels are offset by -.5 units, both horizontally and vertically, for high speed antialiasing.</summary>
        Half = 4,
    }
}