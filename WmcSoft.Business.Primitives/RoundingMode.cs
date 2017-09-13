#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft
{
    public enum RoundingMode
    {
        /// <summary>Rounding mode to round towards positive infinity.</summary>
        Ceiling,
        /// <summary>Rounding mode to round towards zero.</summary>
        Down,
        /// <summary>Rounding mode to round towards negative infinity.</summary>
        Floor,
        /// <summary>Rounding mode to round towards "nearest neighbor" unless both neighbors are equidistant, in which case round down.</summary>
        HalfDown,
        /// <summary>Rounding mode to round towards the "nearest neighbor" unless both neighbors are equidistant, in which case, round towards the even neighbor.</summary>
        HalfEven,
        /// <summary>Rounding mode to round towards "nearest neighbor" unless both neighbors are equidistant, in which case round up.</summary>
        HalfUp,
        /// <summary>Rounding mode to assert that the requested operation has an exact result, hence no rounding is necessary.</summary>
        Unnecessary,
        /// <summary>Rounding mode to round away from zero.</summary>
        Up,
    }
}
