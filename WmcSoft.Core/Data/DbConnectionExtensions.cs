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
        #region Factory methods

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

        /// <summary>
        /// Creates a command to run against the <paramref name="connection"/>.
        /// </summary>
        /// <param name="connection">The connection to the data source.</param>
        /// <param name="commandText">The text command to execute.</param>
        /// <param name="commandType">One of the <see cref="CommandType"/> values. The default is <c>Text</c>.</param>
        /// <param name="timeout">The time to wait for the command to execute.</param>
        /// <param name="transaction">The transaction within which the Command object of a .NET Framework data provider executes. The default value is a null reference.</param>
        /// <param name="parameters">The parameters of the SQL statement or stored procedure.</param>
        /// <returns>The command.</returns>
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

        #endregion

        #region ExecuteXXX

        public static int ExecuteNonQuery(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, object parameters = null) {
            using (var command = connection.CreateCommand(commandText, commandType, timeout, transaction, parameters)) {
                return command.ExecuteNonQuery();
            }
        }

        public static T ExecuteScalar<T>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, object parameters = null) {
            using (var command = connection.CreateCommand(commandText, commandType, timeout, transaction, parameters)) {
                return command.ExecuteScalar<T>();
            }
        }

        public static T ExecuteScalarOrDefault<T>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, object parameters = null) {
            using (var command = connection.CreateCommand(commandText, commandType, timeout, transaction, parameters)) {
                return command.ExecuteScalarOrDefault<T>();
            }
        }

        public static T? ExecuteNullableScalar<T>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, object parameters = null)
            where T : struct {
            using (var command = connection.CreateCommand(commandText, commandType, timeout, transaction, parameters)) {
                return command.ExecuteNullableScalar<T>();
            }
        }

        #endregion

        #region PrepareXXX

        static IDbCommand Prepare(int parameterCount, out IDbDataParameter[] parameters, IDbConnection connection, string commandText, CommandType commandType, TimeSpan? timeout, IDbTransaction transaction, Func<int, string> nameGenerator) {
            var command = connection.CreateCommand(commandText, commandType, timeout, transaction);
            parameters = command.PrepareParameters(parameterCount, nameGenerator);
            return command;
        }

        static void SetValue(IDbDataParameter parameter, object value) {
            parameter.Value = value ?? DBNull.Value;
        }
        static void SetValues(IDbDataParameter[] parameters, params object[] values) {
            for (int i = 0; i < parameters.Length; i++) {
                parameters[i].Value = values[i] ?? DBNull.Value;
            }
        }

        /// <summary>
        /// Generates a function that executes the query with the specified paramter.
        /// </summary>
        /// <typeparam name="T">The type of the parameter</typeparam>
        /// <param name="connection">The connection to the data source.</param>
        /// <param name="commandText">The text command to execute.</param>
        /// <param name="commandType">One of the <see cref="CommandType"/> values. The default is <c>Text</c>.</param>
        /// <param name="timeout">The time to wait for the command to execute.</param>
        /// <param name="transaction">The transaction within which the Command object of a .NET Framework data provider executes. The default value is a null reference.</param>
        /// <param name="nameGenerator">The name generator used to create the parameter's name.</param>
        /// <returns>The function.</returns>
        public static Func<T, int> PrepareExecuteNonQuery<T>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            var command = connection.CreateCommand(commandText, commandType, timeout, transaction);
            var p = command.PrepareParameter(nameGenerator);
            return p0 => {
                SetValue(p, p0);
                return command.ExecuteNonQuery();
            };
        }

        /// <summary>
        /// Generates a function that executes the query with the specified paramters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the secord parameter</typeparam>
        /// <param name="connection">The connection to the data source.</param>
        /// <param name="commandText">The text command to execute.</param>
        /// <param name="commandType">One of the <see cref="CommandType"/> values. The default is <c>Text</c>.</param>
        /// <param name="timeout">The time to wait for the command to execute.</param>
        /// <param name="transaction">The transaction within which the Command object of a .NET Framework data provider executes. The default value is a null reference.</param>
        /// <param name="nameGenerator">The name generator used to create the parameter's name.</param>
        /// <returns>The function.</returns>
        public static Func<T1, T2, int> PrepareExecuteNonQuery<T1, T2>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            IDbDataParameter[] p;
            var command = Prepare(2, out p, connection, commandText, commandType, timeout, transaction, nameGenerator);

            return (p0, p1) => {
                SetValues(p, p0, p1);
                return command.ExecuteNonQuery();
            };
        }

        /// <summary>
        /// Generates a function that executes the query with the specified paramters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the secord parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <param name="connection">The connection to the data source.</param>
        /// <param name="commandText">The text command to execute.</param>
        /// <param name="commandType">One of the <see cref="CommandType"/> values. The default is <c>Text</c>.</param>
        /// <param name="timeout">The time to wait for the command to execute.</param>
        /// <param name="transaction">The transaction within which the Command object of a .NET Framework data provider executes. The default value is a null reference.</param>
        /// <param name="nameGenerator">The name generator used to create the parameter's name.</param>
        /// <returns>The function.</returns>
        public static Func<T1, T2, T3, int> PrepareExecuteNonQuery<T1, T2, T3>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            IDbDataParameter[] p;
            var command = Prepare(3, out p, connection, commandText, commandType, timeout, transaction, nameGenerator);

            return (p0, p1, p2) => {
                SetValues(p, p0, p1, p2);
                return command.ExecuteNonQuery();
            };
        }

        /// <summary>
        /// Generates a function that executes the query with the specified paramters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the secord parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter</typeparam>
        /// <param name="connection">The connection to the data source.</param>
        /// <param name="commandText">The text command to execute.</param>
        /// <param name="commandType">One of the <see cref="CommandType"/> values. The default is <c>Text</c>.</param>
        /// <param name="timeout">The time to wait for the command to execute.</param>
        /// <param name="transaction">The transaction within which the Command object of a .NET Framework data provider executes. The default value is a null reference.</param>
        /// <param name="nameGenerator">The name generator used to create the parameter's name.</param>
        /// <returns>The function.</returns>
        public static Func<T1, T2, T3, T4, int> PrepareExecuteNonQuery<T1, T2, T3, T4>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            IDbDataParameter[] p;
            var command = Prepare(4, out p, connection, commandText, commandType, timeout, transaction, nameGenerator);

            return (p0, p1, p2, p3) => {
                SetValues(p, p0, p1, p2, p3);
                return command.ExecuteNonQuery();
            };
        }

        /// <summary>
        /// Generates a function that executes the query with the specified paramters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the secord parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter</typeparam>
        /// <param name="connection">The connection to the data source.</param>
        /// <param name="commandText">The text command to execute.</param>
        /// <param name="commandType">One of the <see cref="CommandType"/> values. The default is <c>Text</c>.</param>
        /// <param name="timeout">The time to wait for the command to execute.</param>
        /// <param name="transaction">The transaction within which the Command object of a .NET Framework data provider executes. The default value is a null reference.</param>
        /// <param name="nameGenerator">The name generator used to create the parameter's name.</param>
        /// <returns>The function.</returns>
        public static Func<T1, T2, T3, T4, T5, int> Prepare<T1, T2, T3, T4, T5>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            IDbDataParameter[] p;
            var command = Prepare(5, out p, connection, commandText, commandType, timeout, transaction, nameGenerator);

            return (p0, p1, p2, p3, p4) => {
                SetValues(p, p0, p1, p2, p3, p4);
                return command.ExecuteNonQuery();
            };
        }

        /// <summary>
        /// Generates a function that executes the query with the specified paramters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the secord parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter</typeparam>
        /// <param name="connection">The connection to the data source.</param>
        /// <param name="commandText">The text command to execute.</param>
        /// <param name="commandType">One of the <see cref="CommandType"/> values. The default is <c>Text</c>.</param>
        /// <param name="timeout">The time to wait for the command to execute.</param>
        /// <param name="transaction">The transaction within which the Command object of a .NET Framework data provider executes. The default value is a null reference.</param>
        /// <param name="nameGenerator">The name generator used to create the parameter's name.</param>
        /// <returns>The function.</returns>
        public static Func<T1, T2, T3, T4, T5, T6, int> PrepareExecuteNonQuery<T1, T2, T3, T4, T5, T6>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
        IDbDataParameter[] p;
            var command = Prepare(6, out p, connection, commandText, commandType, timeout, transaction, nameGenerator);

            return (p0, p1, p2, p3, p4, p5) => {
                SetValues(p, p0, p1, p2, p3, p4, p5);
                return command.ExecuteNonQuery();
            };
        }

        /// <summary>
        /// Generates a function that executes the query with the specified paramters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the secord parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter</typeparam>
        /// <param name="connection">The connection to the data source.</param>
        /// <param name="commandText">The text command to execute.</param>
        /// <param name="commandType">One of the <see cref="CommandType"/> values. The default is <c>Text</c>.</param>
        /// <param name="timeout">The time to wait for the command to execute.</param>
        /// <param name="transaction">The transaction within which the Command object of a .NET Framework data provider executes. The default value is a null reference.</param>
        /// <param name="nameGenerator">The name generator used to create the parameter's name.</param>
        /// <returns>The function.</returns>
        public static Func<T1, T2, T3, T4, T5, T6, T7, int> PrepareExecuteNonQuery<T1, T2, T3, T4, T5, T6, T7>(this IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null, Func<int, string> nameGenerator = null) {
            IDbDataParameter[] p;
            var command = Prepare(7, out p, connection, commandText, commandType, timeout, transaction, nameGenerator);

            return (p0, p1, p2, p3, p4, p5, p6) => {
                SetValues(p, p0, p1, p2, p3, p4, p5, p6);
                return command.ExecuteNonQuery();
            };
        }

        /// <summary>
        /// Generates a function that executes the query with the specified paramters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the secord parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter</typeparam>
        /// <typeparam name="T8">The type of the eigth parameter</typeparam>
        /// <param name="connection">The connection to the data source.</param>
        /// <param name="commandText">The text command to execute.</param>
        /// <param name="commandType">One of the <see cref="CommandType"/> values. The default is <c>Text</c>.</param>
        /// <param name="timeout">The time to wait for the command to execute.</param>
        /// <param name="transaction">The transaction within which the Command object of a .NET Framework data provider executes. The default value is a null reference.</param>
        /// <param name="nameGenerator">The name generator used to create the parameter's name.</param>
        /// <returns>The function.</returns>
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
