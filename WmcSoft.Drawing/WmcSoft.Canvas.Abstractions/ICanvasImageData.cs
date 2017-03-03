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

namespace WmcSoft.Canvas
{
    /// <summary>
    /// Pixel manipulation.
    /// </summary>
    public interface ICanvasImageData<TImageData, T>
        where TImageData : IImageSize<T>
    {
        TImageData CreateImageData(T sw, T sh);
        TImageData GetImageData(T sx, T sy, T sw, T sh);
        void PutImageData(TImageData imagedata, T dx, T dy, T dirtyX, T dirtyY, T dirtyWidth, T dirtyHeight);
    }

    public static class CanvasImageDataExtensions
    {
        public static TImageData CreateImageData<TImageData, T>(this ICanvasImageData<TImageData, T> canvas, TImageData imagedata)
            where TImageData : IImageSize<T> {
            return canvas.CreateImageData(imagedata.Width, imagedata.Height);
        }

        public static void PutImageData<TImageData, T>(this ICanvasImageData<TImageData, T> canvas, TImageData imagedata, T dx, T dy, T dirtyX = default(T), T dirtyY = default(T), T? dirtyWidth = null, T? dirtyHeight = null)
            where T : struct
            where TImageData : IImageSize<T> {
            canvas.PutImageData(imagedata, dx, dy, dirtyX, dirtyY, dirtyWidth ?? imagedata.Width, dirtyHeight ?? imagedata.Height);
        }
    }
}