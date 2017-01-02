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

namespace WmcSoft.Drawing
{
    /// <summary>
    /// Pixel manipulation.
    /// </summary>
    public interface ICanvasImageData
    {
        // TODO: Define extensions to minimize the interface?
        ImageData CreateImageData(double sw, double sh);
        ImageData CreateImageData(ImageData imagedata);
        ImageData GetImageData(double sx, double sy, double sw, double sh);
        void PutImageData(ImageData imagedata, double dx, double dy);
        void PutImageData(ImageData imagedata, double dx, double dy, double dirtyX, double dirtyY, double dirtyWidth, double dirtyHeight);
    }
}