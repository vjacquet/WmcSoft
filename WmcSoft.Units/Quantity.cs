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

namespace WmcSoft.Units
{
    /// <summary>
    /// Represents an amount measured in some Metric
    /// </summary>
    public struct Quantity : IComparable<Quantity>, IComparable, IEquatable<Quantity>
    {
        #region Fields

        readonly decimal _amount;
        readonly Metric _metric;

        #endregion

        #region Lifecycle

        public Quantity(Metric metric)
            : this(Decimal.Zero, metric) {
        }

        public Quantity(decimal amount, Metric metric) {
            if (metric == null)
                throw new ArgumentNullException("metric");
            _amount = amount;
            _metric = metric;
        }

        #endregion

        #region Properties

        public decimal Amount {
            get { return _amount; }
        }

        public Metric Metric {
            get { return _metric; }
        }

        #endregion

        #region Methods

        public Quantity Round(RoundingPolicy policy) {
            if (policy == null)
                throw new ArgumentNullException("policy");

            return new Quantity(policy.Round(_amount), _metric);
        }

        #endregion

        #region Operators

        public static implicit operator decimal(Quantity that) {
            return that._amount;
        }

        public static Quantity operator +(Quantity x, Quantity y) {
            if (x.Metric != y.Metric)
                throw new IncompatibleMetricException();
            return new Quantity(x._amount + y._amount, x.Metric);
        }

        public static Quantity Add(Quantity x, Quantity y) {
            return x + y;
        }

        public static Quantity operator -(Quantity x, Quantity y) {
            if (x.Metric != y.Metric)
                throw new IncompatibleMetricException();
            return new Quantity(x._amount - y._amount, x.Metric);
        }

        public static Quantity Subtract(Quantity x, Quantity y) {
            return x - y;
        }

        public static Quantity operator *(Quantity x, Quantity y) {
            int count = 0;
            int index = 0;
            if (x.Metric is DerivedUnit) {
                count += ((DerivedUnit)x.Metric).Terms.Length;
            } else {
                count += 1;
            }
            if (y.Metric is DerivedUnit) {
                count += ((DerivedUnit)y.Metric).Terms.Length;
            } else {
                count += 1;
            }
            DerivedUnitTerm[] terms = new DerivedUnitTerm[count];
            if (x.Metric is DerivedUnit) {
                ((DerivedUnit)x.Metric).Terms.CopyTo(terms, index);
                index += ((DerivedUnit)x.Metric).Terms.Length;
            } else {
                terms[index++] = new DerivedUnitTerm((Unit)x.Metric, 1);
            }
            if (y.Metric is DerivedUnit) {
                ((DerivedUnit)x.Metric).Terms.CopyTo(terms, index);
            } else {
                terms[index++] = new DerivedUnitTerm((Unit)y.Metric, 1);
            }

            return new Quantity(x._amount * y._amount, new DerivedUnit(terms));
        }

        public static Quantity Multiply(Quantity x, Quantity y) {
            return x * y;
        }

        public static Quantity operator *(Quantity value, decimal multiplier) {
            return new Quantity(multiplier * value._amount, value.Metric);
        }

        public static Quantity Multiply(Quantity value, decimal multiplier) {
            return value * multiplier;
        }

        public static Quantity operator /(Quantity x, Quantity y) {
            int count = 0;
            int index = 0;
            if (x.Metric is DerivedUnit) {
                count += ((DerivedUnit)x.Metric).Terms.Length;
            } else {
                count += 1;
            }
            if (y.Metric is DerivedUnit) {
                count += ((DerivedUnit)y.Metric).Terms.Length;
            } else {
                count += 1;
            }
            DerivedUnitTerm[] terms = new DerivedUnitTerm[count];
            if (x.Metric is DerivedUnit) {
                ((DerivedUnit)x.Metric).Terms.CopyTo(terms, index);
                index += ((DerivedUnit)x.Metric).Terms.Length;
            } else {
                terms[index++] = new DerivedUnitTerm((Unit)x.Metric, 1);
            }
            if (y.Metric is DerivedUnit) {
                DerivedUnitTerm term;
                for (int i = 0; i < ((DerivedUnit)x.Metric).Terms.Length; i++) {
                    term = ((DerivedUnit)x.Metric).Terms[i];
                    terms[index++] = new DerivedUnitTerm(term.Unit, -term.Power);
                }
                ((DerivedUnit)x.Metric).Terms.CopyTo(terms, index);
            } else {
                terms[index++] = new DerivedUnitTerm((Unit)y.Metric, -1);
            }

            return new Quantity(x._amount * y._amount, new DerivedUnit(terms));
        }

        public static Quantity Divide(Quantity x, Quantity y) {
            return x / y;
        }

        public static Quantity operator /(Quantity value, decimal divider) {
            return new Quantity(value._amount / divider, value.Metric);
        }

        public static Quantity Divide(Quantity value, decimal divider) {
            return value / divider;
        }

        public static bool operator >=(Quantity x, Quantity y) {
            if (((object)x) != null)
                return x.CompareTo(y) >= 0;
            else if (y == null)
                return true;
            return false;
        }

        public static bool operator >(Quantity x, Quantity y) {
            if (((object)x) != null)
                return x.CompareTo(y) > 0;
            return false;
        }

        public static bool operator <=(Quantity x, Quantity y) {
            if (((object)x) != null)
                return x.CompareTo(y) <= 0;
            return false;
        }

        public static bool operator <(Quantity x, Quantity y) {
            if (((object)x) != null)
                return x.CompareTo(y) < 0;
            else if (y != null)
                return true;
            return false;
        }

