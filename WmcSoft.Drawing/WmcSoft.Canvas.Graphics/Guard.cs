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

using System;

namespace WmcSoft.Canvas
{
    internal static class Guard
    {
        public static bool IsPositive(float value) {
            return value > 0f && !Single.IsPositiveInfinity(value); // NaN and -oo are smaller than 0.
        }
        public static void Positive(float value, string paramName = "value") {
            if (!IsPositive(value))
                throw new ArgumentOutOfRangeException(paramName);
        }

        public static bool IsFinite(float value) {
            return !Single.IsNaN(value) && !Single.IsInfinity(value);
        }
        public static void Finite(float value, string paramName = "value") {
            if (!IsFinite(value))
                throw new ArgumentOutOfRangeException(paramName);
        }

        public static bool IsAlphaValue(float value) {
            return value >= 0f && value <= 1f;
        }
        public static void AlphaValue(float value, string paramName = "value") {
            if (!IsAlphaValue(value))
                throw new ArgumentOutOfRangeException(paramName);
        }

    }
}
