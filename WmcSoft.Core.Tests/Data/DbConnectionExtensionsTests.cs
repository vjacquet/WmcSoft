using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Data.Odbc;
using WmcSoft.Data.SqlClient;
using WmcSoft.Data.OleDb;

namespace WmcSoft.Data
{
    [TestClass]
    public class DbConnectionExtensionsTests
    {
        [TestMethod]
        public void CanCreateParametrizedSqlQuery()
        {
            var id = 0;
            using (var connection = new SqlConnection())
            using (var command = connection.CreateParametrizedCommand($"select count(*) from table where id={id}")) {
                Assert.AreEqual(1, command.Parameters.Count);
                Assert.AreEqual("p0", command.Parameters[0].ParameterName);
                Assert.AreEqual("select count(*) from table where id=@p0", command.CommandText);
            }
        }

        [TestMethod]
        public void CanCreateParametrizedOdbcQuery()
        {
            var id = 0;
            using (var connection = new OdbcConnection())
            using (var command = connection.CreateParametrizedCommand($"select count(*) from table where id={id}")) {
                Assert.AreEqual(1, command.Parameters.Count);
                Assert.AreEqual("p0", command.Parameters[0].ParameterName);
                Assert.AreEqual("select count(*) from table where id=?", command.CommandText);
            }
        }

        [TestMethod]
        public void CanCreateParametrizedOleDbQuery()
        {
            var id = 0;
            using (var connection = new OleDbConnection())
            using (var command = connection.CreateParametrizedCommand($"select count(*) from table where id={id}")) {
                Assert.AreEqual(1, command.Parameters.Count);
                Assert.AreEqual("p0", command.Parameters[0].ParameterName);
                Assert.AreEqual("select count(*) from table where id=?", command.CommandText);
            }
        }
    }
}