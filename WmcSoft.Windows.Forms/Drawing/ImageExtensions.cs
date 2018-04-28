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

using System.Drawing;
using System.Drawing.Imaging;

namespace WmcSoft.Drawing
{
    /// <summary>
    /// Defines the extension methods to the <see cref="Image"/> class. This is a static class.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        ///   Converts a <see cref="Icon"/> to a <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="icon">Icon to convert.</param>
        /// <param name="color">Mask color to use as background.</param>
        /// <returns>Bitmap object.</returns>
        public static Bitmap ToBitmap(this Icon icon, Color color) {
            var bmp = new Bitmap(icon.Width, icon.Height, PixelFormat.Format32bppPArgb);
            using (var g = Graphics.FromImage(bmp))
            using (var brush = new SolidBrush(color)) {
                g.FillRectangle(brush, new Rectangle(0, 0, bmp.Width, bmp.Height));
                g.DrawIcon(icon, 0, 0);
            }

            bmp.MakeTransparent(color);
            return bmp;
        }

        /// <summary>
        ///   Converts a <see cref="Icon"/> to a <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="icon">Icon to convert.</param>
        /// <returns>Bitmap object.</returns>
        public static Bitmap ToBitmap(this Icon icon) {
            return ToBitmap(icon, Color.Fuchsia);
        }

        /// <summary>
        /// Creates a negative image.
        /// </summary>
        /// <param name="source">The source image</param>
        /// <returns>The negative image</returns>
        public static Bitmap Negative(this Bitmap source) {
            // http://mariusbancila.ro/blog/2009/11/13/using-colormatrix-for-creating-negative-image/
            var attr = new ImageAttributes();
            // create the negative color matrix
            var m = new ColorMatrix(new[] {
                    new [] { -1f, 0f, 0f, 0f, 0f},
                    new [] { 0f, -1f, 0f, 0f, 0f},
                    new [] { 0f, 0f, -1f, 0f, 0f},
                    new [] { 0f, 0f, 0f, +1f, 0f},
                    new [] { 0f, 0f, 0f, 0f, +1f},
                });
            attr.SetColorMatrix(m);

            var bmp = new Bitmap(source.Width, source.Height);
            using (var g = Graphics.FromImage(bmp)) {
                g.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attr);
            }
            return bmp;
        }
    }
}
