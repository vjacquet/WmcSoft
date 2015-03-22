using System;
using System.Linq.Expressions;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Arithmetics;

namespace WmcSoft.Tests
{
    [TestClass]
    public class CalculatorTests
    {

        public class StringifierVisitor : ExpressionVisitor
        {
            StringBuilder _sb;

            public StringifierVisitor() {
                _sb = new StringBuilder();
            }

            public string Accept(Expression expression) {
                Visit(expression);
                return _sb.ToString();
            }

            protected override Expression VisitConstant(ConstantExpression node) {
                _sb.Append(node.Value.ToString());
                return base.VisitConstant(node);
            }

            protected override Expression VisitBinary(BinaryExpression node) {
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

            protected override Expression VisitGoto(GotoExpression node) {
                throw new NotSupportedException();
            }

            protected override Expression VisitParameter(ParameterExpression node) {
                _sb.Append(node.Name);
                return base.VisitParameter(node);
            }
        }

        public class DerivativeVisitor : ExpressionVisitor
        {

        }

        [TestMethod]
        public void CanAdd() {
            var calc = new Int32Arithmetics();

            Assert.AreEqual(2, calc.Add(1, 1));
            Assert.AreEqual(0, calc.Add(1, -1));
            Assert.AreEqual(1, calc.Add(1, 0));
        }

        [TestMethod]
        public void CanVisitExpression() {
            Expression<Func<double, double>> eq = x => 2 * x + 5;

            var visitor = new StringifierVisitor();
            var actual = visitor.Accept(eq.Body);

            Assert.IsNotNull(eq);
            Assert.AreEqual("((2 * x) + 5)", actual);
        }
    }
}
