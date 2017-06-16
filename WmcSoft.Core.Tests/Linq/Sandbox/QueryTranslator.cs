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

 ****************************************************************************
 * this code is adapted from the series
 * <https://blogs.msdn.microsoft.com/mattwar/2008/11/18/linq-building-an-iqueryable-provider-series/>
 * 
 ****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Linq.Sandbox
{
    internal class QueryTranslator : ExpressionVisitor
    {
        StringBuilder sb;

        internal QueryTranslator() {
        }

        internal string Translate(Expression expression) {
            this.sb = new StringBuilder();
            this.Visit(expression);
            return this.sb.ToString();
        }

        private static Expression StripQuotes(Expression e) {
            while (e.NodeType == ExpressionType.Quote) {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m) {
            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where") {
                sb.Append("SELECT * FROM (");
                this.Visit(m.Arguments[0]);
                sb.Append(") AS T WHERE ");
                LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                this.Visit(lambda.Body);
                return m;
            }

            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u) {
            switch (u.NodeType) {
            case ExpressionType.Not:
                sb.Append(" NOT ");
                this.Visit(u.Operand);
                break;
            default:
                throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b) {
            sb.Append("(");
            this.Visit(b.Left);
            switch (b.NodeType) {
            case ExpressionType.And:
                sb.Append(" AND ");
                break;
            case ExpressionType.Or:
                sb.Append(" OR ");
                break;
            case ExpressionType.Equal:
                sb.Append(" = ");
                break;
            case ExpressionType.NotEqual:
                sb.Append(" <> ");
                break;
            case ExpressionType.LessThan:
                sb.Append(" < ");
                break;
            case ExpressionType.LessThanOrEqual:
                sb.Append(" <= ");
                break;
            case ExpressionType.GreaterThan:
                sb.Append(" > ");
                break;
            case ExpressionType.GreaterThanOrEqual:
                sb.Append(" >= ");
                break;
            default:
                throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
            this.Visit(b.Right);
            sb.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c) {
            IQueryable q = c.Value as IQueryable;
            if (q != null) {
                // assume constant nodes w/ IQueryables are table references
                sb.Append("SELECT * FROM ");
                sb.Append(q.ElementType.Name);
            } else if (c.Value == null) {
                sb.Append("NULL");
            } else {
                switch (Type.GetTypeCode(c.Value.GetType())) {
                case TypeCode.Boolean:
                    sb.Append(((bool)c.Value) ? 1 : 0);
                    break;
                case TypeCode.String:
                    sb.Append("'");
                    sb.Append(c.Value);
                    sb.Append("'");
                    break;
                case TypeCode.Object:
                    throw new NotSupportedException(string.Format("The constant for '{ 0}' is not supported", c.Value));
                default:
                    sb.Append(c.Value);
                    break;
                }
            }
            return c;
        }

        protected override Expression VisitMember(MemberExpression m) {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter) {
                sb.Append(m.Member.Name);
                return m;
            }
            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }
    }

}