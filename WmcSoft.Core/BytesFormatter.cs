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
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using WmcSoft.Properties;
using WmcSoft.Text;
using WmcSoft.Text.RegularExpressions;

namespace WmcSoft
{
    /// <summary>
    /// Utility to format byte or array of bytes.
    /// </summary>
    public class BytesFormatter : IFormatProvider, ICustomFormatter
    {
        static BytesFormatter() { } // for lazyness.

        /// <summary>
        /// Returns an object that provides formatting services for the specified type.
        /// </summary>
        /// <param name="formatType">An object that specifies the type of format object to return.</param>
        /// <returns>An instance of the object specified by <paramref name="formatType"/>, if the <see cref="IFormatProvider"/>
        ///    implementation can supply that type of object; otherwise, <c>null</c>.</returns>
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            return null;
        }

        /// <summary>
        ///  Converts the value of a specified object to an equivalent string representation
        ///  using specified format and culture-specific formatting information.
        /// </summary>
        /// <param name="format">A format string containing formatting specifications.</param>
        /// <param name="arg">An object to format.</param>
        /// <param name="formatProvider">An object that supplies format information about the current instance.</param>
        /// <returns>The string representation of the value of arg, formatted as specified by format
        /// and formatProvider.</returns>
        /// <exception cref="FormatException"><paramref name="format"/> is not valid format.</exception>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            try {
                return DoFormat(format, arg) ?? FormatProviderHelper.HandleOtherFormats(format, arg);
            } catch (FormatException) {
                throw new FormatException(string.Format(Resources.InvalidFormatMessage, format));
            }
        }

        private static readonly Regex regex = new Regex(@"^(?<format>[xBOX])(?<group>\d+)?", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static string DoFormat(string format, object arg)
        {
            if (format == null || format == "G")
                return DoFormat("X", 4, arg);

            if (regex.TryMatch(format, out var match))
                return DoFormat(match.GetGroupValue("format"), match.GetNullableGroupValue<int>("group"), arg);

            return null;
        }

        private static string DoFormat(string format, int? group, object arg)
        {
            var bytes = ToBytes(arg);

            switch (format) {
            case "B":
                return Format(bytes, b => Convert.ToString(b, 2), 8, group ?? 1);
            case "O":
                return Format(bytes, b => Convert.ToString(b, 8), 4, group ?? 4);
            case "x":
            case "X":
            default:
                return Format(bytes, b => b.ToString(format), 2, group ?? 4);
            }
        }

        private static byte[] ToBytes(object arg)
        {
            switch (Convert.GetTypeCode(arg)) {
            case TypeCode.SByte:
                return new byte[1] { byte.Parse(((sbyte)arg).ToString("X2"), NumberStyles.HexNumber) };
            case TypeCode.Byte:
                return new byte[1] { (byte)arg };
            case TypeCode.Int16:
                return BitConverter.GetBytes((short)arg);
            case TypeCode.Int32:
                return BitConverter.GetBytes((int)arg);
            case TypeCode.Int64:
                return BitConverter.GetBytes((long)arg);
            case TypeCode.UInt16:
                return BitConverter.GetBytes((ushort)arg);
            case TypeCode.UInt32:
                return BitConverter.GetBytes((uint)arg);
            case TypeCode.UInt64:
                return BitConverter.GetBytes((ulong)arg);
            }

            switch (arg) {
            case byte[] array:
                return array;
            case BigInteger integer:
                return integer.ToByteArray();
            default:
                return null;
            }
        }

        private static string Format(Func<int, string> reader, int lo, int hi, int groupSize)
        {
            var sb = new StringBuilder();

            if (groupSize != 0) {
                var count = 0;
                for (int i = hi; i > lo; i--) {
                    sb.Prepend(reader(i));

                    if (++count % groupSize == 0)
                        sb.Prepend(' ');
                }
                sb.Prepend(reader(lo));
            } else {
                for (int i = hi; i >= lo; i--) {
                    sb.Prepend(reader(i));
                }
            }

            return sb.ToString();
        }

        private static string Format(byte[] bytes, Func<byte, string> converter, int padding, int groupSize)
        {
            var lo = bytes.GetLowerBound(0);
            var hi = bytes.GetUpperBound(0);
            return Format(i => converter(bytes[i]).PadLeft(padding, '0'), lo, hi, groupSize);
        }
    }
}
