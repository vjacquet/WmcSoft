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
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace WmcSoft.Data
{
    public static partial class DbTransactionExtensions
    {
        #region Factory methods

        /// <summary>
        /// Creates a command to run against the <paramref name="connection"/>.
        /// </summary>
        /// <param name="transaction">The transaction within which the Command object of a .NET Framework data provider executes. The default value is a null reference.</param>
        /// <param name="commandText">The text command to execute.</param>
        /// <param name="commandType">One of the <see cref="CommandType"/> values. The default is <c>Text</c>.</param>
        /// <param name="timeout">The time to wait for the command to execute.</param>
        /// <param name="parameters">The parameters of the SQL statement or stored procedure.</param>
        /// <returns>The command.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities")]
        public static IDbCommand CreateCommand(this IDbTransaction transaction, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, object parameters = null)
        {
            var command = transaction.Connection.CreateCommand(timeout, transaction);
            command.CommandType = commandType;
            command.CommandText = commandText;
            command.WithParameters(parameters);
            return command;
        }

        public static IDbCommand CreateCommand(this IDbTransaction transaction, TimeSpan? timeout = null)
        {
            var command = transaction.Connection.CreateCommand();
            command.Transaction = transaction;
            if (timeout != null)
                command.CommandTimeout = (int)Math.Max(timeout.GetValueOrDefault().TotalSeconds, 1d);
            return command;
        }

        #endregion

        #region StoredProc

        public static T ExecuteStoredProcedure<T>(this IDbTransaction transaction, string name, TimeSpan? timeout = null, object parameters = null)
        {
            return transaction.Connection.ExecuteStoredProcedure<T>(name, timeout, transaction, parameters);
        }

        #endregion

        #region ExecuteXXX

        public static int ExecuteNonQuery(this IDbTransaction transaction, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, object parameters = null)
        {
            using (var command = transaction.Connection.CreateCommand(commandText, commandType, timeout, transaction, parameters)) {
                return command.ExecuteNonQuery();
            }
        }

        public static T ExecuteScalar<T>(this IDbTransaction transaction, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, object parameters = null)
        {
            using (var command = transaction.Connection.CreateCommand(commandText, commandType, timeout, transaction, parameters)) {
                return command.ExecuteScalar<T>();
            }
        }

        public static T ExecuteScalarOrDefault<T>(this IDbTransaction transaction, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, object parameters = null, T defaultValue = default(T))
        {
            using (var command = transaction.Connection.CreateCommand(commandText, commandType, timeout, transaction, parameters)) {
                return command.ExecuteScalarOrDefault(defaultValue);
            }
        }

        public static T? ExecuteNullableScalar<T>(this IDbTransaction transaction, string commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, object parameters = null)
            where T : struct
        {
            using (var command = transaction.Connection.CreateCommand(commandText, commandType, timeout, transaction, parameters)) {
                return command.ExecuteNullableScalar<T>();
            }
        }

        #endregion

        #region ReadAll

        public static IEnumerable<IDataRecord> ReadAll(this IDbTransaction transaction, string commandText, CommandType commandType = CommandType.Text, CommandBehavior behavior = CommandBehavior.Default, TimeSpan? timeout = null, object parameters = null)
        {
            using (var command = transaction.Connection.CreateCommand(commandText, commandType, timeout, transaction, parameters))
            using (var reader = command.ExecuteReader(behavior)) {
                while (reader.Read()) {
                    yield return reader;
                }
            }
        }

        #endregion
    }
}
