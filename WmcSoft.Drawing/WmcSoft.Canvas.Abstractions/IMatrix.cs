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

namespace WmcSoft.Canvas
{
    public interface IMatrix<T> : IEquatable<IMatrix<T>>, ICloneable
    {
        // Mutable
        void MultiplyBy(IMatrix<T> other);
        void PreMultiplyBy(IMatrix<T> other);
        void TranslateBy(T tx, T ty, T tz = default(T));
        void ScaleBy(T scale, T originX = default(T), T originY = default(T));
        void Scale3dBy(T scale, T originX = default(T), T originY = default(T), T originZ = default(T));
        void ScaleNonUniformBy(T scaleX, T scaleY /*= 1*/, T scaleZ /*= 1*/, T originX = default(T), T originY = default(T), T originZ = default(T));
        void RotateBy(T angle, T originX = default(T), T originY = default(T));
        void RotateFromVectorBy(T x, T y);
        void RotateAxisAngleBy(T x, T y, T z, T angle);
        void SkewXBy(T sx);
        void SkewYBy(T sy);
        void Invert();

        // Immutable
        IMatrix<T> Multiply(IMatrix<T> other);
        IMatrix<T> FlipX();
        IMatrix<T> FlipY();
    }

    public static class MatrixExtensions
    {
        public static M Translate<M, T>(this M matrix, T tx, T ty, T tz = default(T))
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.TranslateBy(tx, ty, tz);
            return self;
        }

        public static M Scale<M, T>(this M matrix, T scale, T originX = default(T), T originY = default(T))
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.ScaleBy(scale, originX, originY);
            return self;
        }

        public static M Scale3d<M, T>(this M matrix, T scale, T originX = default(T), T originY = default(T), T originZ = default(T))
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.Scale3dBy(scale, originX, originY, originZ);
            return self;
        }

        public static M ScaleNonUniform<M, T>(this M matrix, T scaleX, T scaleY /*= 1*/, T scaleZ /*= 1*/, T originX = default(T), T originY = default(T), T originZ = default(T))
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.ScaleNonUniformBy(scaleX, scaleY, scaleZ, originX, originY, originZ);
            return self;
        }

        public static M Rotate<M, T>(this M matrix, T angle, T originX = default(T), T originY = default(T))
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.RotateBy(angle, originX, originY);
            return self;
        }

        public static M RotateFromVector<M, T>(this M matrix, T x, T y)
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.RotateFromVectorBy(x, y);
            return self;
        }

        public static M RotateAxisAngle<M, T>(this M matrix, T x, T y, T z, T angle)
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.RotateAxisAngleBy(x, y, z, angle);
            return self;
        }

        public static M SkewX<M, T>(this M matrix, T sx)
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.SkewXBy(sx);
            return self;
        }

        public static M SkewY<M, T>(this M matrix, T sy)
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.SkewXBy(sy);
            return self;
        }

        public static M Inverse<M, T>(this M matrix)
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.Invert();
            return self;
        }
    }
}
