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

namespace WmcSoft.Drawing
{
    public struct ImageData
    {
        public ImageData(uint sw, uint sh) {
            Width = sw;
            Height = sh;
            Data = new ByteClampedArray(new byte[4 * sw * sh]);
        }

        public ImageData(ByteClampedArray data, uint sw) {
            var length = (uint)data.Count;
            if (length % 4 != 0) throw new ArgumentException(nameof(data));
            length /= 4;
            if (length % sw != 0) throw new ArgumentOutOfRangeException(nameof(data));
            length /= sw;

            Width = sw;
            Height = length;
            Data = data;
        }

        public ImageData(ByteClampedArray data, uint sw, uint sh) {
            var length = (uint)data.Count;
            if (length % 4 != 0) throw new ArgumentException(nameof(data));
            length /= 4;
            if (length % sw != 0) throw new ArgumentOutOfRangeException(nameof(data));
            length /= sw;
            if (sh != length) throw new ArgumentOutOfRangeException(nameof(sh));

            Width = sw;
            Height = sh;
            Data = data;
        }

        public uint Width { get; }
        public uint Height { get; }
        public ByteClampedArray Data { get; }
    }
}
