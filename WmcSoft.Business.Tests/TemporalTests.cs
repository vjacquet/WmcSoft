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
        public void CheckIsValidOnFullyDefinedRange()
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
        public void CheckIsValidOnUndefinedRange()
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
        public void CheckIsValidUntil()
        {
            var sample = new Sample {
                ValidSince = null,
                ValidUntil = new DateTime(2016, 4, 2),
            };

            Assert.False(sample.IsValidOn(new DateTime(2016, 4, 2)));
            Assert.False(sample.IsValidOn(new DateTime(2016, 4, 3)));

            Assert.True(sample.IsValidOn(new DateTime(1789, 7, 14)));
            Assert.True(sample.IsValidOn(new DateTime(1973, 5, 2)));
            Assert.True(sample.IsValidOn(new DateTime(2003, 5, 2)));
        }

        [Fact]
        public void CheckIsValidOnRange()
        {
            var range = new Sample {
                ValidSince = new DateTime(1973, 5, 2),
                ValidUntil = new DateTime(2016, 4, 2),
            };

            var since = new Sample {
                ValidSince = new DateTime(1973, 5, 2),
                ValidUntil = null,
            };

            var until = new Sample {
                ValidSince = null,
                ValidUntil = new DateTime(2016, 4, 2),
            };

            var forever = new Sample {
                ValidSince = null,
                ValidUntil = null,
            };


            var a = new DateTime(1789, 7, 14);
            var b = new DateTime(1800, 8, 31);
            var c = new DateTime(2001, 1, 31);
            var d = new DateTime(2001, 12, 31);
            var e = new DateTime(2017, 1, 1);
            var f = new DateTime(2017, 5, 1);

            Assert.False(range.IsValidOn(a, b));
            Assert.True(range.IsValidOn(b, c));
            Assert.True(range.IsValidOn(c, d));
            Assert.True(range.IsValidOn(d, e));
            Assert.False(range.IsValidOn(e, f));

            Assert.False(since.IsValidOn(a, b));
            Assert.True(since.IsValidOn(b, c));
            Assert.True(since.IsValidOn(c, d));
            Assert.True(since.IsValidOn(d, e));
            Assert.True(since.IsValidOn(e, f));

            Assert.True(until.IsValidOn(a, b));
            Assert.True(until.IsValidOn(b, c));
            Assert.True(until.IsValidOn(c, d));
            Assert.True(until.IsValidOn(d, e));
            Assert.False(until.IsValidOn(e, f));

            Assert.True(forever.IsValidOn(a, b));
            Assert.True(forever.IsValidOn(b, c));
            Assert.True(forever.IsValidOn(c, d));
            Assert.True(forever.IsValidOn(d, e));
            Assert.True(forever.IsValidOn(e, f));
        }
    }
}
