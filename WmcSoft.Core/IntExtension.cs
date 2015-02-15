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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace WmcSoft
{
    public static class IntExtension
    {
        public static bool AllFlagsSet<T>(this T flags, T flagsToTest) where T : IConvertible {
            IFormatProvider provider = CultureInfo.CurrentCulture;
            return AllFlagsSet(flags.ToInt64(provider), flagsToTest.ToInt64(provider));
        }

        public static bool AnyFlagsSet<T>(this T flags, T flagsToTest) where T : IConvertible {
            IFormatProvider provider = CultureInfo.CurrentCulture;
            return AnyFlagsSet(flags.ToInt64(provider), flagsToTest.ToInt64(provider));
        }

        public static bool AllFlagsSet(this Int16 flags, Int16 flagsToTest) {
            return (flags & flagsToTest) == flagsToTest;
        }

        public static bool AnyFlagsSet(this Int16 flags, Int16 flagsToTest) {
            return (flags & flagsToTest) != 0;
        }

        public static bool AllFlagsSet(this Int32 flags, Int32 flagsToTest) {
            return (flags & flagsToTest) == flagsToTest;
        }

        public static bool AnyFlagsSet(this Int32 flags, Int32 flagsToTest) {
            return (flags & flagsToTest) != 0;
        }

        public static bool AllFlagsSet(this Int64 flags, Int64 flagsToTest) {
            return (flags & flagsToTest) == flagsToTest;
        }

        public static bool AnyFlagsSet(this Int64 flags, Int64 flagsToTest) {
            return (flags & flagsToTest) != 0;
        }

        public static string ToHexaString(this byte[] buffer) {
            StringBuilder sb = new StringBuilder(2 * buffer.Length);
            foreach (byte b in buffer) {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
