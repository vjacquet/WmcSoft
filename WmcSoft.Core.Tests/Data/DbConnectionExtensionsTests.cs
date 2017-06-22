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
        public void CanCreateParameterizedSqlQuery()
        {
            var id = 0;
            using (var connection = new SqlConnection())
            using (var command = connection.CreateParameterizedCommand($"select count(*) from table where id={id}")) {
                Assert.AreEqual(1, command.Parameters.Count);
                Assert.AreEqual("p0", command.Parameters[0].ParameterName);
                Assert.AreEqual("select count(*) from table where id=@p0", command.CommandText);
            }
        }

        [TestMethod]
        public void CanCreateParameterizedSqlQueryWithMultipleOccurencesOfTheParameter()
        {
            var id = 0;
            using (var connection = new SqlConnection())
            using (var command = connection.CreateParameterizedCommand($"select count(*) from table1 where id={id} union select count(*) from table2 where id={id}")) {
                Assert.AreEqual(2, command.Parameters.Count);
                Assert.AreEqual("p0", command.Parameters[0].ParameterName);
                Assert.AreEqual("p1", command.Parameters[1].ParameterName);
                Assert.AreEqual("select count(*) from table1 where id=@p0 union select count(*) from table2 where id=@p1", command.CommandText);
            }
        }


        [TestMethod]
        public void CanCreateParameterizedOdbcQuery()
        {
            var id = 0;
            using (var connection = new OdbcConnection())
            using (var command = connection.CreateParameterizedCommand($"select count(*) from table where id={id}")) {
                Assert.AreEqual(1, command.Parameters.Count);
                Assert.AreEqual("p0", command.Parameters[0].ParameterName);
                Assert.AreEqual("select count(*) from table where id=?", command.CommandText);
            }
        }

        [TestMethod]
        public void CanCreateParameterizedOdbcQueryWithMultipleOccurencesOfTheParameter()
        {
            var id = 0;
            using (var connection = new OdbcConnection())
            using (var command = connection.CreateParameterizedCommand($"select count(*) from table1 where id={id} union select count(*) from table2 where id={id}")) {
                Assert.AreEqual(2, command.Parameters.Count);
                Assert.AreEqual("p0", command.Parameters[0].ParameterName);
                Assert.AreEqual("p1", command.Parameters[1].ParameterName);
                Assert.AreEqual("select count(*) from table1 where id=? union select count(*) from table2 where id=?", command.CommandText);
            }
        }

        [TestMethod]
        public void CanCreateParameterizedOleDbQuery()
        {
            var id = 0;
            using (var connection = new OleDbConnection())
            using (var command = connection.CreateParameterizedCommand($"select count(*) from table where id={id}")) {
                Assert.AreEqual(1, command.Parameters.Count);
                Assert.AreEqual("p0", command.Parameters[0].ParameterName);
                Assert.AreEqual("select count(*) from table where id=?", command.CommandText);
            }
        }

        [TestMethod]
        public void CanCreateParameterizedOleDbQueryWithMultipleOccurencesOfTheParameter()
        {
            var id = 0;
            using (var connection = new OleDbConnection())
            using (var command = connection.CreateParameterizedCommand($"select count(*) from table1 where id={id} union select count(*) from table2 where id={id}")) {
                Assert.AreEqual(2, command.Parameters.Count);
                Assert.AreEqual("p0", command.Parameters[0].ParameterName);
                Assert.AreEqual("p1", command.Parameters[1].ParameterName);
                Assert.AreEqual("select count(*) from table1 where id=? union select count(*) from table2 where id=?", command.CommandText);
            }
        }
    }
}