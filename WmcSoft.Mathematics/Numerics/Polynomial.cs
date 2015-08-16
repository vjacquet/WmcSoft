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
using WmcSoft.Collections.Generic;

namespace WmcSoft.Numerics
{
    public struct Polynomial : IEquatable<Polynomial>, IFormattable
    {
        public static Polynomial Zero;

        struct ExponentComparer : IComparer<Node>
        {
            #region IComparer<Node> Membres

            public int Compare(Node x, Node y) {
                return y.Exp - x.Exp;
            }

            #endregion
        }
        struct Node : IComparable<Node>, IEquatable<Node>, IFormattable
        {
            public readonly int Exp;
            public readonly double Coef;

            public Node(int exponent, double coefficient) {
                Exp = exponent;
                Coef = coefficient;
            }

            public Node(double coefficient) {
                Exp = 0;
                Coef = coefficient;
            }

            #region IComparable<Node> Membres

            public int CompareTo(Node other) {
                if (Exp == other.Exp)
                    return Comparer<double>.Default.Compare(Coef, other.Coef);
                return Exp - other.Exp;
            }

            #endregion

            #region IEquatable<Node> Membres

            public bool Equals(Node other) {
                return CompareTo(other) == 0;
            }

            public override bool Equals(object obj) {
                if (obj == null || GetType() != obj.GetType())
                    return false;
                return Equals((Node)obj);
            }

            public override int GetHashCode() {
                return (Coef.GetHashCode() * 397) ^ Exp;
            }
            #endregion

            #region IFormattable Membres

            public override string ToString() {
                return ToString(null, null);
            }
            public string ToString(IFormatProvider formatProvider) {
                return ToString(null, formatProvider);
            }
            public string ToString(string format, IFormatProvider formatProvider = null) {
                var one = 1d.ToString(format, formatProvider);
                return Format(Exp, Coef, format, formatProvider, one);
            }

            #endregion
        }

        #region Fields

        private readonly Node[] _nodes;

        #endregion

        #region Lifecycle

        Polynomial(Node[] nodes) {
            _nodes = nodes;
        }

        public Polynomial(params double[] coefficients) {
            if (coefficients == null || coefficients.Length == 0) {
                _nodes = null;
            } else {
                var length = coefficients.Length;
                int i = 0;
                while (i < length && coefficients[i] == 0d) {
                    i++;
                }

                var degree = length - i - 1;
                var list = new List<Node>(degree);
                for (; i < length; i++, degree--) {
                    if (coefficients[i] != 0d) {
                        list.Add(new Node(degree, coefficients[i]));
                    }
                }
                _nodes = list.ToArray();
            }
        }

        #endregion

        #region Properties

        public int Degree { get { return _nodes == null ? 0 : _nodes[_nodes.Length - 1].Exp; } }
        public double this[int index] { get { return _nodes.BinaryFind(d => d.Exp).Coef; } }

        #endregion

        #region Operators

        public static implicit operator Polynomial(double x) {
            return new Polynomial(x);
        }

        public static Polynomial operator +(Polynomial x, Polynomial y) {
            if (x._nodes == null)
                return y;
            if (y._nodes == null)
                return x;
            var comparer = new ExponentComparer();
            var nodes = x._nodes.Combine(y._nodes, comparer, (a, b) => new Node(a.Exp, a.Coef + b.Coef))
                .Where(n => n.Coef != 0);
            return new Polynomial(nodes.ToArray());
        }
        public static Polynomial Add(Polynomial x, Polynomial y) {
            return x + y;
        }

        public static Polynomial operator -(Polynomial x, Polynomial y) {
            if (x._nodes == null)
                return -y;
            if (y._nodes == null)
                return x;
            var comparer = new ExponentComparer();
            var nodes = x._nodes.Combine(y._nodes, comparer, (a, b) => new Node(a.Exp, a.Coef - b.Coef))
                .Where(n => n.Coef != 0);
            return new Polynomial(nodes.ToArray());
        }
        public static Polynomial Subtract(Polynomial x, Polynomial y) {
            return x - y;
        }

