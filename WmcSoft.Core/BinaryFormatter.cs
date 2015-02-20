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
    public class BinaryFormatter : IFormatProvider, ICustomFormatter
    {
        static readonly Regex regex;
        static BinaryFormatter() {
            regex = new Regex(@"^(?<format>[xBOX])(?<group>\d+)?", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        }

        #region IFormatProvider Members

        public object GetFormat(Type formatType) {
            if (formatType == typeof(ICustomFormatter))
                return this;
            return null;
        }

        #endregion

        #region ICustomFormatter Members

        private static byte[] ToBytes(object arg) {
            switch (Convert.GetTypeCode(arg)) {
            case TypeCode.SByte:
                return new byte[1] { Byte.Parse(((sbyte)arg).ToString("X2"), NumberStyles.HexNumber) };
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
            default:
                break;
            }

            if (arg is byte[]) {
                return (byte[])arg;
            }
            if (arg is BigInteger) {
                return ((BigInteger)arg).ToByteArray();
            }
            return null;
        }

        private string DoFormat(string format, int? group, object arg) {
            byte[] bytes = ToBytes(arg);

            int groupSize = 4;
            int padding = 2;
            Func<byte, string> converter;

            switch (format) {
            case "B":
                groupSize = group ?? 8;
                padding = 8;
                converter = (b) => Convert.ToString(b, 2);
                break;
            case "O":
                groupSize = group ?? 4;
                padding = 4;
                converter = (b) => Convert.ToString(b, 8);
                break;
            case "x":
            case "X":
                groupSize = group ?? 4;
                converter = (b) => b.ToString(format);
                break;
            default:
                return null; // should not happen
            }

            var sb = new StringBuilder();
            var lo = bytes.GetLowerBound(0);
            var hi = bytes.GetUpperBound(0);

            if (groupSize == 0) {
                for (int i = hi; i >= lo; i--) {
                    var s = converter(bytes[i]);
                    sb.Prepend(s).Prepend(new String('0', padding - s.Length));
                }
            } else {
                var count = 0;
                for (int i = hi; i >= lo; i--) {
                    var s = converter(bytes[i]);
                    sb.Prepend(s).Prepend(new String('0', padding - s.Length));

                    if ((++count % groupSize == 0) && i != lo) {
                        sb.Prepend(' ');
                    }
                }
            }

            return sb.ToString();
        }

        public string Format(string format, object arg, IFormatProvider formatProvider) {
            try {
                string result;
                Match match;

                if (format == null || format == "G") {
                    result = DoFormat("X", 4, arg);
                } else if (regex.TryMatch(format, out match)) {
                    result = DoFormat(match.GetGroupValue("format"), match.GetGroupValue<int>("group"), arg);
                } else {
                    result = null;
                }

                return result ?? FormatProviderHelper.HandleOtherFormats(format, arg);
            }
            catch (FormatException e) {
                throw new FormatException(Resources.InvalidFormatMessage.FormatWith(format), e);
            }
        }

        #endregion
    }
}
