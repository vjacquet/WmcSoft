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
using System.Diagnostics;
using System.Linq;
using WmcSoft.Units;

namespace WmcSoft.Business.Accounting
{
    /// <summary>
    /// Represents an amount of a specific <see cref="Currency"/>. 
    /// This <see cref="Currency"/> is acceptedIn one or more <see cref="System.Globalization.CultureInfo"/>.
    /// </summary>
    [DebuggerDisplay("{Currency.ThreeLetterISOCode,nq} {Amount,nq}")]
    public struct Money : IComparable<Money>, IComparable, IEquatable<Money>
    {
        #region Lifecycle

        public Money(Currency currency)
            : this(Decimal.Zero, currency) {
        }

        public Money(decimal amount, Currency currency) {
            if (currency == null)
                throw new ArgumentNullException("currency");
            Amount = amount;
            Currency = currency;
        }

        #endregion

        #region Properties

        public decimal Amount { get; }

        public Currency Currency { get; }

        #endregion

        #region Methods

        public Money Round(RoundingPolicy policy) {
            if (policy == null)
                throw new ArgumentNullException("policy");

            return new Money(policy.Round(Amount), Currency);
        }

        public decimal[] AllocateAmounts(int n) {
            var decimalDigits = Currency.DecimalDigits;

            var lo = Floor(Amount / n, decimalDigits);
            var hi = Ceiling(Amount / n, decimalDigits);
            int remainder = (int)(Amount % n);
            var results = new decimal[n];
            var i = 0;
            for (i = 0; i < remainder; i++)
                results[i] = hi;
            for (; i < n; i++)
                results[i] = lo;
            return results;
        }

        public decimal[] AllocateAmounts(params int[] ratios) {
            var decimalDigits = Currency.DecimalDigits;
            var espilon = new Decimal(System.Math.Pow(0.1, decimalDigits));

            var total = ratios.Sum();
            var remainder = Amount;
            var results = new decimal[ratios.Length];
            for (int i = 0; i < results.Length; i++) {
                results[i] = Floor(Amount * ratios[i] / total, decimalDigits);
                remainder -= results[i];
            }
            for (int i = 0; remainder > 0m; i++) {
                results[i] += espilon;
                remainder -= espilon;
            }
            return results;
        }

        public Money[] Allocate(int n) {
            var currency = Currency;
            return AllocateAmounts(n).ConvertAll(a => new Money(a, currency));
        }

        public Money[] Allocate(params int[] ratios) {
            var currency = Currency;
            return AllocateAmounts(ratios).ConvertAll(a => new Money(a, currency));
        }

        #endregion

        #region Operators

        public static explicit operator decimal(Money that) {
            return that.Amount;
        }

        public static Money operator +(Money value, Money money) {
            if (value.Currency != money.Currency)
                throw new IncompatibleCurrencyException();
            return new Money(value.Amount + money.Amount, value.Currency);
        }

        public static Money Add(Money value, Money money) {
            return value + money;
        }

        public static Money operator -(Money value, Money money) {
            if (value.Currency != money.Currency)
                throw new IncompatibleCurrencyException();
            return new Money(value.Amount - money.Amount, value.Currency);
        }

        public static Money Subtract(Money value, Money money) {
            return value - money;
        }

        public static Money operator *(Money value, decimal multiplier) {
            return new Money(multiplier * value.Amount, value.Currency);
        }

        public static Money Multiply(Money value, decimal multiplier) {
            return value * multiplier;
        }

