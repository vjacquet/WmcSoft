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

namespace WmcSoft.Drawing
{
    public class CropTransformation : ImageTransformation
    {
        public CropTransformation(int top, int right, int bottom, int left) {
            if (top < 0) throw new ArgumentOutOfRangeException("top");
            if (right < 0) throw new ArgumentOutOfRangeException("right");
            if (bottom < 0) throw new ArgumentOutOfRangeException("bottom");
            if (left < 0) throw new ArgumentOutOfRangeException("left");

            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public int Top { get; }
        public int Right { get; }
        public int Bottom { get; }
        public int Left { get; }

        public override Image Apply(Image image) {
            var h = Top + Bottom;
            var w = Left + Right;
            if (h > image.Height | w > image.Width) {
                return image;
            }

            using (var bitmap = image.ToBitmap()) {
                try {
                    var rect = new Rectangle(Left, Top, image.Width - w, image.Height - h);
                    return bitmap.Clone(rect, image.PixelFormat);
                }
                catch (OutOfMemoryException) {
                    return image;
                }
            }
        }
    }
}