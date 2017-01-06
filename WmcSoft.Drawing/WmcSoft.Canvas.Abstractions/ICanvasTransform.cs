﻿#region Licence

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

 ****************************************************************************
 * Based on <https://html.spec.whatwg.org/multipage/scripting.html#2dcontext>
 ****************************************************************************/

#endregion

using System.Numerics;

namespace WmcSoft.Canvas
{
    public interface ICanvasTransform<T>
    {
        void Scale(T x, T y);
        void Rotate(T angle);
        void Translate(T x, T y);
        void Transform(T a, T b, T c, T d, T e, T f);

        Matrix4x4 TransformationMatrix { get; set; }
    }

    public static class CanvasTransformExtensions
    {
        public static void Set2DTransformation<T>(this ICanvasTransform<T> canvas, T a, T b, T c, T d, T e, T f) {
            throw new System.NotImplementedException();
        }

        public static void ResetTransformationMatrix<T>(this ICanvasTransform<T> canvas) {
            canvas.TransformationMatrix = Matrix4x4.Identity;
        }
    }
}
