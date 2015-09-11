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

using System.Drawing;
using System.Runtime.InteropServices;

namespace WmcSoft.Drawing
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HSLColor
    {
        #region Fields

        private const int HSLMax = 240;
        private const int RGBMax = 255;

        private readonly int _hue;
        private readonly int _saturation;
        private readonly int _luminosity;

        #endregion

        #region Lifecycle

        public HSLColor(int hue, int saturation, int luminosity) {
            _hue = hue;
            _saturation = saturation;
            _luminosity = luminosity;
        }

        public HSLColor(Color color) {
            int r = color.R;
            int g = color.G;
            int b = color.B;
            int max = Algorithms.Max(r, g, b);
            int min = Algorithms.Min(r, g, b);
            int sum = max + min;
            _luminosity = (sum * HSLMax) / (RGBMax * 2);
            int delta = max - min;
            if (delta == 0) {
                _saturation = _hue = 0;
            } else {
                if (_luminosity <= HSLMax / 2) {
                    _saturation = ((delta * HSLMax) + (sum / 2)) / sum;
                } else {
                    _saturation = ((delta * HSLMax) + ((RGBMax * 2 - sum) / 2)) / (RGBMax * 2 - sum);
                }

                int R = (((max - r) * HSLMax / 6) + (delta / 2)) / delta;
                int G = (((max - g) * HSLMax / 6) + (delta / 2)) / delta;
                int B = (((max - b) * HSLMax / 6) + (delta / 2)) / delta;
                if (r == max)
                    _hue = Mod(B - G);
                else if (g == max)
                    _hue = Mod((HSLMax / 3 + R) - B);
                else
                    _hue = Mod((2 * HSLMax / 3 + G) - R);
            }
        }

        #endregion

        #region Properties

        public int Hue {
            get { return _hue; }
        }

        public int Luminosity {
            get { return _luminosity; }
        }

        public int Saturation {
            get { return _saturation; }
        }

        #endregion

        #region Operators

        public static bool operator ==(HSLColor a, HSLColor b) {
            return a.Equals(b);
        }

        public static bool operator !=(HSLColor a, HSLColor b) {
            return !a.Equals(b);
        }

        public override bool Equals(object o) {
            if (o is HSLColor) {
                HSLColor color = (HSLColor)o;
                return (_hue == color._hue) && (_saturation == color._saturation) && (_luminosity == color._luminosity);
            }
            return false;
        }

        public override int GetHashCode() {
            return (_hue << 6) | (_saturation << 2) | _luminosity;
        }

        public static implicit operator Color(HSLColor hsl) {
            return ToColor(hsl._hue, hsl._saturation, hsl._luminosity);
        }
        public static Color ToColor(HSLColor hsl) {
            return hsl;
        }

        public static implicit operator HSLColor(Color rgb) {
            return new HSLColor(rgb);
        }
        public static HSLColor FromColor(Color rgb) {
            return rgb;
        }

        #endregion

        static int MulDiv(int number, long numerator, int denominator) {
            return (int)(number * numerator / denominator);
        }

        static int Mod(int x) {
            if (x < 0)
                x += HSLMax;
            else if (x > HSLMax)
                x -= HSLMax;
            return x;
        }
        static byte Normalize(int value) {
            return (byte)((255 * value + HSLMax / 2) / HSLMax);
        }

        static Color ToColor(int hue, int saturation, int luminosity) {
            // see http://axonflux.com/handy-rgb-to-hsl-and-rgb-to-hsv-color-model-c
            if (saturation == 0) {
                var c = Normalize(luminosity);
                return Color.FromArgb(c, c, c);
            }

            var q = (luminosity <= HSLMax / 2)
                ? (luminosity * (HSLMax + saturation) + HSLMax / 2) / HSLMax
                : (luminosity + saturation) - (luminosity * saturation + HSLMax / 2) / HSLMax;
            var p = (2 * luminosity) - q;
            var r = Normalize(HueToRGB(p, q, hue + HSLMax / 3));
            var g = Normalize(HueToRGB(p, q, hue));
            var b = Normalize(HueToRGB(p, q, hue - HSLMax / 3));
            return Color.FromArgb(r, g, b);
        }

        private static int HueToRGB(int p, int q, int t) {
            t = Mod(t);

            if (t < HSLMax / 6)
                return p + (q - p) * 6 * t / HSLMax;
            if (t < HSLMax / 2)
                return q;
            if (t < 2 * HSLMax / 3)
                return p + (q - p) * (2 * HSLMax / 3 - t) * 6 / HSLMax;
            return p;
        }

        public override string ToString() {
            return string.Concat(new object[] { _hue, ", ", _luminosity, ", ", _saturation });
        }
    }
}
