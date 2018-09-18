#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WmcSoft.Data.SqlClient
{
    public class SqlDataType
    {
        private SqlDataType(SqlParameter p)
        {
            DbType = p.SqlDbType;
            Size = p.Size;
            Precision = p.Precision;
            Scale = p.Scale;
        }

        public SqlDataType(SqlDbType dbType, byte precision, byte scale)
            : this(new SqlParameter("p", dbType) { Precision = precision, Scale = scale })
        {
        }

        public SqlDataType(SqlDbType dbType, int size)
            : this(new SqlParameter("p", dbType) { Size = size })
        {
        }

        public SqlDataType(SqlDbType dbType)
           : this(new SqlParameter("p", dbType))
        {
        }

        public SqlDbType DbType { get; }
        public int Size { get; }
        public byte Scale { get; }
        public byte Precision { get; }

        public override string ToString()
        {
            // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
            switch (DbType) {
            case SqlDbType.Variant:
                return "sql_variant";
            case SqlDbType.BigInt:
            case SqlDbType.Bit:
            case SqlDbType.DateTime:
            case SqlDbType.Image:
            case SqlDbType.Int:
            case SqlDbType.Money:
            case SqlDbType.UniqueIdentifier:
            case SqlDbType.SmallDateTime:
            case SqlDbType.SmallInt:
            case SqlDbType.SmallMoney:
            case SqlDbType.NText:
            case SqlDbType.Text:
            case SqlDbType.Timestamp:
            case SqlDbType.TinyInt:
            case SqlDbType.Xml:
            case SqlDbType.Date:
            case SqlDbType.Time:
            case SqlDbType.DateTime2:
            case SqlDbType.DateTimeOffset:
                return Format(DbType);
            case SqlDbType.Binary:
            case SqlDbType.Char:
            case SqlDbType.NChar:
            case SqlDbType.NVarChar:
            case SqlDbType.VarBinary:
            case SqlDbType.VarChar:
                return Format(DbType) + Format(Size);
            case SqlDbType.Decimal:
                return Format(DbType) + Format(Precision, Scale);
            case SqlDbType.Float:
            case SqlDbType.Real:
                return Format(DbType) + Format(Size);
            case SqlDbType.Udt:
            case SqlDbType.Structured:
            default:
                throw new NotSupportedException();
            }
        }

        static string Format(SqlDbType dbType) => dbType.ToString().ToLowerInvariant();
        static string Format(int size)
        {
            switch (size) {
            case -1: return "(max)";
            case 0: return "";
            default: return ("(" + size + ")");
            };
        }
        static string Format(byte precision, byte scale) => "(" + precision + (scale > 0 ? ("," + scale + ")") : ")");

        #region Factory methods

        static Dictionary<Type, SqlDataType> Defaults = new Dictionary<Type, SqlDataType>()
        {
            { typeof(long), new SqlDataType(SqlDbType.BigInt) },
            { typeof(int), new SqlDataType(SqlDbType.Int) },
        };

        public static SqlDataType Create<T>()
        {
            if (Defaults.TryGetValue(typeof(T), out SqlDataType result))
                return result;
            throw new NotSupportedException();
        }

        #endregion
    }

    public static class SqlConnectionTraits
    {
        private static string[] SupportedVersion = new[] { "Microsoft SQL Server 2008", "Microsoft SQL Server 2012", "Microsoft SQL Server 2014" };

        static string GetVersion(SqlConnection connection)
        {
            using (connection.Borrow())
            using (var command = connection.CreateCommand("SELECT @@VERSION")) {
                return (string)command.ExecuteScalar();
            }
        }

        /*
        void ForEachTable(string query, params string[] tables)
        {
            var length = tables?.Length ?? 0;
            for (int i = 0; i < length; i++) {
                _connection.ExecuteNonQuery(string.Format(query, tables[i]));
            }
        }

        void TruncateWithPartitions(int[] partitions, string[] tables)
        {
            var truncating = $"TRUNCATE TABLE [{{0}}] WITH (PARTITIONS({string.Join(",", partitions)}));";
            ForEachTable(truncating, tables);
        }

        void TruncateWithoutPartitions(int[] partitions, string[] tables)
        {
            foreach (var partition in partitions) {
                var truncating = $@"ALTER TABLE [{{0}}] SWITCH PARTITION {partition} TO [{_cleanup}].[{{0}}]
TRUNCATE TABLE [{_cleanup}].[{{0}}]";
                ForEachTable(truncating, tables);
            }
        }

        void Truncate(int[] partitions, string[] tables)
        {
            if (_cleanup == null)
                TruncateWithPartitions(partitions, tables);
            else
                TruncateWithoutPartitions(partitions, tables);
        }

        public void Truncate(int boundary, params string[] tables)
        {
            using (var loan = _connection.Borrow()) {
                var partition = PartitionOf(boundary);

                if (tables == null || tables.Length == 0)
                    tables = GetPartitionnedTables();
                var partitions = new[] { partition };
                Truncate(partitions, tables);
            }
        }

        public int DiscardUntil(int boundary, params string[] tables)
        {
            const string query = @"SELECT boundary_id,value FROM sys.partition_range_values v
JOIN sys.partition_functions f ON f.function_id=v.function_id
WHERE f.name = @name AND value < @boundary";

            using (var loan = _connection.Borrow()) {
                var partitions = _connection.ReadAll(query, parameters: new { name = Name, boundary }, materializer: (r) => Tuple.Create(r.GetInt32(0), r.GetInt32(1))).ToList();
                if (partitions.Count == 0)
                    return 0;

                if (tables == null || tables.Length == 0)
                    tables = GetPartitionnedTables();

                Truncate(partitions.Select(p => p.Item1).ToArray(), tables);

                var merging = _connection.Prepare<int>($"ALTER PARTITION FUNCTION {Name}() MERGE RANGE(@p0)");
                partitions.ForEach(p => merging(p.Item2));

                return partitions.Count;
            }
        }
        */
    }
}
