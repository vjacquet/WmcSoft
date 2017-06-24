using System;
using Xunit;

namespace WmcSoft.Data
{
    public class DataConvertTests
    {
        [Fact]
        public void CannotConvertDBNullWithConvert()
        {
            Assert.Throws<InvalidCastException>(() => Convert.ChangeType(DBNull.Value, typeof(int?)));
        }

        [Fact]
        public void CanConvertDBNullWithDataConvert()
        {
            var actual = DataConvert.ChangeType<int?>(DBNull.Value);
            Assert.Null(actual);
        }

        [Fact]
        public void CanConvertToDecimal()
        {
            object value = 0.1f;
            var expected = 0.1m;
            var actual = DataConvert.ChangeType<decimal>(value);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanConvertToNullableDecimal()
        {
            object value = 0.1f;
            var expected = 0.1m;
            var actual = DataConvert.ChangeType<decimal?>(value);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanConvertDBNullToNullableDecimal()
        {
            object value = DBNull.Value;
            decimal? expected = null;
            var actual = DataConvert.ChangeType<decimal?>(value);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CannotConvertDBNullToDecimal()
        {
            Assert.Throws<InvalidCastException>(() => DataConvert.ChangeType<decimal>(DBNull.Value));
        }
    }
}