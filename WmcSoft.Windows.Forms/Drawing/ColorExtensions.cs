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
using System.Drawing;
using static System.Math;

namespace WmcSoft.Drawing
{
    /// <summary>
    /// Defines the extension methods to the <see cref="Color"/> struct.
    /// This is a static class.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Makes the color opaque.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The same color with an alpha channel of 255.</returns>
        public static Color MakeOpaque(this Color color) {
            return Color.FromArgb(255, color);
        }

        /// <summary>
        ///  Takes a color and returns a new one with all the values inverted 
        ///  inside of 255.
        /// </summary>
        /// <param name="color">Color to invert.</param>
        /// <returns>Inverted color</returns>
        public static Color Flip(this Color color) {
            return Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B);
        }

        /// <summary>
        ///   A function for scaling a color down assembly certain percentage
        /// </summary>
        /// <param name="color">Color to be scaled.</param>
        /// <param name="percentage">Percentage to scale it, between 0 and 1.</param>
        /// <returns>New color object.</returns>
        public static Color Scale(this Color color, double d) {
            var R = Clamp(color.R * d);
            var G = Clamp(color.G * d);
            var B = Clamp(color.B * d);
            return Color.FromArgb(color.A, R, G, B);
        }

        static int Clamp(double value) {
            int r = (int)value;
            if (r > 255)
                return 255;
            if (r < 0)
                return 0;
            return r;
        }

        /// <summary>
        ///   A function for scaling a color down assembly certain percentage
        /// </summary>
        /// <param name="color">Color to be scaled.</param>
        /// <param name="percentage">Percentage to scale it, between 0 and 100.</param>
        /// <returns>New color object.</returns>
        public static Color Scale(this Color color, int percentage) {
            return color.Scale(percentage / 100d);
        }

        static float F(int channel) {
            var c = channel / 255f;
            return c <= 0.03928f ? c / 12.92f : (float)Pow((c + 0.055d) / 1.055d, 2.4d);
        }

        /// <summary>
        /// Computes the relative luminance of the specified color
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The relative lumnance.</returns>
        /// <remarks>See <http://www.w3.org/TR/2008/REC-WCAG20-20081211/#relativeluminancedef>.</remarks>
        public static float GetRelativeLuminance(this Color color) {
            var R = F(color.R);
            var G = F(color.G);
            var B = F(color.B);
            return 0.2126f * R + 0.7152f * G + 0.0722f * B;
        }

        /// <summary>
        /// Computes the monochrome luminance of the specified color
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The relative lumnance.</returns>
        /// <remarks>Uses the NTSC formula  Y = .299*r + .587*g + .114*b.</remarks>
        public static double GetLuminance(this Color color) {
            return 0.299 * color.R + 0.587 * color.G + 0.114 * color.B;
        }

        /// <summary>
        /// Returns a gray version of this color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The gray version of the color.</returns>
        public static Color ToGray(this Color color) {
            int y = (int)(Round(GetLuminance(color)));   // round to nearest int
            Color gray = Color.FromArgb(y, y, y);
            return gray;
        }

        /// <summary>
        /// Returns <c>true</c> when the <paramref name="color"/> is compatible with the <paramref name="other"/> color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="other">The other color.</param>
        /// <returns><c>true</c> when the two colors are compatible; otherwise, <c>false</c>.</returns>
        public static bool IsCompatibleWith(this Color color, Color other) {
            return Abs(GetLuminance(color) - GetLuminance(other)) >= 128.0;
        }
    }
}