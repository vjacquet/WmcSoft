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
using System.Globalization;

namespace WmcSoft
{
    struct GeoFormatter
    {
        private readonly char _specifier;
        private readonly string _precision;
        private readonly IFormatProvider _formatProvider;

        public GeoFormatter(string format, IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider ?? CultureInfo.CurrentCulture;

            int precision;
            ParseFormat(format ?? "G", _formatProvider, out _specifier, out precision);
            switch (_specifier) {
            case 'M':
            case 'G':
                _precision = "00." + new string('0', precision);
                break;
            default:
                _precision = "0." + new string('0', precision);
                break;
            }
        }

        static void ParseFormat(string format, IFormatProvider formatProvider, out char specifier, out int precision)
        {
            if (string.IsNullOrWhiteSpace(format))
                throw new FormatException();

            specifier = format[0];
            if (!specifier.Any('M', 'D', 'G'))
                throw new FormatException();

            if (format.Length > 1) {
                format = format.Substring(1);
                if (!int.TryParse(format, NumberStyles.Integer, formatProvider, out precision))
                    throw new FormatException();

                if (precision < 0 | precision > 99)
                    throw new FormatException();
            } else {
                switch (specifier) {
                case 'M':
                    precision = 2;
                    break;
                case 'D':
                    precision = 4;
                    break;
                default:
                    precision = 1;
                    break;
                }
            }
        }

        string Format(int degrees, decimal minutes)
        {
            return degrees + "° " + minutes.ToString(_precision, _formatProvider) + "'";
        }

        string Format(decimal degrees)
        {
            return degrees.ToString(_precision, _formatProvider) + '°';
        }

        public string Format(int degrees, int minutes, int seconds)
        {
            switch (_specifier) {
            case 'M': // 49° 30,25′
                return Format(degrees, minutes + seconds / 60m);
            case 'D': // 49,5000°
                return Format(degrees > 0 ? (degrees + minutes / 60m + seconds / 3600m) : (degrees - minutes / 60m - seconds / 3600m));
            default:
                return degrees + "° " + minutes.ToString("00") + @"' " + seconds.ToString("00") + "\"";
            }
        }

        public static int Encode(int degrees, int minutes, int seconds, int milliseconds)
        {
            return degrees >= 0
                ? degrees * 3600_000 + minutes * 60_000 + seconds * 1000 + milliseconds
                : degrees * 3600_000 - minutes * 60_000 - seconds * 1000 - milliseconds;
        }

        public static (int degrees, int minutes, int seconds, int milliseconds) Decode(int x)
        {
            var degrees = x / 3600_000;
            if (x < 0) {
                x = -x;
            }
            var minutes = (x / 60_000) % 60;
            var seconds = (x / 1000) % 60;
            var milliseconds = x / 1000;
            return (degrees, minutes, seconds, milliseconds);
        }

        public static int DecodeDegrees(int x)
        {
            return x / 3600_000;
        }

        public static int DecodeMinutes(int x)
        {
            return (Abs(x) / 60_000) % 60;
        }

        public static int DecodeSeconds(int x)
        {
            return Abs(x / 1000) % 60;
        }

        public static int DecodeMilliseconds(int x)
        {
            return Abs(x) % 1000;
        }

        static int Abs(int x)
        {
            return (x >= 0) ? x : -x;
        }
    }
}
