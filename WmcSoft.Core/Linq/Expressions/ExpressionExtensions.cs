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
using System.Linq.Expressions;

namespace WmcSoft.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        #region Call

        public static MethodCallExpression Call(this Expression expression, string methodName, Type[] typeArguments, params Expression[] arguments) {
            // guard against null, this extensions should not be used for static members
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return Expression.Call(expression, methodName, typeArguments, arguments);
        }

        public static MethodCallExpression Call(this Expression expression, string methodName, params Expression[] arguments) {
            return Call(expression, methodName, null, arguments);
        }

        public static MethodCallExpression Call<T>(this Expression expression, string methodName, params Expression[] arguments) {
            return Call(expression, methodName, new Type[] { typeof(T) }, arguments);
        }

        public static MethodCallExpression Call<T1, T2>(this Expression expression, string methodName, params Expression[] arguments) {
            return Call(expression, methodName, new Type[] { typeof(T1), typeof(T2) }, arguments);
        }

        public static MethodCallExpression Call<T1, T2, T3>(this Expression expression, string methodName, params Expression[] arguments) {
            return Call(expression, methodName, new Type[] { typeof(T1), typeof(T2), typeof(T3) }, arguments);
        }

        public static MethodCallExpression Call<T1, T2, T3, T4>(this Expression expression, string methodName, params Expression[] arguments) {
            return Call(expression, methodName, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, arguments);
        }

        public static MethodCallExpression Call<T1, T2, T3, T4, T5>(this Expression expression, string methodName, params Expression[] arguments) {
            return Call(expression, methodName, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }, arguments);
        }

        public static MethodCallExpression Call<T1, T2, T3, T4, T5, T6>(this Expression expression, string methodName, params Expression[] arguments) {
            return Call(expression, methodName, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) }, arguments);
        }

        public static MethodCallExpression Call<T1, T2, T3, T4, T5, T6, T7>(this Expression expression, string methodName, params Expression[] arguments) {
            return Call(expression, methodName, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) }, arguments);
        }

        #endregion

        #region Field / Property

        public static MemberExpression Field(this Expression expression, string fieldName) {
            // guard against null, this extensions should not be used for static members
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return Expression.Field(expression, fieldName);
        }

        public static MemberExpression Property(this Expression expression, string propertyName) {
            // guard against null, this extensions should not be used for static members
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return Expression.Property(expression, propertyName);
        }

        public static IndexExpression Property(this Expression expression, string propertyName, params Expression[] arguments) {
            // guard against null, this extensions should not be used for static members
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return Expression.Property(expression, propertyName, arguments);
        }

        public static MemberExpression PropertyOrField(this Expression expression, string propertyOrFieldName) {
            // guard against null, this extensions should not be used for static members
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return Expression.PropertyOrField(expression, propertyOrFieldName);
        }

        public static MemberExpression FollowPropertyOrField(this Expression expression, string propertyOrFieldPath) {
            // guard against null, this extensions should not be used for static members
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            if (propertyOrFieldPath == null) throw new ArgumentNullException(nameof(propertyOrFieldPath));
            var parts = propertyOrFieldPath.Split('.');
            if (parts.Length == 0) throw new ArgumentException(nameof(propertyOrFieldPath));

            var member = Expression.PropertyOrField(expression, parts[0]);
            for (int i = 1; i < parts.Length; i++) {
                member = Expression.PropertyOrField(member, parts[i]);
            }
            return member;
        }

        #endregion

        #region Combiners

        static Expression Replace(Expression expression, Expression oldValue, Expression newValue) {
            var visitor = new ReplaceExpressionVisitor(oldValue, newValue);
            return visitor.Visit(expression);
        }

        /// <summary>
        /// Creates a typed predicate expression equivalent to <paramref name="x"/> && <paramref name="y"/>.
        /// </summary>
        /// <typeparam name="T">The type of parameter the predicates work on.</typeparam>
        /// <param name="x">The left side of the binary expression.</param>
        /// <param name="y">The right side of the binary expression. </param>
        /// <returns>A new predicate expression</returns>
        /// <remarks>Code found at http://stackoverflow.com/questions/457316/combining-two-expressions-expressionfunct-bool, written by Marc Gravell.</remarks>
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> x, Expression<Func<T, bool>> y) {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            var p = Expression.Parameter(typeof(T));
            var left = Replace(x.Body, x.Parameters[0], p);
            var right = Replace(y.Body, y.Parameters[0], p);

            var body = Expression.AndAlso(left, right);
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        /// <summary>
        /// Creates a typed predicate expression equivalent to <paramref name="x"/> || <paramref name="y"/>.
        /// </summary>
        /// <typeparam name="T">The type of parameter the predicates work on.</typeparam>
        /// <param name="x">The left side of the binary expression.</param>
        /// <param name="y">The right side of the binary expression. </param>
        /// <returns>A new predicate expression</returns>
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> x, Expression<Func<T, bool>> y) {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            var p = Expression.Parameter(typeof(T));
            var left = Replace(x.Body, x.Parameters[0], p);
            var right = Replace(y.Body, y.Parameters[0], p);

            var body = Expression.OrElse(left, right);
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        public static Expression<Func<T, bool>> Negation<T>(this Expression<Func<T, bool>> predicate) {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var p = predicate.Parameters[0];
            var body = Expression.Not(predicate.Body);
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        public static Expression<Func<T, bool>> Conjunction<T>(this IList<Expression<Func<T, bool>>> predicates) {
            if (predicates == null) throw new ArgumentNullException(nameof(predicates));

            switch (predicates.Count) {
            case 0:
                throw new InvalidOperationException();
            case 1:
                return predicates[1];
            default:
                var result = predicates[predicates.Count - 2];
                for (int i = predicates.Count - 2; i >= 0; i--) {
                    result = predicates[i].AndAlso(result);
                }
                return result;
            }
        }

        public static Expression<Func<T, bool>> Disjunction<T>(this IList<Expression<Func<T, bool>>> predicates) {
            if (predicates == null) throw new ArgumentNullException(nameof(predicates));

            switch (predicates.Count) {
            case 0:
                throw new InvalidOperationException();
            case 1:
                return predicates[1];
            default:
                var result = predicates[predicates.Count - 2];
                for (int i = predicates.Count - 2; i >= 0; i--) {
                    result = predicates[i].OrElse(result);
                }
                return result;
            }
        }

        #endregion
    }
}
