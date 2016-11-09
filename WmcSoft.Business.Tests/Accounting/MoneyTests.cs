using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Business.Accounting
{
    [TestClass]
    public class MoneyTests
    {
        [TestMethod]
        public void CanCreateEuroCurrency() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));

            Assert.AreEqual("EUR", eur.ThreeLetterISOCode);
            Assert.AreEqual(2, eur.DecimalDigits);
        }

        [TestMethod]
        public void CanCeilingMoney() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            Assert.AreEqual(2, eur.DecimalDigits);

            var amount = 1.05m;
            for (int i = 0; i < 10; i++) {
                amount += 0.001m;

                var money = new Money(amount, eur);

                var ceiling = Money.Ceiling(money);
                Assert.AreEqual(1.06m, ceiling.Amount, "Value was {0}", amount);
            }
        }

        [TestMethod]
        public void CanCeilingMoneyWithNegativeAmounts() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            Assert.AreEqual(2, eur.DecimalDigits);

            var amount = -1.05m;
            for (int i = 0; i < 10; i++) {
                amount += 0.001m;

                var money = new Money(amount, eur);

                var ceiling = Money.Ceiling(money);
                Assert.AreEqual(-1.04m, ceiling.Amount, "Value was {0}", amount);
            }
        }

        [TestMethod]
        public void CanFloorMoney() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            Assert.AreEqual(2, eur.DecimalDigits);

            var amount = 1.05m;
            for (int i = 0; i < 10; i++) {
                var money = new Money(amount, eur);

                var floor = Money.Floor(money);
                Assert.AreEqual(1.05m, floor.Amount, "Value was {0}", amount);

                amount += 0.001m;
            }
        }

        [TestMethod]
        public void CanFloorMoneyWithNegativeAmounts() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            Assert.AreEqual(2, eur.DecimalDigits);

            var amount = -1.05m;
            for (int i = 0; i < 10; i++) {
                var money = new Money(amount, eur);

                var floor = Money.Floor(money);
                Assert.AreEqual(-1.05m, floor.Amount, "Value was {0}", amount);

                amount += 0.001m;
            }
        }

        [TestMethod]
        public void CanAllocate3() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(100, eur);
            var actual = m.AllocateAmounts(3);
            var expected = new[] { 33.34m, 33.33m, 33.33m };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanAllocate3Shares() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(100, eur);
            var actual = m.AllocateAmounts(3);
            var expected = new[] { 33.34m, 33.33m, 33.33m };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanSolveFoemmelsConundrum() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(0.05m, eur);
            var actual = m.AllocateAmounts(3, 7);
            var expected = new[] { 0.02m, 0.03m };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanAddAmountInSameCurrency() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m1 = new Money(1m, eur);
            var m2 = new Money(2m, eur);
            var actual = m1 + m2;
            var expected = new Money(3m, eur);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(IncompatibleCurrencyException))]
        public void CannotAddAmountInDifferentCurrencies() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m1 = new Money(1m, eur);
            var usd = new CultureInfoCurrency(CultureInfo.GetCultureInfo("en-US"));
            var m2 = new Money(2m, usd);
            var m3 = m1 + m2;
        }

        [TestMethod]
        public void CanApplyFactorToMoney() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(2m, eur);
            var alpha = 5m;

            var expected = new Money(10m, eur);
            Assert.AreEqual(expected, m * alpha);
            Assert.AreEqual(expected, alpha * m);
        }

        [TestMethod]
        public void CanDivideMoneyWithFactor() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(6m, eur);
            var alpha = 3m;

            var expected = new Money(2m, eur);
            Assert.AreEqual(expected, m / alpha);
        }

        [TestMethod]
        public void CanDivideMoneyWithMoney() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var dividend = new Money(6m, eur);
            var divisor = new Money(3m, eur);

            var expected = 2m;
            Assert.AreEqual(expected, dividend / divisor);
        }

        [TestMethod]
        public void CanAddAmountInSameStrongCurrency() {
            Money<EUR> m1 = 1m;
            Money<EUR> m2 = 2m;
            Money<EUR> expected = 3m;
            Assert.AreEqual(expected, m1 + m2);

            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(2m, eur);
            var actual = m1 + m;
            Assert.IsInstanceOfType(actual, typeof(Money<EUR>));
            Assert.AreEqual(expected, actual);
        }

#if CHECK_INVALID_OPERATION
        public void MustNotCompile() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(1m, eur);
            var alpha = 3m;

            var cannotMultiplyMoneyWithMoney = m * m;
            var cannotDivideAmountWithModeny = alpha / m;
        }
#endif
    }

    public class EUR : Currency
    {
        public override int DecimalDigits {
            get { return 2; }
        }

        public override string Name {
            get { return "Euro"; }
        }

        public override string ThreeLetterISOCode {
            get { return "EUR"; }
        }

        public override string Symbol {
            get { return "€"; }
        }

        public override string Definition {
            get { throw new NotImplementedException(); }
        }

        public override int NumericCode {
            get { throw new NotImplementedException(); }
        }

        public override RoundingMode Rounding {
            get { throw new NotImplementedException(); }
        }
    }

    public class USDCurrency : Currency
    {
        public override int DecimalDigits {
            get { return 2; }
        }

        public override string Name {
            get { return "Dollar"; }
        }

        public override string ThreeLetterISOCode {
            get { return "USD"; }
        }

        public override string Symbol {
            get { return "$"; }
        }

        public override string Definition {
            get { throw new NotImplementedException(); }
        }

        public override int NumericCode {
            get { throw new NotImplementedException(); }
        }

        public override RoundingMode Rounding {
            get { throw new NotImplementedException(); }
        }
    }
}