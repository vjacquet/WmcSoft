using System;
using Xunit;

namespace WmcSoft.Business
{
    public class TemporalTests
    {
        class Sample : ITemporal
        {
            public DateTime? ValidSince { get; set; }
            public DateTime? ValidUntil { get; set; }
        }

        [Fact]
        public void CheckValidOnFullyDefinedRange()
        {
            var sample = new Sample {
                ValidSince = new DateTime(1973, 5, 2),
                ValidUntil = new DateTime(2016, 4, 2),
            };

            Assert.False(sample.IsValidOn(new DateTime(1789, 7, 14)));
            Assert.False(sample.IsValidOn(new DateTime(2016, 4, 2)));
            Assert.False(sample.IsValidOn(new DateTime(2016, 4, 3)));
            Assert.True(sample.IsValidOn(new DateTime(1973, 5, 2)));
            Assert.True(sample.IsValidOn(new DateTime(2003, 5, 2)));
        }

        [Fact]
        public void CheckValidOnUndefinedRange()
        {
            var sample = new Sample {
                ValidSince = null,
                ValidUntil = null,
            };

            Assert.True(sample.IsValidOn(new DateTime(1789, 7, 14)));
            Assert.True(sample.IsValidOn(new DateTime(2016, 4, 2)));
            Assert.True(sample.IsValidOn(new DateTime(2016, 4, 3)));
            Assert.True(sample.IsValidOn(new DateTime(1973, 5, 2)));
            Assert.True(sample.IsValidOn(new DateTime(2003, 5, 2)));
        }

        [Fact]
        public void CheckValidSince()
        {
            var sample = new Sample {
                ValidSince = new DateTime(1973, 5, 2),
                ValidUntil = null,
            };

            Assert.False(sample.IsValidOn(new DateTime(1789, 7, 14)));
            Assert.True(sample.IsValidOn(new DateTime(2016, 4, 2)));
            Assert.True(sample.IsValidOn(new DateTime(2016, 4, 3)));
            Assert.True(sample.IsValidOn(new DateTime(1973, 5, 2)));
            Assert.True(sample.IsValidOn(new DateTime(2003, 5, 2)));
        }

        [Fact]
        public void CheckValidUntil()
        {
            var sample = new Sample {
                ValidSince = null,
                ValidUntil = new DateTime(2016, 4, 2),
            };

            Assert.True(sample.IsValidOn(new DateTime(1789, 7, 14)));
            Assert.False(sample.IsValidOn(new DateTime(2016, 4, 2)));
            Assert.False(sample.IsValidOn(new DateTime(2016, 4, 3)));
            Assert.True(sample.IsValidOn(new DateTime(1973, 5, 2)));
            Assert.True(sample.IsValidOn(new DateTime(2003, 5, 2)));
        }
    }
}