        public static Polynomial operator -(Polynomial x) {
            if (x._nodes == null)
                return x;
            var nodes = x._nodes.ToArray(n => new Node(n.Exp, -n.Coef));
            return new Polynomial(nodes);
        }
        public static Polynomial Negate(Polynomial x) {
            return -x;
        }

        public static Polynomial operator +(Polynomial x) {
            return x;
        }
        public static Polynomial Plus(Polynomial x) {
            return x;
        }

        public static Polynomial operator *(double alpha, Polynomial x) {
            if (x._nodes == null)
                return x;
            var nodes = x._nodes.ToArray(n => new Node(n.Exp, alpha * n.Coef));
            return new Polynomial(nodes);
        }
        public static Polynomial Multiply(double alpha, Polynomial x) {
            return alpha * x;
        }
        public static Polynomial operator *(Polynomial x, double alpha) {
            return alpha * x;
        }
        public static Polynomial Multiply(Polynomial x, double alpha) {
            return alpha * x;
        }

        public static Polynomial operator *(Polynomial x, Polynomial y) {
            if (x._nodes == null || y._nodes == null)
                return y;

            var m = x._nodes;
            var p = y._nodes;
            var max = m[0].Exp + p[0].Exp;
            var min = m[m.Length - 1].Exp + p[p.Length - 1].Exp;
            var length = max - min + 1;
            var coefficients = new double[length];

            for (int i = 0; i < m.Length; i++) {
                for (int j = 0; j < p.Length; j++) {
                    coefficients[max - m[i].Exp - p[j].Exp] += m[i].Coef * p[j].Coef;
                }
            }

            var degree = max;
            var list = new List<Node>(length);
            for (int i = 0; i < length; i++, degree--) {
                if (coefficients[i] != 0d) {
                    list.Add(new Node(degree, coefficients[i]));
                }
            }
            if (list.Count == 0)
                return Zero;
            var nodes = list.ToArray();
            return new Polynomial(nodes);
        }
        public static Polynomial Multiply(Polynomial x, Polynomial y) {
            return x * y;
        }

        #endregion

        #region IEquatable<Vector> Membres

        public bool Equals(Polynomial other) {
            if (_nodes == null)
                return other._nodes == null;
            if (other._nodes == null)
                return false;

            var length = _nodes.Length;
            if (other._nodes.Length != length)
                return false;
            for (int i = 0; i < length; i++) {
                if (_nodes[i].CompareTo(other._nodes[i]) != 0)
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Vector)obj);
        }

        public override int GetHashCode() {
            if (_nodes == null)
                return 0;
            return _nodes.GetHashCode();
        }

        #endregion

        #region IFormattable Membres

        public override string ToString() {
            return ToString(null, null);
        }
        public string ToString(IFormatProvider formatProvider) {
            return ToString(null, formatProvider);
        }
        public string ToString(string format, IFormatProvider formatProvider = null) {
            if (_nodes == null)
                return 0d.ToString(format, formatProvider);

            var one = 1d.ToString(format, formatProvider);
            var sb = new StringBuilder(_nodes[0].ToString(format, formatProvider));
            var length = _nodes.Length;
            for (int i = 1; i < length; i++) {
                var n = _nodes[i];
                if (n.Coef > 0d) {
                    sb.Append(" + ").Append(Format(n.Exp, n.Coef, format, formatProvider, one));
                } else if (n.Coef < 0d) {
                    sb.Append(" - ").Append(Format(n.Exp, -n.Coef, format, formatProvider, one));
                }
            }
            return sb.ToString();
        }

        static string Format(int exp, double coef, string format, IFormatProvider formatProvider, string one) {
            var coefficient = coef.ToString(format, formatProvider);
            if (exp == 0)
                return coefficient;
            if (coefficient == one)
                coefficient = "";
            if (exp == 1)
                return coefficient + "x";
            return coefficient + "x^" + exp;
        }

        #endregion
    }
}
