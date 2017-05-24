using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Data
{
    [TestClass]
    public class DataConvertTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void CannotConvertDBNullWithConvert()
        {
            var actual = Convert.ChangeType(DBNull.Value, typeof(int?));
        }

        [TestMethod]
        public void CanConvertDBNullWithDataConvert()
        {
            var actual = DataConvert.ChangeType<int?>(DBNull.Value);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void CanConvertToDecimal()
        {
            object value = 0.1f;
            var expected = 0.1m;
            var actual = DataConvert.ChangeType<decimal>(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanConvertToNullableDecimal()
        {
            object value = 0.1f;
            var expected = 0.1m;
            var actual = DataConvert.ChangeType<decimal?>(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CanConvertDBNullToNullableDecimal()
        {
            object value = DBNull.Value;
            decimal? expected = null;
            var actual = DataConvert.ChangeType<decimal?>(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void CannotConvertDBNullToDecimal()
        {
            object value = DBNull.Value;
            var actual = DataConvert.ChangeType<decimal>(value);
        }
    }
}
