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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Collections.Generic;
using WmcSoft.Units;

namespace WmcSoft.Business.Accounting
{
    /// <summary>
    /// Represents an amount of a specific Currency. This Currency is acceptedIn one or more CultureInfo.
    /// </summary>
    [DebuggerDisplay("{Currency.ThreeLetterISOCode,nq} {Amount,nq}")]
    public struct Money : IComparable<Money>, IComparable, IEquatable<Money>
    {
        #region Fields

        readonly decimal _amount;
        readonly Currency _currency;

        #endregion

        #region Lifecycle

        public Money(Currency currency)
            : this(Decimal.Zero, currency) {
        }

        public Money(decimal amount, Currency currency) {
            if (currency == null)
                throw new ArgumentNullException("currency");
            _amount = amount;
            _currency = currency;
        }

        #endregion

        #region Properties

        public decimal Amount {
            get { return _amount; }
        }

        public Currency Currency {
            get { return _currency; }
        }

        #endregion

        #region Methods

        public Money Round(RoundingPolicy policy) {
            if (policy == null)
                throw new ArgumentNullException("policy");

            return new Money(policy.Round(_amount), _currency);
        }

        public decimal[] AllocateAmounts(int n) {
            var decimalDigits = Currency.DecimalDigits;

            var lo = Floor(_amount / n, decimalDigits);
            var hi = Ceiling(_amount / n, decimalDigits);
            int remainder = (int)(_amount % n);
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
            var remainder = _amount;
            var results = new decimal[ratios.Length];
            for (int i = 0; i < results.Length; i++) {
                results[i] = Floor(_amount * ratios[i] / total, decimalDigits);
                remainder -= results[i];
            }
            for (int i = 0; remainder > 0m; i++) {
                results[i] += espilon;
                remainder -= espilon;
            }
            return results;
        }

        public Money[] Allocate(int n) {
            var currency = _currency;
            return AllocateAmounts(n).ConvertAll(a => new Money(a, currency));
        }

        public Money[] Allocate(params int[] ratios) {
            var currency = _currency;
            return AllocateAmounts(ratios).ConvertAll(a => new Money(a, currency));
        }

        #endregion

        #region Operators

        public static explicit operator decimal(Money that) {
            return that._amount;
        }

        public static Money operator +(Money value, Money money) {
            if (value.Currency != money.Currency)
                throw new IncompatibleCurrencyException();
            return new Money(value._amount + money._amount, value.Currency);
        }

        public static Money Add(Money value, Money money) {
            return value + money;
        }

        public static Money operator -(Money value, Money money) {
            if (value.Currency != money.Currency)
                throw new IncompatibleCurrencyException();
            return new Money(value._amount - money._amount, value.Currency);
        }

        public static Money Subtract(Money value, Money money) {
            return value - money;
        }

        public static Money operator *(Money value, decimal multiplier) {
            return new Money(multiplier * value._amount, value.Currency);
        }

        public static Money Multiply(Money value, decimal multiplier) {
            return value * multiplier;
        }

        public static Quantity operator /(Money value, Quantity quantity) {
            throw new NotImplementedException();

            //int count = 1;
            //int index = 0;

            //if (quantity.Metric is DerivedUnit) {
            //    count += ((DerivedUnit)quantity.Metric).Terms.Length;
            //} else {
            //    count += 1;
            //}
            //DerivedUnitTerm[] terms = new DerivedUnitTerm[count];
            //terms[index++] = new DerivedUnitTerm((Unit)value.Currency, 1);

            //if (quantity.Metric is DerivedUnit) {
            //    DerivedUnitTerm term;
            //    for (int i = 0; i < ((DerivedUnit)value.Metric).Terms.Length; i++) {
            //        term = ((DerivedUnit)value.Metric).Terms[i];
            //        terms[index++] = new DerivedUnitTerm(term.Unit, -term.Power);
            //    }
            //    ((DerivedUnit)value.Metric).Terms.CopyTo(terms, index);
            //} else {
            //    terms[index++] = new DerivedUnitTerm((Unit)quantity.Metric, -1);
            //}

            //return new Quantity(value._amount * quantity.Amount, new DerivedUnit(terms));
        }

        public static Quantity Divide(Money value, Quantity quantity) {
            return value / quantity;
        }

        public static Money operator /(Money value, decimal divider) {
            return new Money(value._amount / divider, value.Currency);
        }

        public static Money Divide(Money value, decimal divider) {
            return value / divider;
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
            return this._amount.GetHashCode() ^ this._currency.GetHashCode();
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
            if (this.Currency != other.Currency)
                return false;
            return CompareTo(other) == 0;
        }

        #endregion

        #region IComparable<Money> Membres

        public int CompareTo(Money other) {
            // TODO: attempt to convert before deciding...
            if (this.Currency != other.Currency)
                throw new IncompatibleCurrencyException();
            return _amount.CompareTo(other._amount);
        }

        #endregion

        #region Round

        static decimal GetAwayFromZeroBoundary(int decimalDigits) {
            if (decimalDigits == 2)
                return 0.005m;
            var boundary = 0.5m / new Decimal(System.Math.Pow(10, decimalDigits));
            return boundary;
        }

        static decimal GetTowardsZeroBoundary(int decimalDigits) {
            if (decimalDigits == 2)
                return 0.004m;
            var boundary = 0.4m / new Decimal(System.Math.Pow(10, decimalDigits));
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
}
