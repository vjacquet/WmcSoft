using System.Data.SqlClient;
using System.Transactions;
using NUnit.Framework;

namespace DevFi.Tools.IO.Sources
{
    [TestFixture]
    [Category("TSQL")]
    [Ignore("Integration tests that requires a database.")]
    public class SqlConnectionStreamStoreTests : StreamStoreTests
    {
        private SqlConnection _connection;
        private TransactionScope _scope;

        [SetUp]
        public void SetUp()
        {
            _scope = new TransactionScope();

            _connection = new SqlConnection("data source=MSDZDVSQ01\\MWDDW701;initial catalog=SWING;User Id=SwingUser;Password=rLOfh21d;Persist Security Info=True;MultipleActiveResultSets=True");
            _connection.Open();
        }

        [TearDown]
        public void TearDown()
        {
            _scope.Dispose();
            _connection.Close();
        }

        protected override IStreamStore CreateEmptyStore(IDateTimeSource source)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM [testing].[FileStorage]";
                command.ExecuteNonQuery();
            }
            return new SqlConnectionStreamStore(_connection, "FileStorage", "testing", dateTimeSource: source);
        }
    }
}
