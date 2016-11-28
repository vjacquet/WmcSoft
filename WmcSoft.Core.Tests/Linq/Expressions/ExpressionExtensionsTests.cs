using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace WmcSoft.Linq.Expressions
{
    [TestClass]
    public class ExpressionExtensionsTests
    {
        class AskMe
        {
            readonly StringBuilder _sb;

            public AskMe() {
                _sb = new StringBuilder();
            }

            bool Say(string label, bool value) {
                if (_sb.Length != 0)
                    _sb.Append(" then ");
                _sb.Append(label).Append(" is ").Append(value);
                return value;
            }

            public bool SayTrue<T>(string label, T x) {
                return Say(label, true);
            }

            public bool SayFalse<T>(string label, T x) {
                return Say(label, false);
            }

            public override string ToString() {
                return _sb.ToString();
            }
        }

        [TestMethod]
        public void CanComposeTwoPredicate() {
            var expected = new AskMe();
            var actual = new AskMe();
            Func<int, bool> reference = p => expected.SayTrue("a", p) && expected.SayTrue("b", p);
            Expression<Func<int, bool>> a = p => actual.SayTrue("a", p);
            Expression<Func<int, bool>> b = p => actual.SayTrue("b", p);

            Assert.AreEqual(reference(1), a.AndAlso(b).Compile()(1));
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void CheckConjunctionOnTruePredicates() {
            var expected = new AskMe();
            var actual = new AskMe();
            Func<int, bool> reference = p => expected.SayTrue("a", p) && expected.SayTrue("b", p) && expected.SayTrue("c", p) && expected.SayTrue("d", p) && expected.SayTrue("e", p) && expected.SayTrue("f", p);
            Expression<Func<int, bool>> a = p => actual.SayTrue("a", p);
            Expression<Func<int, bool>> b = p => actual.SayTrue("b", p);
            Expression<Func<int, bool>> c = p => actual.SayTrue("c", p);
            Expression<Func<int, bool>> d = p => actual.SayTrue("d", p);
            Expression<Func<int, bool>> e = p => actual.SayTrue("e", p);
            Expression<Func<int, bool>> f = p => actual.SayTrue("f", p);
            var parameters = new[] { a, b, c, d,e,f };

            Assert.AreEqual(reference(1), parameters.Conjunction().Compile()(1));
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void CheckConjunctionOnDifferentPredicates() {
            var expected = new AskMe();
            var actual = new AskMe();
            Func<int, bool> reference = p => expected.SayTrue("a", p) && expected.SayTrue("b", p) && expected.SayFalse("c", p) && expected.SayTrue("d", p) && expected.SayTrue("e", p) && expected.SayTrue("f", p);
            Expression<Func<int, bool>> a = p => actual.SayTrue("a", p);
            Expression<Func<int, bool>> b = p => actual.SayTrue("b", p);
            Expression<Func<int, bool>> c = p => actual.SayFalse("c", p);
            Expression<Func<int, bool>> d = p => actual.SayFalse("d", p);
            Expression<Func<int, bool>> e = p => actual.SayTrue("e", p);
            Expression<Func<int, bool>> f = p => actual.SayTrue("f", p);
            var parameters = new[] { a, b, c, d, e, f };

            Assert.AreEqual(reference(1), parameters.Conjunction().Compile()(1));
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void CheckDisjunctionOnFalsePredicates() {
            var expected = new AskMe();
            var actual = new AskMe();
            Func<int, bool> reference = p => expected.SayFalse("a", p) || expected.SayFalse("b", p) || expected.SayFalse("c", p) || expected.SayFalse("d", p) || expected.SayFalse("e", p) || expected.SayFalse("f", p);
            Expression<Func<int, bool>> a = p => actual.SayFalse("a", p);
            Expression<Func<int, bool>> b = p => actual.SayFalse("b", p);
            Expression<Func<int, bool>> c = p => actual.SayFalse("c", p);
            Expression<Func<int, bool>> d = p => actual.SayFalse("d", p);
            Expression<Func<int, bool>> e = p => actual.SayFalse("e", p);
            Expression<Func<int, bool>> f = p => actual.SayFalse("f", p);
            var parameters = new[] { a, b, c, d, e, f };

            Assert.AreEqual(reference(1), parameters.Disjunction().Compile()(1));
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void CheckDisjunctionOnDifferentPredicates() {
            var expected = new AskMe();
            var actual = new AskMe();
            Func<int, bool> reference = p => expected.SayFalse("a", p) || expected.SayFalse("b", p) || expected.SayFalse("c", p) || expected.SayTrue("d", p) || expected.SayFalse("e", p) || expected.SayFalse("f", p);
            Expression<Func<int, bool>> a = p => actual.SayFalse("a", p);
            Expression<Func<int, bool>> b = p => actual.SayFalse("b", p);
            Expression<Func<int, bool>> c = p => actual.SayFalse("c", p);
            Expression<Func<int, bool>> d = p => actual.SayTrue("d", p);
            Expression<Func<int, bool>> e = p => actual.SayFalse("e", p);
            Expression<Func<int, bool>> f = p => actual.SayFalse("f", p);
            var parameters = new[] { a, b, c, d, e, f };

            Assert.AreEqual(reference(1), parameters.Disjunction().Compile()(1));
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void CheckDisjunctionOnComplexPredicates() {
            var expected = new AskMe();
            var actual = new AskMe();
            Func<int, bool> reference = p => expected.SayFalse("a", p) || expected.SayFalse("b", p) || expected.SayFalse("c", p) || (expected.SayTrue("d", p) && expected.SayFalse("e", p)) || expected.SayFalse("f", p);
            Expression<Func<int, bool>> a = p => actual.SayFalse("a", p);
            Expression<Func<int, bool>> b = p => actual.SayFalse("b", p);
            Expression<Func<int, bool>> c = p => actual.SayFalse("c", p);
            Expression<Func<int, bool>> d = p => actual.SayTrue("d", p);
            Expression<Func<int, bool>> e = p => actual.SayFalse("e", p);
            Expression<Func<int, bool>> f = p => actual.SayFalse("f", p);
            var parameters = new[] { a, b, c, d.AndAlso(e), f };

            Assert.AreEqual(reference(1), parameters.Disjunction().Compile()(1));
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}
