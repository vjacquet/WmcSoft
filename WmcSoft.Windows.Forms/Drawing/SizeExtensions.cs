﻿#region Licence

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

using System;
using System.Drawing;

namespace WmcSoft.Drawing
{
    /// <summary>
    /// Defines the extension methods to the <see cref="Size"/> struct. This is a static class.
    /// </summary>
    public static class SizeExtensions
    {
        public static Size Combine(Size x, Size y) {
            var w = Math.Max(x.Width, y.Width);
            var h = Math.Max(x.Height, y.Height);
            return new Size(w, h);
        }

        public static Size CombineWith(this Size x, Size y) {
            return Combine(x, y);
        }
    }
}
