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
using System.Drawing.Drawing2D;

namespace WmcSoft.Drawing
{
    public static class ImageExtensions
    {
        public static Bitmap ToBitmap(this Image image, bool preserveResolution = true) {
            return ToBitmap(image, image.Width, image.Height, preserveResolution);
        }

        public static Bitmap ToBitmap(this Image image, int width, int height, bool preserveResolution = true) {
            var bitmap = new Bitmap(width, height, image.PixelFormat);
            if (preserveResolution)
                bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (var graphics = Graphics.FromImage(bitmap)) {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return bitmap;
        }
    }
}
