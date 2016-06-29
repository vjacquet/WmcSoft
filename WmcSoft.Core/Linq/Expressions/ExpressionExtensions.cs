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
using System.Diagnostics;
using System.Linq.Expressions;

namespace WmcSoft.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static MemberExpression Field(this Expression expression, string fieldName) {
            // guard against null, this extensions should not be used for static members
            if (expression == null) throw new ArgumentNullException("expression");

            return Expression.Field(expression, fieldName);
        }

        public static MemberExpression Property(this Expression expression, string propertyName) {
            // guard against null, this extensions should not be used for static members
            if (expression == null) throw new ArgumentNullException("expression");

            return Expression.Property(expression, propertyName);
        }

        public static IndexExpression Property(this Expression expression, string propertyName, params Expression[] arguments) {
            // guard against null, this extensions should not be used for static members
            if (expression == null) throw new ArgumentNullException("expression");

            return Expression.Property(expression, propertyName, arguments);
        }

        public static MemberExpression PropertyOrField(this Expression expression, string propertyOrFieldName) {
            // guard against null, this extensions should not be used for static members
            if (expression == null) throw new ArgumentNullException("expression");

            return Expression.PropertyOrField(expression, propertyOrFieldName);
        }

        public static MemberExpression FollowPropertyOrField(this Expression expression, string propertyOrFieldPath) {
            // guard against null, this extensions should not be used for static members
            if (expression == null) throw new ArgumentNullException("expression");

            if (propertyOrFieldPath == null)
                throw new ArgumentException("propertyOrFieldPath");
            var parts = propertyOrFieldPath.Split('.');
            if (parts.Length == 0)
                throw new ArgumentException("propertyOrFieldPath");
            var member = Expression.PropertyOrField(expression, parts[0]);
            for (int i = 1; i < parts.Length; i++) {
                member = Expression.PropertyOrField(member, parts[i]);
            }
            return member;
        }
    }
}
