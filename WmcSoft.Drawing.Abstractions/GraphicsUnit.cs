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
    /// <summary>Specifies the unit of measure for the given data.</summary>
    public enum GraphicsUnit
    {
        /// <summary>Specifies the world coordinate system unit as the unit of measure.</summary>
        World = 0,

        /// <summary>Specifies the unit of measure of the display device. Typically pixels for video displays, and 1/100 inch for printers.</summary>
        Display = 1,

        /// <summary>Specifies a device pixel as the unit of measure.</summary>
        Pixel = 2,

        /// <summary>Specifies a printer's point (1/72 inch) as the unit of measure.</summary>
        Point = 3,

        /// <summary>Specifies the inch as the unit of measure.</summary>
        Inch = 4,

        /// <summary>Specifies the document unit (1/300 inch) as the unit of measure.</summary>
        Document = 5,

        /// <summary>Specifies the millimeter as the unit of measure.</summary>
        Millimeter = 6,
    }
}
