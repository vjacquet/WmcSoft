using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Data
{
    [TestClass]
    public class DbConnectionExtensionsTests
    {
        [TestMethod]
        public void CanCreateParametrizedQuery()
        {
            var id = 0;
            using (var connection = new SqlConnection())
            using (var command = connection.CreateParametrizedCommand($"select count(*) from [cms].[pages] where id={id}")) {
                Assert.AreEqual(1, command.Parameters.Count);
                Assert.AreEqual("p0", ((IDbDataParameter) command.Parameters[0]).ParameterName);
                Assert.AreEqual("select count(*) from [cms].[pages] where id=@p0", command.CommandText);
            }
        }
    }
}
