#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Algebra;

namespace WmcSoft.Arithmetics
{
    public static class Operations
    {
        /// <summary>
        /// Compute the dot products of two vectors.
        /// </summary>
        /// <param name="x">The first vector</param>
        /// <param name="y">The second vector</param>
        /// <returns>The dot product.</returns>
        /// <remarks>null vector are consider like empty vector and if the two vector have different size, 
        /// the small is padded with zeros.</remarks>
        public static double DotProduct(this double[] x, double[] y) {
            if (x == null || y == null)
                return 0d;
            var length = Math.Min(x.Length, y.Length);
            var result = x[0] * y[0];
            for (int i = 1; i < length; i++) {
                result += x[i] * y[i];
            }
            return result;
        }

        /// <summary>
        /// Compute the dot products of two vectors.
        /// </summary>
        /// <param name="x">The first vector</param>
        /// <param name="y">The second vector</param>
        /// <returns>The dot product.</returns>
        /// <remarks>null vector are consider like empty vector and if the two vector have different size, 
        /// the small is padded with zeros.</remarks>
        public static T DotProduct<T, A>(this A arithmetics, T[] x, T[] y)
            where A : IArithmetics<T> {
            if (x == null || y == null)
                return arithmetics.Zero;
            var length = Math.Min(x.Length, y.Length);
            var result = arithmetics.Multiply(x[0], y[0]);
            for (int i = 1; i < length; i++) {
                result = arithmetics.Add(result, arithmetics.Multiply(x[i], y[i]));
            }
            return result;
        }

        public static T DotProduct<T, A>(this T[] x, T[] y)
            where A : IArithmetics<T>, new() {
            return DotProduct(new A(), x, y);
        }

        static bool Odd(int n) {
            return (n % 2) == 1;
        }
        static bool Even(int n) {
            return (n % 2) == 0;
        }
        static int HalfNonNegative(int n) {
            return n / 2;
        }

        static T PowerAccumulateSemiGroup<T, G>(T x, T y, int n, G group)
            where G : IGroupLike<T> {
            // precondition: n >= 0
            if (n == 0)
                return x;
            for (; ; ) {
                if (Odd(n)) {
                    x = group.Eval(x, y);
                    if (n == 1)
                        return x;
                }
                y = group.Eval(y, y);
                n = HalfNonNegative(n);
            }
        }

        public static T PowerSemiGroup<T, G>(this T x, int n, G group)
            where G : IGroupLike<T> {
            // precondition: n > 0
            while (Even(n)) {
                x = group.Eval(x, x);
                n = HalfNonNegative(n);
            }
            if (n == 1)
                return x;
            return PowerAccumulateSemiGroup(x, group.Eval(x, x), HalfNonNegative(n - 1), group);
        }

        public static T PowerMonoid<T, G>(this T x, int n, G group)
            where G : IGroupLike<T> {
            if (n == 0)
                return group.Identity;
            return PowerSemiGroup(x, n, group);
        }

        public static T Power<T, G>(this T x, int n, G group)
            where G : IGroupLike<T> {
            if (n < 0) {
                n = -n;
                x = group.Inverse(x);
            }
            return PowerMonoid(x, n, group);
        }

        public static T PowerSemiGroup<T, G>(this T x, int n)
            where G : IGroupLike<T>, new() {
            return PowerSemiGroup(x, n, new G());
        }

        public static T PowerMonoid<T, G>(this T x, int n)
            where G : IGroupLike<T>, new() {
            return PowerMonoid(x, n, new G());
        }

        public static T Power<T, G>(this T x, int n)
            where G : IGroupLike<T>, new() {
            return Power(x, n, new G());
        }
    }
}
