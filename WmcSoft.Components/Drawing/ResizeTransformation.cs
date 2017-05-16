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

using System;
using System.Drawing;
using static System.Math;

namespace WmcSoft.Drawing
{
    public class ResizeTransformation : ImageTransformation
    {
        public ResizeTransformation(int width, int height, bool preserveAspectRatio = true, bool preventEnlarge = true)
        {
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height < 0) throw new ArgumentOutOfRangeException(nameof(height));

            Width = width;
            Height = height;
            PreserveAspectRatio = preserveAspectRatio;
            PreventEnlarge = preventEnlarge;
        }

        public int Height { get; }
        public int Width { get; }
        public bool PreserveAspectRatio { get; }
        public bool PreventEnlarge { get; }

        public override Image Apply(Image image)
        {
            var h = Height;
            var w = Width;
            if (PreserveAspectRatio) {
                var hp = (double)h / image.Height;
                var wp = (double)w / image.Width;
                if (hp > wp)
                    h = (int)Round(wp * image.Height);
                else if (hp < wp)
                    w = (int)Round(hp * image.Width);
            }
            if (PreventEnlarge) {
                h = Min(h, image.Height);
                w = Min(w, image.Width);
            }
            if (image.Width == w && image.Height == h)
                return image;
            return image.ToBitmap(w, h);
        }
    }
}