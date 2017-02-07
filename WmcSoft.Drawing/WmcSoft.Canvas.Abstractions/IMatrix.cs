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
        where T : IConvertible
    {
        // Mutable
        IMatrix<T> MultiplyBy(IMatrix<T> other);
        IMatrix<T> PreMultiplyBy(IMatrix<T> other);
        IMatrix<T> TranslateBy(T tx, T ty, T tz = default(T));
        IMatrix<T> ScaleBy(T scale, T originX = default(T), T originY = default(T));
        IMatrix<T> Scale3dBy(T scale, T originX = default(T), T originY = default(T), T originZ = default(T));
        IMatrix<T> ScaleNonUniformBy(T scaleX, T scaleY /*= 1*/, T scaleZ /*= 1*/, T originX = default(T), T originY = default(T), T originZ = default(T));
        IMatrix<T> RotateBy(T angle, T originX = default(T), T originY = default(T));
        IMatrix<T> RotateFromVectorBy(T x, T y);
        IMatrix<T> RotateAxisAngleBy(T x, T y, T z, T angle);
        IMatrix<T> SkewXBy(T sx);
        IMatrix<T> SkewYBy(T sy);
        IMatrix<T> Invert();
    }

    //public interface IReadOnlyValue<T>
    //{
    //    T this[int index] { get; }
    //}
    //public interface IMutableValue<T> : IReadOnlyValue<T>
    //{
    //    new T this[int index] { get; set; }
    //}

    //public interface IMatrixOperations<M, T>
    //{
    //    M MultiplyBy(M other);
    //    M PreMultiplyBy(M other);
    //    M TranslateBy(T tx, T ty, T tz = default(T));
    //    M ScaleBy(T scale, T originX = default(T), T originY = default(T));
    //    M Scale3dBy(T scale, T originX = default(T), T originY = default(T), T originZ = default(T));
    //    M ScaleNonUniformBy(T scaleX, T scaleY /*= 1*/, T scaleZ /*= 1*/, T originX = default(T), T originY = default(T), T originZ = default(T));
    //    M RotateBy(T angle, T originX = default(T), T originY = default(T));
    //    M RotateFromVectorBy(T x, T y);
    //    M RotateAxisAngleBy(T x, T y, T z, T angle);
    //    M SkewXBy(T sx);
    //    M SkewYBy(T sy);
    //    M Invert();
    //}

    public static class MatrixExtensions
    {
        static class Traits<T> where T : IConvertible
        {
            public static readonly T Zero = (T)Convert.ChangeType(0, typeof(T));
            public static readonly T One = (T)Convert.ChangeType(1, typeof(T));
            public static readonly T MinusOne = (T)Convert.ChangeType(-1, typeof(T));
        }

        class Prototypes<M, T>
            where T : IConvertible
            where M : IMatrix<T>
        {
            public static readonly M FlipX;
            public static readonly M FlipY;

            static Prototypes() {
                var zero = Traits<T>.Zero;
                var one = Traits<T>.One;
                var minusOne = Traits<T>.MinusOne;

                FlipX = (M)Activator.CreateInstance(typeof(M), new object[] { minusOne, zero, zero, one, zero, zero });
                FlipY = (M)Activator.CreateInstance(typeof(M), new object[] { one, zero, zero, minusOne, zero, zero });
            }
        }

        public static M Translate<M, T>(this M matrix, T tx, T ty, T tz = default(T))
            where T : IConvertible
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.TranslateBy(tx, ty, tz);
            return self;
        }

        public static M Scale<M, T>(this M matrix, T scale, T originX = default(T), T originY = default(T))
            where T : IConvertible
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.ScaleBy(scale, originX, originY);
            return self;
        }

        public static M Scale3d<M, T>(this M matrix, T scale, T originX = default(T), T originY = default(T), T originZ = default(T))
            where T : IConvertible
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.Scale3dBy(scale, originX, originY, originZ);
            return self;
        }

        public static M ScaleNonUniform<M, T>(this M matrix, T scaleX, T scaleY /*= 1*/, T scaleZ /*= 1*/, T originX = default(T), T originY = default(T), T originZ = default(T))
            where T : IConvertible
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.ScaleNonUniformBy(scaleX, scaleY, scaleZ, originX, originY, originZ);
            return self;
        }

        public static M Rotate<M, T>(this M matrix, T angle, T originX = default(T), T originY = default(T))
            where T : IConvertible
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.RotateBy(angle, originX, originY);
            return self;
        }

        public static M RotateFromVector<M, T>(this M matrix, T x, T y)
            where T : IConvertible
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.RotateFromVectorBy(x, y);
            return self;
        }

        public static M RotateAxisAngle<M, T>(this M matrix, T x, T y, T z, T angle)
            where T : IConvertible
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.RotateAxisAngleBy(x, y, z, angle);
            return self;
        }

        public static M SkewX<M, T>(this M matrix, T sx)
            where T : IConvertible
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.SkewXBy(sx);
            return self;
        }

        public static M SkewY<M, T>(this M matrix, T sy)
            where T : IConvertible
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.SkewXBy(sy);
            return self;
        }

        public static M Inverse<M, T>(this M matrix)
            where T : IConvertible
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.Invert();
            return self;
        }

        public static M Multiply<M, T>(this M matrix, IMatrix<T> other)
            where T : IConvertible
            where M : IMatrix<T> {
            var self = (M)matrix.Clone();
            self.MultiplyBy(other);
            return self;
        }

        public static M FlipX<M, T>(this M matrix)
            where T : IConvertible
            where M : IMatrix<T> {
            return Multiply(matrix, Prototypes<M, T>.FlipX);
        }

        public static M FlipY<M, T>(this M matrix)
            where T : IConvertible
            where M : IMatrix<T> {
            return Multiply(matrix, Prototypes<M, T>.FlipY);
        }
    }
}