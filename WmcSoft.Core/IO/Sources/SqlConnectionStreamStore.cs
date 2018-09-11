using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;
using DevFi.Tools.Data;

namespace DevFi.Tools.IO.Sources
{
    public class SqlConnectionStreamStore : StreamStore
    {
        public class SqlStorageEntry : StorageEntry
        {
            private readonly SqlConnectionStreamStore _store;
            private readonly int _id;

            public SqlStorageEntry(SqlConnectionStreamStore store, int id, string name, int length, byte[] hash, DateTime since, DateTime? until = null)
                : base(name, length, hash, since, until)
            {
                _store = store;
                _id = id;
            }

            public override Stream Open()
            {
                return _store.Open(_id);
            }
        }

        private readonly Func<SqlConnection> _factory;
        private readonly string _tableName;

        private const int BufferSize = 4096;

        static Func<SqlConnection> CreateFactory(SqlConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            return () => connection;
        }

        public SqlConnectionStreamStore(SqlConnection connection, string tableName, string schema = null, IDateTimeSource dateTimeSource = null)
            : this(CreateFactory(connection), tableName, schema, dateTimeSource)
        {
        }

        public SqlConnectionStreamStore(Func<SqlConnection> factory, string tableName, string schema = null, IDateTimeSource dateTimeSource = null)
            : base(dateTimeSource ?? DateTimeSources.Utc)
        {
            Schema = schema;
            TableName = tableName;

            _tableName = (schema == null) ? tableName : (schema + "." + tableName);
            _factory = factory;
        }

        protected string Schema { get; }
        protected string TableName { get; }

        protected Stream Open(int id)
        {
            var connection = _factory();
            using (connection.Borrow())
            using (var command = connection.CreateCommand($@"SELECT FileLength, RawStorage FROM {_tableName} WHERE [Id]=@id", parameters: new { id }))
            using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
            {
                if (reader.Read())
                {
                    var length = reader.GetInt32(0);
                    var buffer = new byte[length];
                    var count = reader.GetBytes(1, 0, buffer, 0, length);
                    return new MemoryStream(buffer, 0, (int)count);
                }
                return Stream.Null;
            }
        }

        public override IEnumerable<StorageEntry> GetHistory(string name)
        {
            var connection = _factory();
            using (connection.Borrow())
            using (var command = connection.CreateCommand($@"SELECT Id, FileName, FileLength, Hash, ValidSince, ValidUntil FROM {_tableName} WHERE FileName=@name ORDER BY ValidSince DESC", parameters: new { name }))
            using (var reader = command.ExecuteReader())
            {
                return reader.ReadAll(ReadEntry).ToList();
            }
        }

        StorageEntry ReadEntry(IDataRecord reader)
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);
            var length = reader.GetInt32(2);
            var hash = reader.GetBytes(3, 32);
            var since = reader.GetDateTime(4);
            var until = reader.GetNullableDateTime(5);
            return new SqlStorageEntry(this, id, name, length, hash, since, until);
        }

        public override IEnumerable<StorageEntry> GetInventory(DateTime asOfUtc)
        {
            var connection = _factory();
            using (connection.Borrow())
            using (var command = connection.CreateCommand($@"SELECT Id, FileName, FileLength, Hash, ValidSince, ValidUntil FROM {_tableName} WHERE ValidSince<=@asOfUtc And (ValidUntil>@asOfUtc or ValidUntil is null)", parameters: new { asOfUtc }))
            using (var reader = command.ExecuteReader())
            {
                return reader.ReadAll(ReadEntry).ToList();
            }
        }

        public override StorageEntry Find(string name, DateTime asOfUtc)
        {
            var connection = _factory();
            using (connection.Borrow())
            using (var command = connection.CreateCommand($@"SELECT Id, FileName, FileLength, Hash, ValidSince, ValidUntil FROM {_tableName} WHERE FileName=@name AND ValidSince<=@asOfUtc And (ValidUntil>@asOfUtc or ValidUntil is null)", parameters: new { name, asOfUtc }))
            using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
            {
                if (!reader.Read())
                    return null;
                return ReadEntry(reader);
            }
        }

        protected override void Store(StorageEntry latest, StorageEntry metadata, Stream stream)
        {
            var name = metadata.Name;
            var data = ReadAll(stream);
            var length = metadata.Length;
            var hash = metadata.Hash;
            var since = metadata.ValidSinceUtc;
            XmlReader xml = null;
            TextReader text = null;

            var ext = Path.GetExtension(name).ToUpperInvariant();
            switch (ext)
            {
                case ".XML":
                    xml = XmlReader.Create(new MemoryStream(data));
                    break;
                case ".TXT":
                case ".CSV":
                case ".JSON":
                    text = new StreamReader(new MemoryStream(data));
                    break;
            }

            string query = $@"UPDATE {_tableName} SET ValidUntil=@since WHERE FileName=@name AND ValidUntil IS NULL;
INSERT INTO {_tableName}(FileName, FileLength, ValidSince, Hash, RawStorage, Xml, Text)
VALUES (@name, @length, @since, @hash, @data, @xml, @text)";

            var connection = _factory();
            using (connection.Borrow())
            using (var transaction = connection.BeginTransaction())
            {
                if (latest != null)
                {
                    var closeQuery = $@"UPDATE { _tableName} SET ValidUntil=@since WHERE FileName = @name AND ValidUntil IS NULL AND Hash=@hash";
                    using (var close = connection.CreateCommand(closeQuery, transaction: transaction, parameters: new { name, since, hash = latest.Hash }))
                    {
                        var affected = close.ExecuteNonQuery();
                        if (affected == 0)
                            throw new DBConcurrencyException();
                    }
                }
                using (var command = connection.CreateCommand(query, transaction: transaction, parameters: new { name, length, since, hash, data, xml, text }))
                {
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }
    }
}
