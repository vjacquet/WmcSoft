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
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WmcSoft.Data
{
    public static partial class DbCommandExtensions
    {
        #region ExecuteXXXAsync

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset
        /// returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>The first column of the first row in the resultset.</returns>
        public static async Task<T> ExecuteScalarAsync<T>(this DbCommand command)
        {
            var result = await command.ExecuteScalarAsync();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static async Task<T> ExecuteScalarOrDefaultAsync<T>(this DbCommand command, T defaultValue = default(T))
        {
            var result = await command.ExecuteScalarAsync();
            if (result == null || DBNull.Value.Equals(result))
                return defaultValue;
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static async Task<T?> ExecuteNullableScalarAsync<T>(this DbCommand command) where T : struct
        {
            var result = await command.ExecuteScalarAsync();
            if (result == null || DBNull.Value.Equals(result))
                return null;
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static async Task<object> ExecuteStoredProcedureAsync(this DbCommand command)
        {
            Debug.Assert(command.CommandType == CommandType.StoredProcedure);

            var result = command.Parameters.Cast<IDbDataParameter>().FirstOrDefault(p => p.Direction == ParameterDirection.ReturnValue);
            if (result == null) {
                result = command.CreateParameter();
                result.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(result);
            }
            await command.ExecuteNonQueryAsync();
            return result.Value;
        }

        public static async Task<T> ExecuteStoredProcedureAsync<T>(this DbCommand command)
        {
            var result = await command.ExecuteStoredProcedureAsync();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static async Task<T> ExecuteStoredProcedureOrDefaultAsync<T>(this DbCommand command, T defaultValue = default(T))
        {
            var result = await command.ExecuteStoredProcedureAsync();
            if (result == null || DBNull.Value.Equals(result))
                return defaultValue;
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static async Task<T?> ExecuteNullableStoredProcedureAsync<T>(this DbCommand command) where T : struct
        {
            var result = await command.ExecuteStoredProcedureAsync();
            if (result == null || DBNull.Value.Equals(result))
                return null;
            return (T)Convert.ChangeType(result, typeof(T));
        }

        #endregion
    }
}
