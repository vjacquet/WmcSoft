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

        public void CanSolveFoemmelsConundrum() {
            var eur = new CultureInfoCurrency(CultureInfo.GetCultureInfo("fr-FR"));
            var m = new Money(0.05m, eur);
            var actual = m.AllocateAmounts(3, 7);
            var expected = new[] { 0.02m, 0.03m };
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