        public static bool operator ==(Quantity x, Quantity y) {
            if (((object)x) != null)
                return ((IComparable)x).CompareTo(y) == 0;
            return (((object)y) == null);
        }

        public static bool operator !=(Quantity x, Quantity y) {
            if (((object)x) != null)
                return x.CompareTo(y) != 0;
            return (((object)y) != null);
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;
            return this.CompareTo((Quantity)obj) == 0;
        }

        public override int GetHashCode() {
            return this._amount.GetHashCode() ^ this._metric.GetHashCode();
        }

        #endregion

        #region Membres de IComparable

        int IComparable.CompareTo(object obj) {
            if (obj == null)
                return 1;
            return CompareTo((Quantity)obj);
        }

        #endregion

        #region IEquatable<Quantity> Membres

        public bool Equals(Quantity other) {
            return CompareTo(other) == 0;
        }

        #endregion

        #region IComparable<Quantity> Membres

        public int CompareTo(Quantity other) {
            // TODO: attempt to convert before deciding...
            if (this.Metric != other.Metric)
                throw new IncompatibleMetricException();
            return _amount.CompareTo(other._amount);
        }

        #endregion

    }

    /// <summary>
    /// Represents an amount measured in some Metric. Strongly typed version.
    /// </summary>
    /// <typeparam name="M">The metric</typeparam>
    public struct Quantity<M> : IComparable<Quantity<M>>, IComparable, IEquatable<Quantity<M>> where M : Metric
    {
        #region Fields

        readonly decimal _amount;

        #endregion

        #region Lifecycle

        public Quantity(decimal amount) {
            _amount = amount;
        }

        #endregion

        #region Operators

        public static implicit operator Quantity<M>(int value) {
            return new Quantity<M>(value);
        }
        public static implicit operator Quantity<M>(double value) {
            return new Quantity<M>((decimal)value);
        }
        public static implicit operator Quantity<M>(decimal value) {
            return new Quantity<M>(value);
        }

        public static implicit operator Quantity(Quantity<M> that) {
            return new Quantity(that._amount, Activator.CreateInstance<M>());
        }

        public static explicit operator decimal(Quantity<M> that) {
            return that._amount;
        }

        public static Quantity<M> operator +(Quantity<M> x, Quantity<M> y) {
            return new Quantity<M>(x._amount + y._amount);
        }

        public static Quantity<M> Add(Quantity<M> x, Quantity<M> y) {
            return x + y;
        }

        public static Quantity<M> operator -(Quantity<M> x, Quantity<M> y) {
            return new Quantity<M>(x._amount - y._amount);
        }

        public static Quantity<M> Subtract(Quantity<M> x, Quantity<M> y) {
            return x - y;
        }

        public static Quantity<M> operator *(decimal multiplier, Quantity<M> value) {
            return new Quantity<M>(multiplier * value._amount);
        }

        public static Quantity<M> Multiply(decimal multiplier, Quantity<M> value) {
            return value * multiplier;
        }

        public static Quantity<M> operator *(Quantity<M> value, decimal multiplier) {
            return new Quantity<M>(multiplier * value._amount);
        }

        public static Quantity<M> Multiply(Quantity<M> value, decimal multiplier) {
            return value * multiplier;
        }

        public static Quantity operator *(Quantity<M> x, Quantity y) {
            return ((Quantity)x) * y;
        }

        public static Quantity Multiply(Quantity<M> x, Quantity y) {
            return x * y;
        }

        public static Quantity<M> operator /(Quantity<M> value, decimal divider) {
            return new Quantity<M>(value._amount / divider);
        }

        public static Quantity<M> Divide(Quantity<M> value, decimal divider) {
            return value / divider;
        }

        public static Quantity operator /(decimal divider, Quantity<M> value) {
            return new Quantity(divider / value._amount, new DerivedUnit(new DerivedUnitTerm((Unit)Activator.CreateInstance(typeof(M)), -1)));
        }

        public static Quantity Divide(decimal divider, Quantity<M> value) {
            return divider / value;
        }

        public static Quantity operator /(Quantity<M> x, Quantity y) {
            return ((Quantity)x) / y;
        }

        public static Quantity Divide(Quantity<M> x, Quantity y) {
            return x / y;
        }

        public static bool operator >=(Quantity<M> x, Quantity<M> y) {
            return x.CompareTo(y) >= 0;
        }

        public static bool operator >(Quantity<M> x, Quantity<M> y) {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <=(Quantity<M> x, Quantity<M> y) {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator <(Quantity<M> x, Quantity<M> y) {
            return x.CompareTo(y) < 0;
        }

        public static bool operator ==(Quantity<M> x, Quantity<M> y) {
            return x.CompareTo(y) == 0;
        }

        public static bool operator !=(Quantity<M> x, Quantity<M> y) {
            return x.CompareTo(y) != 0;
        }

        #endregion

        #region IComparable<Quantity<M>> Members

        public int CompareTo(Quantity<M> other) {
            return _amount.CompareTo(other._amount);
        }

        #endregion

        #region IEquatable<Quantity<M>> Members

        public bool Equals(Quantity<M> other) {
            return CompareTo(other) == 0;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return 1;
            var other = (Quantity<M>)obj;
            return _amount.CompareTo(other._amount);
        }

        #endregion

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var that = (Quantity<M>)obj;
            return (that == this);
        }

        public override int GetHashCode() {
            return _amount.GetHashCode();
        }
    }
}
