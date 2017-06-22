using System;
using System.Linq.Expressions;
using System.Text;
using Xunit;
using WmcSoft.Arithmetics;

using static System.Math;

namespace WmcSoft.Tests
{
    public class CalculatorTests
    {
        public class StringifierVisitor : ExpressionVisitor
        {
            StringBuilder _sb;

            public StringifierVisitor()
            {
                _sb = new StringBuilder();
            }

            public string Accept(Expression expression)
            {
                Visit(expression);
                return _sb.ToString();
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                _sb.Append(node.Value.ToString());
                return base.VisitConstant(node);
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                _sb.Append('(');
                Visit(node.Left);

                switch (node.NodeType) {
                case ExpressionType.Add:
                    _sb.Append(" + ");
                    break;
                case ExpressionType.Subtract:
                    _sb.Append(" - ");
                    break;
                case ExpressionType.Multiply:
                    _sb.Append(" * ");
                    break;
                case ExpressionType.Divide:
                    _sb.Append(" / ");
                    break;
                }

                Visit(node.Right);
                _sb.Append(')');

                return node;
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.DeclaringType == typeof(Math)) {
                    switch (node.Method.Name) {
                    case "Sin":
                    case "Cos":
                        _sb.Append(node.Method.Name.ToLowerInvariant());
                        _sb.Append('(');
                        Visit(node.Arguments[0]);
                        _sb.Append(')');
                        return node;
                    }
                }
                return base.VisitMethodCall(node);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                _sb.Append(node.Name);
                return base.VisitParameter(node);
            }
        }

        public class DerivativeVisitor : ExpressionVisitor
        {

            public Expression<Func<double, double>> Accept(Expression<Func<double, double>> expression)
            {
                var expr = Visit(expression.Body);
                return (Expression<Func<double, double>>)Expression.Lambda(expr, expression.Parameters);
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                return Expression.Constant(0d);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return Expression.Constant(1d);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                return base.VisitMethodCall(node);
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                switch (node.NodeType) {
                case ExpressionType.Constant:
                    return Expression.Constant(0d);
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                    return base.VisitBinary(node);
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                    if (node.Left.NodeType == ExpressionType.Constant) {
                        if (IsOne((ConstantExpression)node.Left))
                            return Visit(node.Right);
                        return Expression.MakeBinary(node.NodeType, node.Left, Visit(node.Right));
                    } else if (node.Right.NodeType == ExpressionType.Constant) {
                        if (IsOne((ConstantExpression)node.Right))
                            return Visit(node.Left);
                        return Expression.MakeBinary(node.NodeType, node.Right, Visit(node.Left));
                    }
                    break;
                }
                throw new InvalidOperationException();
            }

            static bool IsZero(ConstantExpression x)
            {
                return 0d.Equals(x.Value);
            }
            static bool IsOne(ConstantExpression x)
            {
                return 1d.Equals(x.Value);
            }
        }

        class SimplifierVisitor : ExpressionVisitor
        {
            protected override Expression VisitBinary(BinaryExpression node)
            {
                var left = base.Visit(node.Left);
                var right = base.Visit(node.Right);

                switch (node.NodeType) {
                case ExpressionType.Add:
                    if (right.NodeType == ExpressionType.Constant && IsZero((ConstantExpression)right))
                        return left;
                    if (left.NodeType == ExpressionType.Constant && IsZero((ConstantExpression)left))
                        return right;
                    return Expression.Add(left, right);
                case ExpressionType.Subtract:
                    if (right.NodeType == ExpressionType.Constant && IsZero((ConstantExpression)right))
                        return left;
                    if (left.NodeType == ExpressionType.Constant && IsZero((ConstantExpression)left))
                        return Expression.Negate(right);
                    return Expression.Subtract(left, right);
                case ExpressionType.Multiply:
                    if (left.NodeType == ExpressionType.Constant && IsOne((ConstantExpression)left))
                        return right;
                    if (right.NodeType == ExpressionType.Constant && IsOne((ConstantExpression)right))
                        return left;
                    return Expression.Multiply(left, right);
                case ExpressionType.Divide:
                    if (right.NodeType == ExpressionType.Constant && IsOne((ConstantExpression)right))
                        return left;
                    return Expression.Divide(left, right);
                }

                return Expression.MakeBinary(node.NodeType, left, right);
            }

            static bool IsZero(ConstantExpression x)
            {
                return 0d.Equals(x.Value);
            }
            static bool IsOne(ConstantExpression x)
            {
                return 1d.Equals(x.Value);
            }
        }

        [Fact]
        public void CanAddWithArithmetics()
        {
            var calc = new Int32Arithmetics();

            Assert.Equal(2, calc.Add(1, 1));
            Assert.Equal(0, calc.Add(1, -1));
            Assert.Equal(1, calc.Add(1, 0));
        }

        [Fact]
        public void CanStringifyExpression()
        {
            Expression<Func<double, double>> eq = x => 2 * x + 5;

            var stringifier = new StringifierVisitor();
            var actual = stringifier.Accept(eq.Body);

            Assert.NotNull(eq);
            Assert.Equal("((2 * x) + 5)", actual);
        }

        [Fact]
        public void CanStringifyExpressionWithSin()
        {
            Expression<Func<double, double>> eq = x => Sin(x);

            var stringifier = new StringifierVisitor();
            var actual = stringifier.Accept(eq.Body);

            Assert.NotNull(eq);
            Assert.Equal("sin(x)", actual);
        }

        [Fact]
        public void CanSimplifyAddition()
        {
            Expression<Func<double, double>> eq = x => x + 0d;
            var simplifier = new SimplifierVisitor();

            var simplified = simplifier.Visit(eq.Body);

            var stringifier = new StringifierVisitor();
            var actual = stringifier.Accept(simplified);

            Assert.NotNull(eq);
            Assert.Equal("x", actual);
        }

        [Fact]
        public void CanDeriveExpression()
        {
            Expression<Func<double, double>> eq = x => 2 * x + 5;
            var deriver = new DerivativeVisitor();
            var simplifier = new SimplifierVisitor();

            var derivative = deriver.Accept(eq);
            var simplified = simplifier.Visit(derivative.Body);

            var stringifier = new StringifierVisitor();
            var actual = stringifier.Accept(simplified);
            Assert.Equal("2", actual);
        }
    }
}
