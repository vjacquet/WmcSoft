using DevFi.Tools.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace DevFi.Tools.IO.Sources
{
    [Obsolete("Use SqlConnectionStreamStore instead.", false)]
    public class DatabaseStreamStore : IStreamStore
    {
        private readonly string _schema;
        private readonly string _connectionString;
        private const int BufferSize = 4096;

        public DatabaseStreamStore(string connectionString, string schema = "LongTerm")
        {
            _schema = schema;

            if (!schema.Contains("."))
                _schema += ".";

            _connectionString = connectionString;
        }

        public byte[] ComputeHash(IStreamSource readable)
        {
            using (var stream = readable.Open())
            using (var algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(stream);
            }
        }

        public IEnumerable<StorageEntry> GetHistory(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $@"Select FileLength, ValidSince, ValidUntil, Hash, RawStorage, Xml, Text from {_schema}FileStorage where FileName = @FileName";
                        command.AddParameter("FileName", name);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var length = reader.GetInt32(0);
                                var hashBuffer = new byte[32];
                                var rawStorageBuffer = new byte[length];
                                reader.GetBytes(3, 0, hashBuffer, 0, 32);
                                reader.GetBytes(4, 0, rawStorageBuffer, 0, length);
                                yield return new DatabaseStorageEntry(name, length, hashBuffer, rawStorageBuffer, reader.GetDateTime(1), reader.GetNullableDateTime(2));
                            }
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }

            }
        }

        public IEnumerable<StorageEntry> GetInventory(DateTime asOfUtc)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $"Select FileName, FileLength, ValidSince, ValidUntil, Hash, RawStorage, Xml, Text from {_schema}FileStorage where ValidSince <= @asOfUtc And (ValidUntil >=  @asOfUtc or ValidUntil is null)";
                        command.AddParameter("asOfUtc", asOfUtc);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                                yield return ReadAnEntryFromReader(reader);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public StorageEntry Find(string name, DateTime asOfUtc)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $@"Select FileName, FileLength, ValidSince, ValidUntil, Hash, RawStorage from {_schema}FileStorage where ValidSince = @ValidSince and FileName = @FileName";
                        command.AddParameter("ValidSince", asOfUtc);
                        command.AddParameter("FileName", name);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return ReadAnEntryFromReader(reader);
                            }
                            return null;
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public Stream Load(string name, DateTime asOfUtc)
        {
            var entry = Find(name, asOfUtc) as DatabaseStorageEntry;
            return new MemoryStream(entry.RawStorage);
        }

        public bool Store(string name, Stream stream)
        {
            var now = DateTime.UtcNow;
            using (var memoryStream = new MemoryStream())
            using (var hashAlgorithm = SHA256.Create())
            using (var cryptoStream = new CryptoStream(memoryStream, hashAlgorithm, CryptoStreamMode.Write))
            {
                var size = StreamCopy(stream, cryptoStream);
                var hash = hashAlgorithm.Hash;
                var rawStorage = memoryStream.ToArray();

                var previousHash = GetWithPreviousHash(name);

                var areEqual = Equals(previousHash, hash);

                if (!areEqual)
                    UpdateAndSave(name, now, hash, rawStorage);

                return !areEqual;
            }
        }

        private byte[] GetWithPreviousHash(string fileName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"Select Hash from {_schema}FileStorage where FileName = @FileName and ValidUntil is Null";
                    command.AddParameter("FileName", fileName);
                    using (var reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            var hash = new byte[32];
                            reader.GetBytes(0, 0, hash, 0, 32);
                            return hash;
                        }
                        return null;
                    }
                }
            }
        }

        private static bool Equals(byte[] x, byte[] y)
        {
            if (x == null || y == null || x.Length != y.Length)
                return false;

            for (var i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                    return false;
            }

            return true;
        }

        private void UpdateAndSave(string fileName, DateTime now, byte[] hash, byte[] rawStorage)
        {
            string target = Path.GetExtension(fileName).Equals(".xml", StringComparison.OrdinalIgnoreCase) ?
                        "Xml" : "Text";
            var text = Encoding.Default.GetString(rawStorage);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                using (var transaction = connection.BeginTransaction())
                {
                    command.Transaction = transaction;
                    try
                    {
                        //1 - InsertInDatabase
                        const string query = @"Insert Into {0}FileStorage (FileName, FileLength, ValidSince, Hash, RawStorage, {1})
                                                values (@FileName, @FileLength, @ValidSince, @Hash, @RawStorage, @{1})";

                        command.AddParameter("FileName", fileName);
                        command.AddParameter("FileLength", rawStorage.Length);
                        command.AddParameter("ValidSince", now);
                        command.AddParameter("Hash", hash);
                        command.AddParameter("RawStorage", rawStorage);

                        command.CommandText = string.Format(query, _schema, target);
                        command.AddParameter(target, text);

                        command.ExecuteNonQuery();

                        //2 - UpdatePreviousSaves
                        command.CommandText = $@"Update {_schema}FileStorage set ValidUntil = @ValidUntil
                                                where FileName = @FileName AND ValidUntil IS NULL and ValidSince < @ValidUntil";
                        command.AddParameter("ValidUntil", now);

                        command.ExecuteNonQuery();
                        command.Transaction.Commit();
                    }
                    catch (Exception)
                    {
                        command.Transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            }
        }

        private static DatabaseStorageEntry ReadAnEntryFromReader(SqlDataReader reader)
        {
            var length = reader.GetInt32(1);
            var hashBuffer = new byte[32];
            var rawStorageBuffer = new byte[length];
            reader.GetBytes(4, 0, hashBuffer, 0, 32);
            reader.GetBytes(5, 0, rawStorageBuffer, 0, length);
            return new DatabaseStorageEntry(reader.GetString(0), length, hashBuffer, rawStorageBuffer, reader.GetDateTime(2), reader.GetNullableDateTime(3));
        }

        private static int StreamCopy(Stream from, Stream to)
        {
            var size = 0;
            var buffer = new byte[BufferSize];
            var r = 0;
            while ((r = from.Read(buffer, 0, BufferSize)) > 0)
            {
                to.Write(buffer, 0, r);
                size += r;
            }
            to.Close();
            return size;
        }
    }
}
