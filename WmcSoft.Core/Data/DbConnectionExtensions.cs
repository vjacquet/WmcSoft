#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

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
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace WmcSoft.Data
{
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Opens a database connection.
        /// </summary>
        /// <param name="connectionStringSettings">Connection string from the connection strings configuration section.</param>
        /// <returns>An open instance of the connection.</returns>
        public static DbConnection OpenConnection(this ConnectionStringSettings connectionStringSettings) {
            var factory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionStringSettings.ConnectionString;
            connection.Open();
            return connection;
        }

        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, object parameters = null) {
            var command = connection.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;
            command.Transaction = transaction;
            command.WithParameters(parameters);
            if (timeout != null)
                command.CommandTimeout = (int)Math.Max(timeout.GetValueOrDefault().TotalSeconds, 1d);
            return command;
        }

        public static int ExecuteNonQuery(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, object parameters = null) {
            using (var command = connection.CreateCommand(commandText, commandType, timeout, transaction, parameters)) {
                return command.ExecuteNonQuery();
            }
        }

        public static T ReadScalar<T>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, object parameters = null) {
            using (var command = connection.CreateCommand(commandText, commandType, timeout, transaction, parameters)) {
                return command.ReadScalar<T>();
            }
        }

        public static T ReadScalarOrDefault<T>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, object parameters = null) {
            using (var command = connection.CreateCommand(commandText, commandType, timeout, transaction, parameters)) {
                return command.ReadScalarOrDefault<T>();
            }
        }

        public static T? ReadNullableScalar<T>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, object parameters = null)
            where T : struct {
            using (var command = connection.CreateCommand(commandText, commandType, timeout, transaction, parameters)) {
                return command.ReadNullableScalar<T>();
            }
        }

        static IDbCommand Prepare(int parameterCount, out IDbDataParameter[] parameters, IDbConnection connection, string commandText, CommandType commandType, TimeSpan? timeout, IDbTransaction transaction, Func<int, string> nameGenerator) {
            var command = connection.CreateCommand(commandText, commandType, timeout, transaction);
            parameters = command.AddParameters(parameterCount, nameGenerator);
            return command;
        }

        static void SetValues(IDbDataParameter[] parameters, params object[] values) {
            for (int i = 0; i < parameters.Length; i++) {
                parameters[i].Value = values[i];
            }
        }

        #region PrepareExecuteNonQuery

        public static Func<T, int> PrepareExecuteNonQuery<T>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            var command = connection.CreateCommand(commandText, commandType, timeout, transaction);
            var p = command.AddParameter(nameGenerator);
            return p0 => {
                p.Value = p0;
                return command.ExecuteNonQuery();
            };
        }

        public static Func<T1, T2, int> PrepareExecuteNonQuery<T1, T2>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            IDbDataParameter[] p;
            var command = Prepare(2, out p, connection, commandText, commandType, timeout, transaction, nameGenerator);

            return (p0, p1) => {
                SetValues(p, p0, p1);
                return command.ExecuteNonQuery();
            };
        }

        public static Func<T1, T2, T3, int> PrepareExecuteNonQuery<T1, T2, T3>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            IDbDataParameter[] p;
            var command = Prepare(3, out p, connection, commandText, commandType, timeout, transaction, nameGenerator);

            return (p0, p1, p2) => {
                SetValues(p, p0, p1, p2);
                return command.ExecuteNonQuery();
            };
        }

        public static Func<T1, T2, T3, T4, int> PrepareExecuteNonQuery<T1, T2, T3, T4>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            IDbDataParameter[] p;
            var command = Prepare(4, out p, connection, commandText, commandType, timeout, transaction, nameGenerator);

            return (p0, p1, p2, p3) => {
                SetValues(p, p0, p1, p2, p3);
                return command.ExecuteNonQuery();
            };
        }

        public static Func<T1, T2, T3, T4, T5, int> Prepare<T1, T2, T3, T4, T5>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            IDbDataParameter[] p;
            var command = Prepare(5, out p, connection, commandText, commandType, timeout, transaction, nameGenerator);

            return (p0, p1, p2, p3, p4) => {
                SetValues(p, p0, p1, p2, p3, p4);
                return command.ExecuteNonQuery();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, int> PrepareExecuteNonQuery<T1, T2, T3, T4, T5, T6>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            IDbDataParameter[] p;
            var command = Prepare(6, out p, connection, commandText, commandType, timeout, transaction, nameGenerator);

            return (p0, p1, p2, p3, p4, p5) => {
                SetValues(p, p0, p1, p2, p3, p4, p5);
                return command.ExecuteNonQuery();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, int> PrepareExecuteNonQuery<T1, T2, T3, T4, T5, T6, T7>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            IDbDataParameter[] p;
            var command = Prepare(7, out p, connection, commandText, commandType, timeout, transaction, nameGenerator);

            return (p0, p1, p2, p3, p4, p5, p6) => {
                SetValues(p, p0, p1, p2, p3, p4, p5, p6);
                return command.ExecuteNonQuery();
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, int> PrepareExecuteNonQuery<T1, T2, T3, T4, T5, T6, T7, T8>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            IDbDataParameter[] p;
            var command = Prepare(8, out p, connection, commandText, commandType, timeout, transaction, nameGenerator);

            return (p0, p1, p2, p3, p4, p5, p6, p7) => {
                SetValues(p, p0, p1, p2, p3, p4, p5, p6, p7);
                return command.ExecuteNonQuery();
            };
        }

        #endregion
    }
}
