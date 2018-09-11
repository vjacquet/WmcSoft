using System.Data;
using Xunit;
using WmcSoft.Data.SqlClient;

namespace WmcSoft.Data.Tests
{
    public class SqlDataTypeTests
    {
        [Theory]
        [InlineData(SqlDbType.Bit, "bit")]
        [InlineData(SqlDbType.BigInt, "bigint")]
        [InlineData(SqlDbType.Int, "int")]
        [InlineData(SqlDbType.Date, "date")]
        [InlineData(SqlDbType.Real, "real")]
        public void CanCreateUnsizedDefinition(SqlDbType dbType, string expected)
        {
            var definition = new SqlDataType(dbType);
            var actual = definition.ToString();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(SqlDbType.NVarChar, -1, "nvarchar(max)")]
        [InlineData(SqlDbType.VarChar, 256, "varchar(256)")]
        [InlineData(SqlDbType.Real, 5, "real(5)")]
        public void CanCreateSizedDefinition(SqlDbType dbType, int size, string expected)
        {
            var definition = new SqlDataType(dbType, size);
            var actual = definition.ToString();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(SqlDbType.Decimal, 7, 0, "decimal(7)")]
        [InlineData(SqlDbType.Decimal, 7, 1, "decimal(7,1)")]
        public void CanCreateScaledDefinition(SqlDbType dbType, byte precision, byte scale, string expected)
        {
            var definition = new SqlDataType(dbType, precision, scale);
            var actual = definition.ToString();
            Assert.Equal(expected, actual);
        }
    }
}
