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

namespace WmcSoft.Drawing
{
    public static class ColorExtensions
    {
        public static Color MakeOpaque(this Color color) {
            if (color.A < 255)
                return Color.FromArgb(255, color);
            return color;
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
            var r = Clamp(color.R * d);
            var g = Clamp(color.G * d);
            var b = Clamp(color.B * d);
            return Color.FromArgb(color.A, r, g, b);
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
    }
}