        public static bool operator >=(Money left, Money right) {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator >(Money left, Money right) {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Money left, Money right) {
            if (((object)left) != null)
                return left.CompareTo(right) <= 0;
            return false;
        }

        public static bool operator <(Money left, Money right) {
            return left.CompareTo(right) < 0;
        }

        public static bool operator ==(Money left, Money right) {
            return left.CompareTo(right) == 0;
        }

        public static bool operator !=(Money left, Money right) {
            return left.CompareTo(right) != 0;
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;
            return this.CompareTo((Money)obj) == 0;
        }

        public override int GetHashCode() {
            return this.Amount.GetHashCode() ^ this.Currency.GetHashCode();
        }

        #endregion

        #region Membres de IComparable

        int IComparable.CompareTo(object obj) {
            if (obj == null)
                return 1;
            return CompareTo((Money)obj);
        }

        #endregion

        #region IEquatable<Money> Membres

        public bool Equals(Money other) {
            if (Currency != other.Currency)
                return false;
            return CompareTo(other) == 0;
        }

        #endregion

        #region IComparable<Money> Membres

        public int CompareTo(Money other) {
            // TODO: attempt to convert before deciding...
            if (this.Currency != other.Currency)
                throw new IncompatibleCurrencyException();
            return Amount.CompareTo(other.Amount);
        }

        #endregion

        #region Round

        static decimal GetAwayFromZeroBoundary(int decimalDigits) {
            if (decimalDigits == 2)
                return 0.005m;
            var boundary = 0.5m / (decimal)Math.Pow(10, decimalDigits);
            return boundary;
        }

        static decimal GetTowardsZeroBoundary(int decimalDigits) {
            if (decimalDigits == 2)
                return 0.004m;
            var boundary = 0.4m / (decimal)Math.Pow(10, decimalDigits);
            return boundary;
        }

        static decimal Floor(decimal amount, int decimalDigits) {
            if (decimalDigits == 0)
                return Decimal.Floor(amount);
            var boundary = (amount >= 0)
                ? GetAwayFromZeroBoundary(decimalDigits)
                : GetTowardsZeroBoundary(decimalDigits);
            return Decimal.Round(amount - boundary, decimalDigits, MidpointRounding.AwayFromZero);
        }

        public static Money Floor(Money money) {
            var decimalDigits = money.Currency.DecimalDigits;
            var amount = Floor(money.Amount, decimalDigits);
            return new Money(amount, money.Currency);
        }

        static decimal Ceiling(decimal amount, int decimalDigits) {
            if (decimalDigits == 0)
                return Decimal.Ceiling(amount);
            var boundary = (amount >= 0)
                ? GetTowardsZeroBoundary(decimalDigits)
                : GetAwayFromZeroBoundary(decimalDigits);
            return Decimal.Round(amount + boundary, decimalDigits, MidpointRounding.AwayFromZero);
        }

        public static Money Ceiling(Money money) {
            var decimalDigits = money.Currency.DecimalDigits;
            var amount = Ceiling(money.Amount, decimalDigits);
            return new Money(amount, money.Currency);
        }

        public static Money Round(Money money, MidpointRounding mode) {
            var decimalDigits = money.Currency.DecimalDigits;
            var amount = Decimal.Round(money.Amount, decimalDigits, mode);
            return new Money(amount, money.Currency);
        }

        public static Money Round(Money money) {
            var decimalDigits = money.Currency.DecimalDigits;
            var amount = Decimal.Round(money.Amount, decimalDigits, MidpointRounding.AwayFromZero);
            return new Money(amount, money.Currency);
        }

        #endregion
    }

    /// <summary>
    /// Represents an amount of a specific <see cref="Currency"/>. Strongly typed version.
    /// This <see cref="Currency"/> is acceptedIn one or more <see cref="System.Globalization.CultureInfo"/>.
    /// </summary>
    public struct Money<C> : IComparable<Money<C>>, IComparable, IEquatable<Money<C>>
        where C : Currency, new()
    {
        #region Fields

        readonly decimal _amount;

        #endregion

        #region Lifecycle

        public Money(decimal amount) {
            _amount = amount;
        }

        #endregion

        #region Operators

        public static implicit operator Money<C>(int value) {
            return new Money<C>(value);
        }
        public static implicit operator Money<C>(double value) {
            return new Money<C>((decimal)value);
        }
        public static implicit operator Money<C>(decimal value) {
            return new Money<C>(value);
        }

        public static implicit operator Quantity(Money<C> that) {
            return new Quantity(that._amount, new C());
        }

        public static explicit operator decimal(Money<C> that) {
            return that._amount;
        }

        public static Money<C> operator +(Money<C> x, Money<C> y) {
            return new Money<C>(x._amount + y._amount);
        }

        public static Money<C> Add(Money<C> x, Money<C> y) {
            return x + y;
        }

        public static Money<C> operator -(Money<C> x, Money<C> y) {
            return new Money<C>(x._amount - y._amount);
        }

        public static Money<C> Subtract(Money<C> x, Money<C> y) {
            return x - y;
        }

        public static Money<C> operator *(decimal multiplier, Money<C> value) {
            return new Money<C>(multiplier * value._amount);
        }

        public static Money<C> Multiply(decimal multiplier, Money<C> value) {
            return value * multiplier;
        }

        public static Money<C> operator *(Money<C> value, decimal multiplier) {
            return new Money<C>(multiplier * value._amount);
        }

        public static Money<C> Multiply(Money<C> value, decimal multiplier) {
            return value * multiplier;
        }

        public static bool operator >=(Money<C> x, Money<C> y) {
            return x.CompareTo(y) >= 0;
        }

        public static bool operator >(Money<C> x, Money<C> y) {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <=(Money<C> x, Money<C> y) {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator <(Money<C> x, Money<C> y) {
            return x.CompareTo(y) < 0;
        }

        public static bool operator ==(Money<C> x, Money<C> y) {
            return x.CompareTo(y) == 0;
        }

        public static bool operator !=(Money<C> x, Money<C> y) {
            return x.CompareTo(y) != 0;
        }

        #endregion

        #region IComparable<Quantity<M>> Members

        public int CompareTo(Money<C> other) {
            return _amount.CompareTo(other._amount);
        }

        #endregion

        #region IEquatable<Quantity<M>> Members

        public bool Equals(Money<C> other) {
            return CompareTo(other) == 0;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return 1;
            var other = (Money<C>)obj;
            return _amount.CompareTo(other._amount);
        }

        #endregion

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var that = (Money<C>)obj;
            return (that == this);
        }

        public override int GetHashCode() {
            return _amount.GetHashCode();
        }
    }
}
