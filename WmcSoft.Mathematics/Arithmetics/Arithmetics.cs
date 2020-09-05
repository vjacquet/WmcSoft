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
using System.Linq.Expressions;

namespace WmcSoft.Arithmetics
{
    public static class Arithmetics<T>
    {
        private static readonly Func<T, T, T> Add;
        private static readonly Func<T, T, T> Subtract;
        private static readonly Func<T, T, T> Multiply;
        private static readonly Func<T, T, T> Divide;
        private static readonly Func<T, T, T> Remainder;
        private static readonly Func<T, T> Negate;
        private static readonly Func<T, T> Reciprocal;
        private static readonly T Zero = default;
        private static readonly T One = default;

        private static Func<T, T> Compile(UnaryExpression op)
        {
            return Expression.Lambda<Func<T, T>>(op, (ParameterExpression)op.Operand).Compile();
        }

        private static Func<T, T, T> Compile(BinaryExpression op)
        {
            return Expression.Lambda<Func<T, T, T>>(op, (ParameterExpression)op.Left, (ParameterExpression)op.Right).Compile();
        }

        private static TDelegate Compile<TDelegate>(Expression body, params ParameterExpression[] parameters)
        {
            return Expression.Lambda<TDelegate>(body, parameters).Compile();
        }

        static Arithmetics()
        {
            var x = Expression.Parameter(typeof(T), "x");
            var y = Expression.Parameter(typeof(T), "y");

            Add = Compile(Expression.Add(x, y));
            Subtract = Compile(Expression.Subtract(x, y));
            Multiply = Compile(Expression.Multiply(x, y));
            Divide = Compile(Expression.Divide(x, y));
            Remainder = Compile(Expression.Modulo(x, y));
            Negate = Compile(Expression.Negate(x));
            Reciprocal = Compile<Func<T, T>>(Expression.Divide(Expression.Constant(One, typeof(T)), x));
        }

        public struct Impl : IArithmetics<T>
        {
            public T Zero => Arithmetics<T>.Zero;

            public T One => Arithmetics<T>.One;

            public bool SupportReciprocal => throw new NotImplementedException();

            public T Add(T x, T y) => Arithmetics<T>.Add(x, y);
            public T Subtract(T x, T y) => Arithmetics<T>.Subtract(x, y);
            public T Multiply(T x, T y) => Arithmetics<T>.Multiply(x, y);
            public T Divide(T x, T y) => Arithmetics<T>.Divide(x, y);
            public T Remainder(T x, T y) => Arithmetics<T>.Remainder(x, y);

            public T Negate(T x) => Arithmetics<T>.Negate(x);
            public T Reciprocal(T x) => Arithmetics<T>.Reciprocal(x);

            public T DivideRemainder(T x, T y, out T remainder)
            {
                remainder = Remainder(x, y);
                return Divide(x, y);
            }
        }

        public static readonly Impl Default;
    }
}
