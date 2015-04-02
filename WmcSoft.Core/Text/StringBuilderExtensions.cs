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

using System.Text;

namespace WmcSoft.Text
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder Prepend(this StringBuilder sb, bool value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, byte value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, char value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, char[] value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, decimal value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, double value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, float value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, int value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, long value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, object value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, sbyte value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, short value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, uint value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, ulong value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, ushort value) {
            return sb.Insert(0, value);
        }

        public static StringBuilder Prepend(this StringBuilder sb, char[] value, int startIndex, int charCount) {
            return sb.Insert(0, value, startIndex, charCount);
        }

        public static StringBuilder Prepend(this StringBuilder sb, string value, int count) {
            return sb.Insert(0, value, count);
        }
    }
}
