using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Business
{
    [TestClass]
    public class TemporalTests
    {
        class Sample : ITemporal
        {
            public DateTime? ValidSince { get; set; }
            public DateTime? ValidUntil { get; set; }
        }

        [TestMethod]
        public void CheckValidOnFullyDefinedRange() {
            var sample = new Sample {
                ValidSince = new DateTime(1973, 5, 2),
                ValidUntil = new DateTime(2016, 4, 2),
            };

            Assert.IsFalse(sample.IsValidOn(new DateTime(1789, 7, 14)));
            Assert.IsFalse(sample.IsValidOn(new DateTime(2016, 4, 2)));
            Assert.IsFalse(sample.IsValidOn(new DateTime(2016, 4, 3)));
            Assert.IsTrue(sample.IsValidOn(new DateTime(1973, 5, 2)));
            Assert.IsTrue(sample.IsValidOn(new DateTime(2003, 5, 2)));
        }

        [TestMethod]
        public void CheckValidOnUndefinedRange() {
            var sample = new Sample {
                ValidSince = null,
                ValidUntil = null,
            };

            Assert.IsTrue(sample.IsValidOn(new DateTime(1789, 7, 14)));
            Assert.IsTrue(sample.IsValidOn(new DateTime(2016, 4, 2)));
            Assert.IsTrue(sample.IsValidOn(new DateTime(2016, 4, 3)));
            Assert.IsTrue(sample.IsValidOn(new DateTime(1973, 5, 2)));
            Assert.IsTrue(sample.IsValidOn(new DateTime(2003, 5, 2)));
        }

        [TestMethod]
        public void CheckValidSince() {
            var sample = new Sample {
                ValidSince = new DateTime(1973, 5, 2),
                ValidUntil = null,
            };

            Assert.IsFalse(sample.IsValidOn(new DateTime(1789, 7, 14)));
            Assert.IsTrue(sample.IsValidOn(new DateTime(2016, 4, 2)));
            Assert.IsTrue(sample.IsValidOn(new DateTime(2016, 4, 3)));
            Assert.IsTrue(sample.IsValidOn(new DateTime(1973, 5, 2)));
            Assert.IsTrue(sample.IsValidOn(new DateTime(2003, 5, 2)));
        }

        [TestMethod]
        public void CheckValidUntil() {
            var sample = new Sample {
                ValidSince = null,
                ValidUntil = new DateTime(2016, 4, 2),
            };

            Assert.IsTrue(sample.IsValidOn(new DateTime(1789, 7, 14)));
            Assert.IsFalse(sample.IsValidOn(new DateTime(2016, 4, 2)));
            Assert.IsFalse(sample.IsValidOn(new DateTime(2016, 4, 3)));
            Assert.IsTrue(sample.IsValidOn(new DateTime(1973, 5, 2)));
            Assert.IsTrue(sample.IsValidOn(new DateTime(2003, 5, 2)));
        }

    }
}
