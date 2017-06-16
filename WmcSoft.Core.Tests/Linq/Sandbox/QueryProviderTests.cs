using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using WmcSoft.Data;

namespace WmcSoft.Linq.Sandbox
{
    /// <summary>
    /// Summary description for QueryProviderTests
    /// </summary>
    [TestClass]
    public class QueryProviderTests
    {

        class MockConnection : IDbConnection
        {
            private readonly Dictionary<string, object> _results;

            public MockConnection() {
                _results = new Dictionary<string, object>();

            }

            public string ConnectionString {
                get; set;
            }

            public int ConnectionTimeout {
                get; set;
            }

            public string Database {
                get; private set;
            }

            public ConnectionState State {
                get; private set;
            }

            public IDbTransaction BeginTransaction() {
                return BeginTransaction(IsolationLevel.Unspecified);
            }

            public IDbTransaction BeginTransaction(IsolationLevel il) {
                throw new NotImplementedException();
            }

            public void ChangeDatabase(string databaseName) {
                Database = databaseName;
            }

            public void Close() {
                State = ConnectionState.Closed;
            }

            public IDbCommand CreateCommand() {
                return new MockCommand(this);
            }

            public void Dispose() {
                Close();
            }

            public void Open() {
                State = ConnectionState.Open;
            }

            public int ExecuteNonQuery(string commandText) {
                return (int)_results[commandText];
            }

            public IDataReader ExecuteReader(string commandText) {
                return (IDataReader)_results[commandText];
            }

            public object ExecuteScalar(string commandText) {
                return _results[commandText];
            }

            public void ExpectCommandResult(string commandText, object result) {
                _results.Add(commandText, result);
            }
        }

        class MockCommand : IDbCommand
        {
            readonly MockConnection _connection;

            public MockCommand(MockConnection connection) {
                CommandType = CommandType.Text;
                Connection = _connection = connection;
            }

            public string CommandText {
                get; set;
            }

            public int CommandTimeout {
                get; set;
            }

            public CommandType CommandType {
                get; set;
            }

            public IDbConnection Connection {
                get; set;
            }

            public IDataParameterCollection Parameters {
                get {
                    throw new NotImplementedException();
                }
            }

            public IDbTransaction Transaction {
                get; set;
            }

            public UpdateRowSource UpdatedRowSource {
                get; set;
            }

            public void Cancel() {
                throw new NotImplementedException();
            }

            public IDbDataParameter CreateParameter() {
                throw new NotImplementedException();
            }

            public void Dispose() {
            }

            public int ExecuteNonQuery() {
                return _connection.ExecuteNonQuery(CommandText);
            }

            public IDataReader ExecuteReader() {
                return ExecuteReader(CommandBehavior.Default);
            }

            public IDataReader ExecuteReader(CommandBehavior behavior) {
                return _connection.ExecuteReader(CommandText);
            }

            public object ExecuteScalar() {
                return _connection.ExecuteScalar(CommandText);
            }

            public void Prepare() {
            }
        }


        [TestMethod]
        public void CanQueryCustomers() {
            using (var connection = new MockConnection()) {
                var customers = new Customer[] {
                    new Customer { ContactName="Thomas Hardy", City = "London", Phone = "(171) 555-7788" },
                    new Customer { ContactName="Victoria Ashworth", City = "London", Phone = "(171) 555-1212" },
                    new Customer { ContactName="Elizabeth Brown", City = "London", Phone = "(171) 555-2282" },
                    new Customer { ContactName="Ann Devon", City = "London", Phone = "(171) 555-0297" },
                    new Customer { ContactName="Simon Crowther", City = "London", Phone = "(171) 555-7733" },
                    new Customer { ContactName="Hari Kumar", City = "London", Phone = "(171) 555-1717" },
                };
                connection.ExpectCommandResult("SELECT * FROM (SELECT * FROM Customer) AS T WHERE (City = 'London')", new TypeDescriptorDataReader<Customer>(customers));
                connection.Open();

                Northwind db = new Northwind(connection);

                string city = "London";
                var query = db.Customers.Where(c => c.City == city);
                var actual = query.ToList();
                CollectionAssert.AreEqual(customers.Select(c => c.ContactName).ToList(), actual.Select(c => c.ContactName).ToList());
            }
        }
    }
}
