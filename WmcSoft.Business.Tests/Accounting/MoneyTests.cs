using System;
using System.Globalization;
using Xunit;

namespace WmcSoft.Business.Accounting
{
    public class MoneyTests
    {
        [Fact]
        public void CanCreateEuroCurrency()
        {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));

            Assert.Equal("EUR", eur.ThreeLetterISOCode);
            Assert.Equal(2, eur.DecimalDigits);
        }

        [Fact]
        public void CanCeilingMoney()
        {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            Assert.Equal(2, eur.DecimalDigits);

            var amount = 1.05m;
            for (int i = 0; i < 10; i++) {
                amount += 0.001m;

                var money = new Money(amount, eur);

                var ceiling = Money.Ceiling(money);
                Assert.True(1.06m == ceiling.Amount, $"Value was {amount}");
            }
        }

        [Fact]
        public void CanCeilingMoneyWithNegativeAmounts()
        {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            Assert.Equal(2, eur.DecimalDigits);

            var amount = -1.05m;
            for (int i = 0; i < 10; i++) {
                amount += 0.001m;

                var money = new Money(amount, eur);

                var ceiling = Money.Ceiling(money);
                Assert.True(-1.04m == ceiling.Amount, $"Value was {amount}");
            }
        }

        [Fact]
        public void CanFloorMoney()
        {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            Assert.Equal(2, eur.DecimalDigits);

            var amount = 1.05m;
            for (int i = 0; i < 10; i++) {
                var money = new Money(amount, eur);

                var floor = Money.Floor(money);
                Assert.True(1.05m == floor.Amount, $"Value was {amount}");

                amount += 0.001m;
            }
        }

        [Fact]
        public void CanFloorMoneyWithNegativeAmounts()
        {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            Assert.Equal(2, eur.DecimalDigits);

            var amount = -1.05m;
            for (int i = 0; i < 10; i++) {
                var money = new Money(amount, eur);

                var floor = Money.Floor(money);
                Assert.True(-1.05m == floor.Amount, $"Value was {amount}");

                amount += 0.001m;
            }
        }

        [Fact]
        public void CanAllocate3()
        {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(100, eur);
            var actual = m.AllocateAmounts(3);
            var expected = new[] { 33.34m, 33.33m, 33.33m };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAllocate3Shares()
        {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(100, eur);
            var actual = m.AllocateAmounts(3);
            var expected = new[] { 33.34m, 33.33m, 33.33m };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanSolveFoemmelsConundrum()
        {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(0.05m, eur);
            var actual = m.AllocateAmounts(3, 7);
            var expected = new[] { 0.02m, 0.03m };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanAddAmountInSameCurrency()
        {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m1 = new Money(1m, eur);
            var m2 = new Money(2m, eur);
            var actual = m1 + m2;
            var expected = new Money(3m, eur);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CannotAddAmountInDifferentCurrencies()
        {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m1 = new Money(1m, eur);
            var usd = new CultureInfoCurrency(CultureInfo.GetCultureInfo("en-US"));
            var m2 = new Money(2m, usd);
            Assert.Throws<IncompatibleCurrencyException>(() => m1 + m2);
        }

        [Fact]
        public void CanApplyFactorToMoney()
        {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(2m, eur);
            var alpha = 5m;

            var expected = new Money(10m, eur);
            Assert.Equal(expected, m * alpha);
            Assert.Equal(expected, alpha * m);
        }

        [Fact]
        public void CanDivideMoneyWithFactor()
        {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(6m, eur);
            var alpha = 3m;

            var expected = new Money(2m, eur);
            Assert.Equal(expected, m / alpha);
        }

        [Fact]
        public void CanDivideMoneyWithMoney()
        {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var dividend = new Money(6m, eur);
            var divisor = new Money(3m, eur);

            var expected = 2m;
            Assert.Equal(expected, dividend / divisor);
        }

        [Fact]
        public void CanAddAmountInSameStrongCurrency()
        {
            Money<EUR> m1 = 1m;
            Money<EUR> m2 = 2m;
            Money<EUR> expected = 3m;
            Assert.Equal(expected, m1 + m2);

            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(2m, eur);
            var actual = m1 + m;
            Assert.IsType(typeof(Money<EUR>), actual);
            Assert.Equal(expected, actual);
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
        public override int DecimalDigits => 2;
        public override string Name => "Euro";
        public override string ThreeLetterISOCode => "EUR";
        public override string Symbol => "€";
        public override string Definition => throw new NotImplementedException();
        public override int NumericCode => throw new NotImplementedException();
        public override RoundingMode Rounding => throw new NotImplementedException();
    }

    public class USD : Currency
    {
        public override int DecimalDigits => 2;
        public override string Name => "Dollar";
        public override string ThreeLetterISOCode => "USD";
        public override string Symbol => "$";
        public override string Definition => throw new NotImplementedException();
        public override int NumericCode => throw new NotImplementedException();
        public override RoundingMode Rounding => throw new NotImplementedException();
    }
}