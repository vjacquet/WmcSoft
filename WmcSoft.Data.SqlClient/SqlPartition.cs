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
using System.Linq;
using System.Data.SqlClient;

namespace WmcSoft.Data.SqlClient
{
    public class SqlPartition
    {
        private readonly SqlConnection _connection;

        private readonly string _queryPartitions;

        public SqlPartition(SqlConnection connection, string name)
        {
            _connection = connection;

            _queryPartitions = $@"SELECT {{0}} FROM sys.partition_range_values v
JOIN sys.partition_functions f ON f.function_id=v.function_id
WHERE name = @name";
            Name = name;
        }

        public string Name { get; }

        private string QueryPartitions(string expr)
        {
            return string.Format(_queryPartitions, expr);
        }

        public int GetLowerBound()
        {
            string query = QueryPartitions("MIN(value)");

            using (var loan = _connection.Borrow()) {
                return _connection.ExecuteScalar<int>(query, parameters: new { name = Name });
            }
        }

        public int GetUpperBound()
        {
            string query = QueryPartitions("MAX(value)");

            using (var loan = _connection.Borrow()) {
                return _connection.ExecuteScalar<int>(query, parameters: new { name = Name });
            }
        }

        public int DiscardUntil(int boundary, params string[] tables)
        {
            // value is an SqlVariant
            string query = QueryPartitions("boundary_id, value, boundary_value_on_right") + " AND value < @boundary";

            using (var loan = _connection.Borrow())
            using (var command = _connection.CreateCommand(query, parameters: new { name = Name, boundary })) {
                var partitions = command.ReadAll(r => Tuple.Create(r.GetInt32(0), r.GetInt32(1))).ToList();
                if (partitions.Count == 0)
                    return 0;

                if (tables == null || tables.Length == 0)
                    tables = GetPartitionnedTables();

                var truncating = $"TRUNCATE TABLE [{{0}}] WITH (PARTITIONS({string.Join(",", partitions.Select(p => p.Item1))}))";
                var length = tables?.Length ?? 0;
                for (int i = 0; i < length; i++) {
                    _connection.ExecuteNonQuery(string.Format(truncating, tables[i]));
                }

                var merging = _connection.PrepareExecuteNonQuery<int>($"ALTER PARTITION FUNCTION {Name}() MERGE RANGE(@p0)");
                foreach (var p in partitions.Select(p => p.Item2)) {
                    merging(p);
                }

                return partitions.Count;
            }
        }

        public string[] GetPartitionnedTables()
        {
            var query = $@"SELECT distinct t.name AS [table], s.name AS [scheme], f.name AS [partition]
FROM sys.indexes i
JOIN sys.partition_schemes s ON s.data_space_id = i.data_space_id
JOIN sys.partition_functions f ON f.function_id = s.function_id
JOIN sys.tables t ON i.object_id=t.object_id
WHERE f.name = '{Name}'";

            using (var loan = _connection.Borrow())
            using (var command = _connection.CreateCommand(query)) {
                return command.ReadAll(r => r.GetString(0)).ToArray();
            }
        }

        public void MergeAt(int boundary)
        {
            var query = $@"ALTER PARTITION FUNCTION {Name}() MERGE RANGE (@boundary);";

            using (var loan = _connection.Borrow()) {
                _connection.ExecuteNonQuery(query, parameters: new { boundary });
            }
        }

        public bool CanSplit()
        {
            return !EnumerateSchemes().Any(r => r.unmarked);
        }

        public string[] GetUnmarkedSchemes()
        {
            return EnumerateSchemes().Where(r => r.unmarked).Select(r => r.scheme).ToArray();
        }

        private IEnumerable<(string scheme, bool unmarked)> EnumerateSchemes()
        {
            var query = $@"select scheme, max(dest_rank)
from
(
   select fg.Name as filegroup, ps.Name as scheme, RANK() OVER (PARTITION BY ps.name order by dds.destination_id) as dest_rank
   from sys.partition_schemes ps
   inner join sys.partition_functions f ON f.function_id=ps.function_id
   inner join sys.destination_data_spaces as dds on dds.partition_scheme_id = ps.data_space_id
   inner join sys.filegroups as fg on fg.data_space_id = DDS.data_space_ID 
   left join sys.partition_range_values as prv on PRV.Boundary_ID = DDS.destination_id and prv.function_id=ps.function_id 
   where prv.Value is null and f.name = '{Name}'
) as q
where dest_rank <= 2
group by filegroup, scheme";

            using (var loan = _connection.Borrow())
            using (var command = _connection.CreateCommand(query)) {
                foreach (var record in command.ReadAll(r => (r.GetString(0), r.GetInt64(1) == 1L))) {
                    yield return record;
                }
            }
        }

        public void SplitAt(int boundary)
        {
            var query = $@"ALTER PARTITION FUNCTION {Name}() SPLIT RANGE (@boundary);";

            using (var loan = _connection.Borrow()) {
                _connection.ExecuteNonQuery(query, parameters: new { boundary });
            }
        }

        public int PartitionOf(int value)
        {
            var query = $@"SELECT $PARTITION.{Name}(@value)";

            using (var loan = _connection.Borrow()) {
                return _connection.ExecuteScalar<int>(query, parameters: new { value });
            }
        }

        public void MarkNextUsed(string fileGroup = "[PRIMARY]", string scheme = null)
        {
            if (string.IsNullOrWhiteSpace(fileGroup)) throw new ArgumentException(nameof(fileGroup));

            var query = $@"ALTER PARTITION SCHEME {{0}} NEXT USED {fileGroup}";
            using (var loan = _connection.Borrow()) {
                if (scheme == null) {
                    foreach (var s in GetUnmarkedSchemes()) {
                        _connection.ExecuteNonQuery(string.Format(query, s));
                    }
                } else if (string.IsNullOrWhiteSpace(scheme)) {
                    throw new ArgumentException(nameof(scheme));
                } else {
                    _connection.ExecuteNonQuery(string.Format(query, scheme));
                }
            }
        }
    }
}
