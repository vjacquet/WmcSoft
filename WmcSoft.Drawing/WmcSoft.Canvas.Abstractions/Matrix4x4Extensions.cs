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

 ****************************************************************************
 * Based on <https://html.spec.whatwg.org/multipage/scripting.html#2dcontext>
 ****************************************************************************/

#endregion

using System;
using System.Numerics;
using static System.Math;

namespace WmcSoft.Canvas
{
    public static class Matrix4x4Extensions
    {
        public static Matrix4x4 Scale(this Matrix4x4 matrix, float x, float y)
        {
            var m = new Matrix4x4(x, 0, 0, 0, 0, y, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
            return matrix * m;
        }

        public static Matrix4x4 Rotate(this Matrix4x4 matrix, float angle)
        {
            var half = angle / 2;
            var sin = Sin(half);
            var sc = sin * Cos(half);
            var sq = sin * sin;
            throw new NotImplementedException();
        }

        public static Matrix4x4 Translate(this Matrix4x4 matrix, float x, float y)
        {
            var m = new Matrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, x, y, 0, 1);
            return matrix * m;
        }

        public static Matrix4x4 Transform(this Matrix4x4 matrix, float a, float b, float c, float d, float e, float f)
        {
            var m = new Matrix4x4(a, b, 0, 0, c, d, 0, 0, 0, 0, 1, 0, e, f, 0, 1);
            return matrix * m;
        }
    }
}
