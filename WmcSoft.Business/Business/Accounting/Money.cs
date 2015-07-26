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
using System.Threading.Tasks;
using WmcSoft.Units;

namespace WmcSoft.Business.Accounting
{
    /// <summary>
    /// Represents an amount of a specific Currency. This Currency is acceptedIn one or more CultureInfo.
    /// </summary>
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
    }
}
